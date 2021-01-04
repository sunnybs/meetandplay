using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using MeetAndPlay.Core.Abstraction.Services;
using MeetAndPlay.Core.Abstraction.Services.ReadService;
using MeetAndPlay.Core.Infrastructure.Extensions;
using MeetAndPlay.Data.DTO;
using MeetAndPlay.Data.DTO.ReadFilters;
using MeetAndPlay.Web.Components.Select2.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;

namespace MeetAndPlay.Web.Components.Select2
{
    public class Select2Base<TItem> : ComponentBase, IDisposable
    {
        private readonly JsonSerializerOptions _jsonSerializerOptions =
            new() {PropertyNamingPolicy = JsonNamingPolicy.CamelCase};

        private readonly EventHandler<ValidationStateChangedEventArgs> _validationStateChangedHandler;
        private DotNetObjectReference<Select2Base<TItem>> _elementRef;
        private Type _nullableUnderlyingType;
        private ValidationMessageStore _parsingValidationMessages;
        private bool _previousParsingAttemptFailed;
        private IReadService<TItem> _source;

        /// <summary>
        ///     Constructs an instance of <see cref="Select2Base{TItem}" />.
        /// </summary>
        protected Select2Base()
        {
            _validationStateChangedHandler = (sender, eventArgs) => StateHasChanged();
        }

        [Inject] private IJSRuntime JSRuntime { get; set; }

        [CascadingParameter] public EditContext CascadingEditContext { get; set; }

        [Parameter] public EditContext EditContext { get; set; }

        [Parameter] public string Id { get; set; }

        [Parameter] public bool IsDisabled { get; set; }

        [Parameter] public bool IsMultiple { get; set; }

        [Parameter] public bool AllowClear { get; set; }

        [Parameter] public Func<TItem, bool> IsOptionDisabled { get; set; } = item => false;


        [Parameter]
        public IReadService<TItem> Source
        {
            get => _source;
            set
            {
                _source = value;
                GetPagedData = query => _source.GetAsync(new ReadFilter
                    {PageNumber = query.Page, PageSize = query.Size, SearchTerm = query.Term});
            }
        }

        [Parameter] public TItem[] Data { get; set; }

        [Parameter] public Func<Select2QueryData, Task<TItem[]>> GetPagedData { get; set; }

        [Parameter] public Func<TItem, string> OptionTemplate { get; set; }

        [Parameter] public Func<TItem, string> TextExpression { get; set; } = item => item.ToString();

        [Parameter] public string Placeholder { get; set; } = "Выберите...";

        [Parameter] public string Theme { get; set; } = "bootstrap";
        
        [Parameter] public HashSet<TItem> Values { get; set; }

        [Parameter] public EventCallback<HashSet<TItem>> ValuesChanged { get; set; }

        protected Dictionary<string, TItem> InternallyMappedData { get; set; } = new();

        protected string FieldClass => "form-group";

        protected EditContext GivenEditContext { get; set; }

        protected HashSet<TItem> CurrentValues
        {
            get => Values;
            set
            {
                _ = SelectItems(value);

                var hasChanged = value.Count != Values.Count || !Values.SetEquals(value);
                if (!hasChanged) return;

                Values = value;
                _ = ValuesChanged.InvokeAsync(value);
            }
        }

        void IDisposable.Dispose()
        {
            if (GivenEditContext != null) GivenEditContext.OnValidationStateChanged -= _validationStateChangedHandler;

            Dispose(true);
        }

        public void Refresh()
        {
            StateHasChanged();
        }

        private bool TryParseValueFromString(string value, out TItem result)
        {
            result = default;

            if (value == "null" || string.IsNullOrEmpty(value))
                return AllowClear;

            if (!InternallyMappedData.ContainsKey(value))
                return false;

            result = InternallyMappedData[value];
            return true;
        }

        private bool TryParseValues(string[] values, out HashSet<TItem> result)
        {
            result = new HashSet<TItem>();
            foreach (var value in values)
            {
                if (value == "null" || value.IsNullOrWhiteSpace())
                    continue;

                if (!InternallyMappedData.ContainsKey(value))
                    return false;

                result.Add(InternallyMappedData[value]);
            }

            return true;
        }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            _elementRef = DotNetObjectReference.Create(this);
        }

        public override Task SetParametersAsync(ParameterView parameters)
        {
            parameters.SetParameterProperties(this);

            _nullableUnderlyingType = Nullable.GetUnderlyingType(typeof(TItem));
            GivenEditContext = EditContext ?? CascadingEditContext;
            if (GivenEditContext != null)
                GivenEditContext.OnValidationStateChanged += _validationStateChangedHandler;

            GetPagedData ??= GetStaticData;

            // For derived components, retain the usual lifecycle with OnInit/OnParametersSet/etc.
            return base.SetParametersAsync(ParameterView.Empty);
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (firstRender)
            {
                var options = JsonSerializer.Serialize(new
                {
                    placeholder = Placeholder,
                    allowClear = AllowClear,
                    theme = Theme,
                    multiple = IsMultiple
                }, _jsonSerializerOptions);

                await JSRuntime.InvokeVoidAsync("select2Blazor.init",
                    Id, _elementRef, options, "select2Blazor_GetData");

                if (CurrentValues != null)
                    await SelectItems(CurrentValues);

                var changeCallback = IsMultiple ? "select2Blazor_OnChange_multiple" : "select2Blazor_OnChange";
                await JSRuntime.InvokeVoidAsync("select2Blazor.onChange",
                    Id, _elementRef, changeCallback);
            }
        }

