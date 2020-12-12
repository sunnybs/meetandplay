using System;

namespace MeetAndPlay.Data.Interfaces
{
    public interface IIdentifiableEntity
    {
        Guid Id { get; set; }
    }
}