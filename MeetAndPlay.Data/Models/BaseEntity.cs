using System;
using MeetAndPlay.Data.Interfaces;

namespace MeetAndPlay.Data.Models
{
    public class BaseEntity : IIdentifiableEntity
    {
        public Guid Id { get; set; }
    }
}