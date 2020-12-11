using System;
using MeetAndPlay.Data.Enums;

namespace MeetAndPlay.Data.DTO.OfferAggregator
{
    public record AggregatedOfferDto
        (
            Guid Id,
            OfferType OfferType,
            string Title,
            string Description,
            string PosterUrl,
            string[] AdditionalImages,
            DateTime? ActualOfferDate
        );
}