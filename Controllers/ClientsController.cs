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
    public class ClientsController : Controller
    {
        private readonly IClientService _serviceClients;
        private readonly ICacheService _cacheService;

        public ClientsController(IClientService service, ICacheService cacheService)
        {
            _serviceClients = service;
            _cacheService = cacheService;
        }

        [HttpGet]
        public IActionResult GetClients()
        {
            try
            {
                var cacheResult = _cacheService.GetFromCache(CacheKeys.Client);
                if (cacheResult != null)
                {
                    return Ok(cacheResult);
                }
                else
                {
                    var items = _serviceClients.GetClients();
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
        public IActionResult GetClient(Guid id)
        {
            try
            {
                var result = _serviceClients.GetClient(id);
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
        public IActionResult CreateClient(ClientIn client)
        {

            try
            {
                var result = _serviceClients.CreateClient(client);
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
        public IActionResult EditClient([FromBody] ClientIn client, Guid id)
        {
            try
            {
                if (client.Id == id)
                {
                    var result = _serviceClients.UpdateClient(client, id);

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
        public IActionResult DeleteClient(Guid id)
        {
            if (_serviceClients.DeleteClient(id))
            {
                return StatusCode(204);
            }

            return NotFound(new ControllerMessage("There is nothing to delete."));
        }
    }
}
