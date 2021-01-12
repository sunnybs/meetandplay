using System;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using MeetAndPlay.Data.DTO.ReadFilters;
using MeetAndPlay.Data.Enums;

namespace MeetAndPlay.Data.DTO.OfferAggregator
{
    public class OffersFilterDto : ReadFilter
    {
        private OfferType? _offerType;
        private DateTime? _from;
        private DateTime? _to;
        private string _gameName;
        private GameLevel? _gameLevel;
        private int? _ageFrom;
        private int? _ageTo;
        private PlaceType? _placeType;

        public OfferType? OfferType
        {
            get => _offerType;
            set
            {
                _offerType = value;
                StateHasChanged?.Invoke();
            }
        }

        public DateTime? From
        {
            get => _from;
            set
            {
                _from = value;
                StateHasChanged?.Invoke();
            }
        }

        public DateTime? To
        {
            get => _to;
            set
            {
                _to = value;
                StateHasChanged?.Invoke();
            }
        }

        public string GameName
        {
            get => _gameName;
            set
            {
                _gameName = value;
                StateHasChanged?.Invoke();
            } 
        }

        public GameLevel? GameLevel
        {
            get => _gameLevel;
            set
            {
                _gameLevel = value;
                StateHasChanged?.Invoke();
            }
        }

        public int? AgeFrom
        {
            get => _ageFrom;
            set
            {
                _ageFrom = value;
                StateHasChanged?.Invoke();
            }
        }

        public int? AgeTo
        {
            get => _ageTo;
            set
            {
                _ageTo = value;
                StateHasChanged?.Invoke();
            } 
        }

        public PlaceType? PlaceType
        {
            get => _placeType;
            set
            {
                 _placeType = value;
                 StateHasChanged?.Invoke();
            }
        }

        [JsonIgnore] public Action StateHasChanged { get; set; }
    }
}