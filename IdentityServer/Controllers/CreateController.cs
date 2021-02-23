using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Order.DomainModel;
using Order.IdentityServer.Persistence;

namespace IdentityServer.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class CreateController : ControllerBase
    {
        private readonly ISContext context;

        public CreateController(ISContext context)
        {
            this.context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Client(Client[] clients)
        {
            await context.Clients.AddRangeAsync(clients.Select(c => c.ToEntity()));
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> IdentityResource(IdentityResource[] resources)
        {
            await context.IdentityResources.AddRangeAsync(resources.Select(r => r.ToEntity()));
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> ApiResource(ApiResource[] resources)
        {
            await context.ApiResources.AddRangeAsync(resources.Select(r => r.ToEntity()));
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> ApiScopes(ApiScope[] scopes)
        {
            await context.ApiScopes.AddRangeAsync(scopes.Select(r => r.ToEntity()));
            await context.SaveChangesAsync();
            return Ok();
        }
    }
}
