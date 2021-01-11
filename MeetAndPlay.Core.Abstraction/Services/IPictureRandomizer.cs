namespace MeetAndPlay.Core.Abstraction.Services
{
    public interface IPictureRandomizer
    {
        string GetRandomLobbyPictureLink();
        string GetRandomPlayerPictureLink();
        string GetRandomOfferPictureLink();
    }
}