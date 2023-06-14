using MagicVilla_API.Datos;
using MagicVilla_API.Modelos;
using MagicVilla_API.Modelos.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VillaController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<VillaDto>> GetVillas()
        {
            return Ok(VillaStore.villaList);
        }

        [HttpGet("id:int")]
        public ActionResult<VillaDto> GetVilla(int id)
        {
            if (id == 0) return BadRequest(string.Empty);

            var villa = VillaStore.villaList.FirstOrDefault(x => x.Id == id);

            if (villa == null) return NotFound();

            return Ok(villa);

        }
    }
}
