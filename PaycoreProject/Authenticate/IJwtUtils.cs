using PaycoreProject.Helpers;
using PaycoreProject.Model;

namespace PaycoreProject.Authenticate
{
    public interface IJwtUtils
    {
       
        BaseResponse<AuthenticateResponse> GenerateToken(AuthenticateRequest tokenRequest);

    }
}
