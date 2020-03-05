using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using mservicesample.Search.Api.Dtos.Queries;

namespace mservicesample.Search.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArtistsController : ControllerBase
    {
        private readonly IMediator _bus;

        public ArtistsController(IMediator bus)
        {
            _bus = bus;
        }


        // GET api/values
        [HttpGet()]
        [AllowAnonymous]
        public async Task<ActionResult> SearchAsync([FromQuery] string q,int page=1,int pagesize=10)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(await _bus.Send(new FindArtistQuery {QueryText = q, PageSize = pagesize, Page = page}));
        }
    }
}
