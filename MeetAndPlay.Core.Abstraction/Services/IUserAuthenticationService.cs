namespace MeetAndPlay.Core.Abstraction.Services
{
    public interface IUserAuthenticationService
    {
        public string GetCurrentUserName();
        public bool IsAuthenticated();
        void Challenge();
    }
}