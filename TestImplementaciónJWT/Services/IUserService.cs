
using TestImplementaciónJWT.Models.Request;
using TestImplementaciónJWT.Models.Response;

namespace TestImplementaciónJWT.Services
{
    public interface IUserService
    {
        UserResponse Auth(AuthRequest model);
    }
}
