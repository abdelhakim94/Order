using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using Order.Shared.Contracts;

namespace Order.Client.Services
{
    public interface IOrderAuthenticationStateProvider : IScopedService
    {
        Task<AuthenticationState> GetAuthenticationStateAsync();
        Task MarkUserAsSignedIn(string accessToken, string refreshToken);
        Task MarkUserAsSignedOut();
    }
}
