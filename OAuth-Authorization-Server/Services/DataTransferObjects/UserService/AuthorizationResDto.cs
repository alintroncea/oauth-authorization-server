namespace OAuth_Authorization_Server.Services.DataTransferObjects.UserService
{
    public class AuthorizationResDto
    {
        public string RedirectUri { get; set; }

        public string Scope { get; set; }

        public string State { get; set; }
    }
}
