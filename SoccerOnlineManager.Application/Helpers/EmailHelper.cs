using System;
using System.Net.Mail;

namespace SoccerOnlineManager.Application.Helpers
{
    public static class EmailHelper
    {
        public static bool IsValid(string email)
        {
            try
            {
                var m = new MailAddress(email);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}
