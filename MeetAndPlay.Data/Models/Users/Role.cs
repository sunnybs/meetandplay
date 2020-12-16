using System;
using MeetAndPlay.Data.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace MeetAndPlay.Data.Models.Users
{
    public class Role : IdentityRole<Guid>, INamedEntity
    {
    }
}