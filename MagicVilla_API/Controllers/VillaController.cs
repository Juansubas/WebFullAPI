using MagicVilla_API.Modelos;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VillaController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<Villa> GetVillas()
        {
            return new List<Villa>
            {
                new Villa{Id=1, Name="Vista a la Piscina"},
                new Villa{Id=2, Name="Vista a la Playa"}
            };
        }
    }
}
