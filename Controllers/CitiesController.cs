using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ubank_api.Data.Helpers;
using ubank_api.Data.Models.In;
using ubank_api.Services.Interfaces;

namespace ubank_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CitiesController : Controller
    {
        private readonly ICityService _cityService;
        private readonly ICacheService _cacheService;

        public CitiesController(ICityService service, ICacheService cacheService)
        {
            _cityService = service;
            _cacheService = cacheService;
        }

        [HttpGet]
        public IActionResult GetCities()
        {
            try
            {
                var cacheResult = _cacheService.GetFromCache(CacheKeys.City);
                if (cacheResult != null)
                {
                    return Ok(cacheResult);
                }
                else
                {
                    var items = _cityService.GetCities();
                    if (items != null)
                    {
                        return Ok(items);
                    }
                }
            }
            catch (ArgumentException e)
            {
                return StatusCode(409, new ControllerMessage(e.Message));
            }

            return Ok();
        }

        [HttpGet("{id}")]
        public IActionResult GetCity(Guid id)
        {
            try
            {
                var result = _cityService.GetCity(id);
                if (result != null)
                {
                    return Ok(result);
                }
            }
            catch (ArgumentException e)
            {
                return BadRequest(new ControllerMessage(e.Message));
            }
            return NotFound();
        }

        [HttpPost]
        public IActionResult CreateCity(CityIn city)
        {

            try
            {
                var result = _cityService.CreateCity(city);
                if (result != null)
                {
                    return Ok(result);
                }
            }
            catch (ArgumentException e)
            {
                return BadRequest(new ControllerMessage(e.Message));
            }
            return BadRequest();
        }

        [HttpPut("{id}")]
        public IActionResult EditCity([FromBody] CityIn city, Guid id)
        {
            try
            {
                if (city.Id == id)
                {
                    var result = _cityService.UpdateCity(city, id);

                    if (result != null)
                    {
                        return Ok(result);
                    }

                    return NotFound(new ControllerMessage("There is no client with this id."));
                }
                else
                {
                    return StatusCode(400, new ControllerMessage("Id's provided do not match."));
                }
            }
            catch (ArgumentException e)
            {
                return BadRequest(new ControllerMessage(e.Message));
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteCity(Guid id)
        {
            if (_cityService.DeleteCity(id))
            {
                return StatusCode(204);
            }

            return NotFound(new ControllerMessage("There is nothing to delete."));
        }
    }
}
