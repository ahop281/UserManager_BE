using System.Threading.Tasks;
using UserManager.API.Models.Request;
using UserManager.API.Models.Response;

namespace UserManager.API.Services
{
    public interface IAuthService
    {
        public Task<SignUpResponse> SignUp(SignUpRequest request);
        public Task<SignInResponse> SignIn(SignInRequest request);
    }
}
