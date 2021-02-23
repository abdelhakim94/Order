using System.Threading.Tasks;
using IdentityModel.Client;
using Order.Application.Shared.Dto.Users;

namespace Order.Application.Client.Services
{
    public interface IAuthenticationService
    {
        Task<SignUpResultDto> SignUp(UserSignUpDto userSignUpData);
        Task<SignInResultDto> SignIn(UserSignInDto userSignInData, DiscoveryDocumentResponse discoveryDocument);
        Task SignOut();
    }
}
