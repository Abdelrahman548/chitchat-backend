using ChitChat.Service.Helpers;
using ChitChat.Service.Interfaces;

namespace ChitChat.Service.Implementations
{
    public class OTPService : IOTPService
    {
        public string GenerateOTP(int length = 6)
        {
            var random = new Random();
            return random.Next((int)Math.Pow(10, length - 1), (int)Math.Pow(10, length)).ToString();
        }

        public string GetBodyTemplate(string otp, int time, string companyName)
        {
            string logoUrl = "https://res.cloudinary.com/ddokqzy9r/image/upload/v1740418717/omsk73div6rtj5ek0wy7.png";
            return OTPTemplate.GetTemplate(otp, time, companyName, logoUrl);
        }
    }
}
