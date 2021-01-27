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
    public class UserOfferController : ControllerBase
    {
        private readonly IUserOfferService _userOfferService;

        public UserOfferController(IUserOfferService userOfferService)
        {
            _userOfferService = userOfferService;
        }
        
        [HttpGet]
        public async Task<CountArray<AggregatedOfferDto>> GetAll()
        {
            return await _userOfferService.AggregateOffersAsync(new OffersFilterDto());
        }

        [HttpGet("{id}")]
        public async Task<UserOffer> GetById(Guid id)
        {
            return await _userOfferService.GetByIdAsync(id);
        }
    }
}