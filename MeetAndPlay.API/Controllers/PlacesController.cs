using System;
using System.Threading.Tasks;
using MeetAndPlay.Core.Abstraction.Services;
using MeetAndPlay.Data.DTO;
using MeetAndPlay.Data.DTO.OfferAggregator;
using MeetAndPlay.Data.Models.Offers;
using Microsoft.AspNetCore.Mvc;

namespace MeetAndPlay.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PlacesController : ControllerBase
    {
        private readonly IPlaceService _placeService;

        public PlacesController(IPlaceService placeService)
        {
            _placeService = placeService;
        }
        
        [HttpGet("ExecuteSeeding")]
        public async Task ExecuteSeeding()
        {
            await _placeService.ExecuteSeeding();
        }
        
        [HttpGet]
        public async Task<CountArray<AggregatedOfferDto>> GetAll()
        {
            return await _placeService.AggregateOffersAsync(new OffersFilterDto());
        }
    }
}