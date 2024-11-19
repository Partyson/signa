using System.Text;

namespace signa.FunctionalTests.Helpers;

public static class MailGeneratorHelper
{
    private static Random random = new ();
    private static string alphabet = "abcdefghijklmnopqrstuvwxyz";
    private static string digits = "0123456789";
    
    public static string Generate()
    {
        var username = new StringBuilder();
        
        for (var i = 0; i < 8; i++)
        {
            var chooseLetter = random.Next(2) == 0;
            if (chooseLetter)
                username.Append(alphabet[random.Next(alphabet.Length)]);
            else
                username.Append(digits[random.Next(digits.Length)]);
        }
        
        return username + "@example.com";
    }
}