namespace ChitChat.Service.Helpers
{
    public static class OTPTemplate
    {
        public static string GetTemplate(string otpCode, int expirationTime, string companyName, string logoUrl)
        {
            string htmlTemplate = "";
            try
            {
                string basePath = AppDomain.CurrentDomain.BaseDirectory;
                string servicePath = Path.Combine(basePath, "..", "..", "..", "..", "TaskManager.Service");
                string templatePath = Path.Combine(servicePath, "Templates", "otp_template_logo.html");
                htmlTemplate = File.ReadAllText(templatePath);
            }
            catch
            {

            }
            string emailBody = htmlTemplate.Replace("{{OTP_CODE}}", $"{otpCode}");
            emailBody = emailBody.Replace("{{OTP_EXPIRATION_TIME}}", $"{expirationTime} minutes");
            emailBody = emailBody.Replace("{{COMPANY_NAME}}", $"{companyName}");
            emailBody = emailBody.Replace("{{LOGO_URL}}", $"{logoUrl}");
            return emailBody;
        }
    }
}
