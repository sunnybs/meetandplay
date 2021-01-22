using MeetAndPlay.Core.Abstraction.Services;

namespace MeetAndPlay.Core.Services
{
    public class MockAuthenticationService : IUserAuthenticationService
    {
        public string GetCurrentUserName()
        {
            throw new System.NotImplementedException();
        }

        public bool IsAuthenticated()
        {
            throw new System.NotImplementedException();
        }

        public void Challenge()
        {
            throw new System.NotImplementedException();
        }
    }
}