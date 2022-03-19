namespace WebApi.Features.Login
{
    public class LoginRequest
    {
        public LoginRequest()
        {
            this.Username = string.Empty;
            this.Password = string.Empty;
        }

        public string Password { get; set; }
        public string Username { get; set; }
    }
}