        private Task<TItem[]> GetStaticData(Select2QueryData query)
        {
            if (query.Page != 1)
                return Task.FromResult(default(TItem[]));

            var data = Data;
            var searchTerm = query.Term;
            if (!string.IsNullOrWhiteSpace(searchTerm))
                data = data
                    .Where(x => TextExpression(x).Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                    .ToArray();
            return Task.FromResult(data);
        }

        private async Task SelectItems(IEnumerable<TItem> items)
        {
            foreach (var item in items.Where(i => i != null)) await SelectItem(item);
        }

        private async Task SelectItem(TItem item)
        {
            var mappedItem = MapToSelect2Item(item);
            InternallyMappedData[mappedItem.Id] = item;
            await JSRuntime.InvokeVoidAsync("select2Blazor.select", Id, mappedItem);
        }

        private Select2Item MapToSelect2Item(TItem item)
        {
            var id = GetId(item);
            var select2Item = new Select2Item(id, TextExpression(item), IsOptionDisabled(item));
            if (OptionTemplate != null)
                select2Item.Html = OptionTemplate(item);
            if (Values != null)
                select2Item.Selected = Values.Select(GetId).Contains(id);
            return select2Item;
        }

        [JSInvokable("select2Blazor_GetData")]
        public async Task<string> Select2_GetDataWrapper(JsonElement element)
        {
            var json = element.GetRawText();
            var queryParams = JsonSerializer.Deserialize<Select2QueryParams>(json, _jsonSerializerOptions);

            var data = await GetPagedData(queryParams.Data);

            if (!queryParams.Data.Type.Contains("append", StringComparison.OrdinalIgnoreCase))
                InternallyMappedData.Clear();

            var response = new Select2Response();
            if (data == null) 
                return JsonSerializer.Serialize(response, _jsonSerializerOptions);
            
            foreach (var item in data)
            {
                var mappedItem = MapToSelect2Item(item);
                InternallyMappedData[mappedItem.Id] = item;
                response.Results.Add(mappedItem);
            }

            response.Pagination.More = data.Length == queryParams.Data.Size;

            return JsonSerializer.Serialize(response, _jsonSerializerOptions);
        }

        [JSInvokable("select2Blazor_OnChange")]
        public void Change(string value)
        {
            _parsingValidationMessages?.Clear();

            bool parsingFailed;

            if (_nullableUnderlyingType != null && string.IsNullOrEmpty(value))
            {
                // Assume if it's a nullable type, null/empty inputs should correspond to default(T)
                // Then all subclasses get nullable support almost automatically (they just have to
                // not reject Nullable<T> based on the type itself).
                parsingFailed = false;
                CurrentValues = default;
            }
            else if (TryParseValueFromString(value, out var parsedValue))
            {
                parsingFailed = false;
                CurrentValues = new HashSet<TItem> {parsedValue};
            }
            else
            {
                parsingFailed = true;

                if (_parsingValidationMessages == null)
                    _parsingValidationMessages = new ValidationMessageStore(GivenEditContext);
            }

            // We can skip the validation notification if we were previously valid and still are
            if (parsingFailed || _previousParsingAttemptFailed)
            {
                GivenEditContext?.NotifyValidationStateChanged();
                _previousParsingAttemptFailed = parsingFailed;
            }
        }

        [JSInvokable("select2Blazor_OnChange_multiple")]
        public void ChangeMultiple(string[] values)
        {
            _parsingValidationMessages?.Clear();

            bool parsingFailed;

            if (_nullableUnderlyingType != null && values?.Any() == false)
            {
                // Assume if it's a nullable type, null/empty inputs should correspond to default(T)
                // Then all subclasses get nullable support almost automatically (they just have to
                // not reject Nullable<T> based on the type itself).
                parsingFailed = false;
                CurrentValues = default;
            }
            else if (TryParseValues(values, out var parsedValues))
            {
                parsingFailed = false;
                CurrentValues = parsedValues;
            }
            else
            {
                parsingFailed = true;

                if (_parsingValidationMessages == null)
                    _parsingValidationMessages = new ValidationMessageStore(GivenEditContext);

                //_parsingValidationMessages.Add(FieldIdentifier, "Given value was not found");

                // Since we're not writing to CurrentValue, we'll need to notify about modification from here
                //GivenEditContext?.NotifyFieldChanged(FieldIdentifier);
            }

            // We can skip the validation notification if we were previously valid and still are
            if (parsingFailed || _previousParsingAttemptFailed)
            {
                GivenEditContext?.NotifyValidationStateChanged();
                _previousParsingAttemptFailed = parsingFailed;
            }
        }

        private static string GetId(TItem item)
        {
            return item.GetHashCode().ToString();
        }

        protected virtual void Dispose(bool disposing)
        {
        }
    }
}