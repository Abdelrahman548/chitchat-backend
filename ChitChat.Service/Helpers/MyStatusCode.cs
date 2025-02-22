namespace ChitChat.Service.Helpers
{
    public enum MyStatusCode
    {
        OK = 200,
        Created = 201,
        BadRequest = 400,
        Unauthorized = 401,
        Forbidden = 403,
        NotFound = 404,
        Conflict = 409,
        InternalServerError = 500,
        TooManyRequests = 429
    }
}
