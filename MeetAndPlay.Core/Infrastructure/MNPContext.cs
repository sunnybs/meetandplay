using MeetAndPlay.Data.Models.Files;
using MeetAndPlay.Data.Models.Games;
using MeetAndPlay.Data.Models.Offers;
using MeetAndPlay.Data.Models.Places;
using MeetAndPlay.Data.Models.Users;
using Microsoft.EntityFrameworkCore;

namespace MeetAndPlay.Core.Infrastructure
{
    public class MNPContext : DbContext
    {
        public DbSet<File> Files { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<GameGenre> GameGenres { get; set; }
        public DbSet<GameImage> GameImages { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Lobby> Lobbies { get; set; }
        public DbSet<LobbyGame> LobbyGames { get; set; }
        public DbSet<LobbyImage> LobbyImages { get; set; }
        public DbSet<LobbyJoiningRequest> LobbyJoiningRequests { get; set; }
        public DbSet<LobbyPlayer> LobbyPlayers { get; set; }
        public DbSet<UserOffer> UserOffers { get; set; }
        public DbSet<UserOfferGame> UserOfferGames { get; set; }
        public DbSet<UserOfferPeriod> UserOfferPeriods { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Place> Places { get; set; }
        public DbSet<PlaceGame> PlaceGames { get; set; }
        public DbSet<PlaceImage> PlaceImages { get; set; }
        public DbSet<SocialNetwork> SocialNetworks { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserFreePeriod> UserFreePeriods { get; set; }
        public DbSet<UserGame> UserGames { get; set; }
        public DbSet<UserImage> UserImages { get; set; }
        public DbSet<UserSocialNetwork> UserSocialNetworks { get; set; }
         
        public MNPContext(DbContextOptions<MNPContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GameGenre>()
                .HasKey(key => new {key.GameId, key.GenreId});
            
            modelBuilder.Entity<GameImage>()
                .HasKey(key => new {key.GameId, key.FileId});
            
            modelBuilder.Entity<LobbyGame>()
                .HasKey(key => new {key.GameId, key.LobbyId});
            
            modelBuilder.Entity<LobbyImage>()
                .HasKey(key => new {key.FileId, key.LobbyId});
            
            modelBuilder.Entity<LobbyJoiningRequest>()
                .HasKey(key => new {key.UserId, key.LobbyId});
            
            modelBuilder.Entity<LobbyPlayer>()
                .HasKey(key => new {key.PlayerId, key.LobbyId});
            
            modelBuilder.Entity<UserOfferGame>()
                .HasKey(key => new {key.UserOfferId, key.GameId});
            
            modelBuilder.Entity<PlaceGame>()
                .HasKey(key => new {key.PlaceId, key.GameId});
            
            modelBuilder.Entity<PlaceImage>()
                .HasKey(key => new {key.PlaceId, key.FileId});
            
            modelBuilder.Entity<UserGame>()
                .HasKey(key => new {key.UserId, key.GameId});
            
            modelBuilder.Entity<UserImage>()
                .HasKey(key => new {key.UserId, key.FileId});
            
            modelBuilder.Entity<UserSocialNetwork>()
                .HasKey(key => new {key.UserId, key.SocialNetworkId});
        }
    }
}