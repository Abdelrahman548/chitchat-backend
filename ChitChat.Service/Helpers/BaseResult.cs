namespace ChitChat.Service.Helpers
{
    public class BaseResult<T>
    {
        public bool IsSuccess { get; set; } = true;
        public string Message { get; set; } = string.Empty;
        public List<string>? Errors { get; set; }
        public T? Data { get; set; }
        public MyStatusCode StatusCode { get; set; } = MyStatusCode.OK;
    }
}
