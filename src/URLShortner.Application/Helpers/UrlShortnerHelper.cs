using System;

namespace URLShortner.Application.Helpers
{
    public class UrlShortnerHelper
    {
        public static string GetUniqueKey()
        {
            string uniqueToken = string.Empty;

            var alphaNumericSet = "AaBbCcDdEeFfGgHhIiJjKkLlMmNnOoPpQqRrSsTtUuVvWwXxYyZz0123456789";

            //Get random value 6 times
            for (int i = 0; i < 6; i++)
            {
                uniqueToken += alphaNumericSet.Substring(new Random().Next(0, alphaNumericSet.Length), 1);
            }

            return uniqueToken;
        }
    }
}
