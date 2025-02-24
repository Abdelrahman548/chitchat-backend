namespace ChitChat.Service.Helpers
{
    public class EmailOptions
    {
        public string SmtpServer { get; set; }
        public string SenderEmail { get; set; }
        public string SenderName { get; set; }
        public string AppPassword { get; set; }
        public int PortSSL { get; set; }
        public int Port { get; set; }

    }
}
