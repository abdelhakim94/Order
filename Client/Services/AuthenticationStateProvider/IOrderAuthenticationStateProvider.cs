using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using Order.Shared.Interfaces;

namespace Order.Client.Services
{
    public interface IOrderAuthenticationStateProvider : IService
    {
        Task<AuthenticationState> GetAuthenticationStateAsync();
        Task MarkUserAsSignedIn(string accessToken, string refreshToken);
        Task MarkUserAsSignedOut();
    }
}
