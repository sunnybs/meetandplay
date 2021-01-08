using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MeetAndPlay.Data.Models.Commons;
using MeetAndPlay.Web.Services;
using MeetAndPlay.Web.ViewModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace MeetAndPlay.Web.Components
{
    public class ImageInputComponent : ComponentBase, IDisposable
    {
        private readonly EventHandler<ValidationStateChangedEventArgs> _validationStateChangedHandler;

        protected ImageInputComponent()
        {
            _validationStateChangedHandler = (sender, eventArgs) => StateHasChanged();
        }
        [CascadingParameter] public EditContext CascadingEditContext { get; set; }
        [Inject] private FileViewModelsService FileViewModelsService { get; set; }
        [Parameter] public string Label { get; set; }
        [Parameter] public IList<FileViewModel> ResultCompressedImages { get; set; } = new List<FileViewModel>();
        [Parameter] public IList<FileViewModel> ResultImages  { get; set; } = new List<FileViewModel>();
        [Parameter] public ICollection<FileWithCompressedCopy> InitialValues { get; set; }
        [Parameter] public bool IsMultiple { get; set; } = false;
        
        private Dictionary<FileViewModel, FileViewModel> _compressedImageToImageMappings = new ();

        protected readonly Dictionary<string, object> InputAttributes = new ();

        private const int MaxAllowedFiles = 3;
        private const string ResizeImageFormat = "image/png";
        
        protected override void OnInitialized()
        {
            if (CascadingEditContext != null)
                CascadingEditContext.OnValidationStateChanged += _validationStateChangedHandler;
            
            if (InitialValues != null && InitialValues.Any())
            {
                foreach (var value in InitialValues)
                {
                    ResultImages.Add(FileViewModelsService.CreateFromDomain(value.File));
                    ResultCompressedImages.Add(FileViewModelsService.CreateFromDomain(value.CompressedFile));
                }

                _compressedImageToImageMappings =
                    FileViewModelsService.CreateCompressedImageToImageMapping(InitialValues);
            }
            
            base.OnInitialized();
            InputAttributes.Add("accept", "image/*");
            if (IsMultiple)
            {
                InputAttributes.Add("multiple", "multiple");
            }
        }

        protected async Task OnInputFileChange(InputFileChangeEventArgs e)
        {
            if (!IsMultiple)
            {
                ResultCompressedImages.Clear();
                ResultImages.Clear();
            }
            
            foreach (var imageFile in e.GetMultipleFiles(MaxAllowedFiles))
            {
                var image = await ReadFileAsync(imageFile, ResultImages);
                var resizedImageFile = await imageFile.RequestImageFileAsync(ResizeImageFormat, 
                    300, 300);
                var compressedImage = await ReadFileAsync(resizedImageFile, ResultCompressedImages);
                _compressedImageToImageMappings.Add(compressedImage, image);
            }
        }

        private async Task<FileViewModel> ReadFileAsync(IBrowserFile file, IList<FileViewModel> destination)
        {
            var buffer = new byte[file.Size];
            await file.OpenReadStream().ReadAsync(buffer);
            var base64DataUrl =
                $"data:{ResizeImageFormat};base64,{Convert.ToBase64String(buffer)}";

            var fileViewModel = new FileViewModel
            {
                Filename = file.Name,
                MimeType = file.ContentType,
                IsNewFile = true,
                FileLink = base64DataUrl
            };
            destination.Add(fileViewModel);

            return fileViewModel;
        }

        protected void DeleteImage(FileViewModel compressedImage)
        {
            ResultCompressedImages.Remove(compressedImage);
            var sourceImage = _compressedImageToImageMappings[compressedImage];
            ResultImages.Remove(sourceImage);
            StateHasChanged();
        }

        public void Dispose()
        {
            if (CascadingEditContext != null) 
                CascadingEditContext.OnValidationStateChanged -= _validationStateChangedHandler;
        }
    }
}