namespace MeetAndPlay.Core.Infrastructure.Helpers
{
    public static class Commons
    {
        public static string GetDeclension(int number, string nominative, string genitive, string plural)
        {
            number %= 100;
            if (number >= 11 && number <= 19)
            {
                return plural;
            }

            var i = number % 10;
            switch (i)
            {
                case 1:
                    return nominative;
                case 2:
                case 3:
                case 4:
                    return genitive;
                default:
                    return plural;
            }

        }
    }
}