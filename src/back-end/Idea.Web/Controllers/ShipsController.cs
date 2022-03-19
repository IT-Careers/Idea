using Idea.Service;
using Idea.Service.Models;
using Idea.Web.Mapping;
using Idea.Web.Models.Ships;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Idea.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ShipsController : ControllerBase
    {
        private readonly IShipService shipService;

        public ShipsController(IShipService shipService)
        {
            this.shipService = shipService;
        }

        [HttpGet("Me")]
        [Authorize]
        public async Task<IActionResult> Me()
        {
            var userId = ((IdeaUserServiceModel)this.HttpContext.Items["User"]).Id;

            var ship = (await this.shipService.GetShipByUserIdAsync(userId)).ToSpaceshipMeViewModel();

            return Ok(ship);
        }

        [HttpPost("Travel")]
        [Authorize]
        public async Task<IActionResult> Travel([FromBody] FTLTravelBindingModel fTLTravelBindingModel)
        {
            var userId = ((IdeaUserServiceModel)this.HttpContext.Items["User"]).Id;

            var ship = await this.shipService.GetShipByUserIdAsync(userId);

            ship.X = fTLTravelBindingModel.X;
            ship.Y = fTLTravelBindingModel.Y;
            ship.Z = fTLTravelBindingModel.Z;

            await this.shipService.UpdateShipAsync(ship);

            return Ok(new { Message = "Ok" });
        }
    }
}
