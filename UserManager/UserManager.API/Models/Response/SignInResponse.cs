namespace UserManager.API.Models.Response
{
    public class SignInResponse
    {
        public string AccessToken { get; set; }
        public int ExpiresIn { get; set; }
    }
}
