using Asp.Versioning;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace VersioningMicroservice.API.Controllers
{
    [ApiVersion(1)]
    [ApiVersion(2)]
    [Route("api/[controller]")]
    [Route("api/v{v:apiVersion}/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        [MapToApiVersion(1)]
        [HttpGet("{id}")]
        public IActionResult GetV1(int id)
        {
            return Ok($"Version 1.0- {id}");
        }

        [HttpGet("{id}")]
        [MapToApiVersion(2)]
        public IActionResult GetV2(int id)
        {
            return Ok($"Version 2.0 - {id}");
        }
    }
}