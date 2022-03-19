using Idea.Service;
using Idea.Service.Models;
using Microsoft.AspNetCore.Mvc;

namespace Idea.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LocationsController : ControllerBase
    {
        private readonly ILocationService locationService;

        private readonly IShipService shipService;

        public LocationsController(ILocationService locationService, IShipService shipService)
        {
            this.locationService = locationService;
            this.shipService = shipService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // TODO: Refactor this with Location View Model
            var allLocations = await this.locationService.GetAllLocations();

            return Ok(allLocations);
        }

        [HttpGet("Here")]
        public async Task<IActionResult> LocationsHere()
        {
            var userId = ((IdeaUserServiceModel)this.HttpContext.Items["User"]).Id;

            var ship = await this.shipService.GetShipByUserIdAsync(userId);

            var locationsHere = await this.locationService.GetLocationsByCoordinate(new CoordinateServiceModel
            {
                X = ship.X,
                Y = ship.Y,
                Z = ship.Z
            });

            // TODO: Refactor this with Location View Model
            return Ok(locationsHere);
        }
    }
}
