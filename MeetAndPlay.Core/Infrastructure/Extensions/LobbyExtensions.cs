using System.Collections.Generic;
using System.Linq;
using MeetAndPlay.Core.Infrastructure.Helpers;
using MeetAndPlay.Data.Models.Offers;

namespace MeetAndPlay.Core.Infrastructure.Extensions
{
    public static class LobbyExtensions
    {
        public static string BuildTitle(this Lobby lobby)
        {
            var titleParts = new List<string>();
            titleParts.Add("Поиск компании");
            var games = lobby.LobbyGames?.Select(lg => lg.Game.Name).JoinAsString(", ");
            if (!games.IsNullOrWhiteSpace())
                titleParts.Add(games);
            var requestedPlayersCount = lobby.MaxPlayersCount - lobby.CurrentPlayersCount;
            var requestedPlayers = $"нужно еще {requestedPlayersCount} {GetPlayersDeclension(requestedPlayersCount)}";
            titleParts.Add(requestedPlayers);
            return titleParts.JoinAsString(", ");
        }
        
        private static string GetPlayersDeclension(int count)
        {
            return Commons.GetDeclension(count, "игрок", "игрока", "игроков");
        }
    }
}