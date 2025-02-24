namespace ChitChat.Service.Interfaces
{
    public interface IOTPService
    {
        string GenerateOTP(int length = 6);
        string GetBodyTemplate(string otp, int time, string companyName);
    }
}
