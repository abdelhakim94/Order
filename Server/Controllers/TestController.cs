using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using Order.Shared.Dto.Account;
using Order.Server.Services;
using Order.Shared.Security;
using Order.Server.Dto.Users;
using Order.Server.Middlewares;
using Order.Shared.Dto;
using Order.Server.Dto;
using Order.Shared.Dto.Chef;
using Order.Shared.Dto.Dish;

namespace Order.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class TestController : ControllerBase
    {
        private IChefService chefService;
        private IDishService dishService;

        public TestController(IChefService chefService, IDishService dishService)
        {
            this.chefService = chefService;
            this.dishService = dishService;
        }

        [HttpPost]
        [AllowAnonymous]
        public Task<PaginatedList<ChefDetailsDto>> SearchForChefs(ChefsSearchFilter filter)
        {
            return chefService.SearchForChefs(filter);
        }

        [HttpPost]
        [AllowAnonymous]
        public Task<PaginatedList<DishOrMenuDetailsDto>> SearchForDishesAndMenues(DishesOrMenuesSearchFilter filter)
        {
            return dishService.SearchForDishesAndMenues(filter);
        }
    }
}
