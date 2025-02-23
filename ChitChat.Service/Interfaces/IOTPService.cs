namespace ChitChat.Service.Interfaces
{
    public interface IOTPService
    {
        string GenerateOTP();
        string GetBodyTemplate(string otp, int time, string companyName);
    }
}
