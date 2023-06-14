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

        [HttpGet("id:int", Name = "GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<VillaDto> GetVilla(int id)
        {
            if (id == 0) return BadRequest(string.Empty);

            var villa = VillaStore.villaList.FirstOrDefault(x => x.Id == id);

            if (villa == null) return NotFound();

            return Ok(villa);

        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<VillaDto> CrearVilla([FromBody]VillaDto villaDto) 
        {
            if (villaDto == null) return BadRequest(villaDto);

            if (villaDto.Id > 0) return StatusCode(StatusCodes.Status500InternalServerError);

            villaDto.Id = VillaStore.villaList.OrderByDescending(x => x.Id).FirstOrDefault().Id + 1;

            VillaStore.villaList.Add(villaDto);

            return CreatedAtRoute();

        }
    }
}
