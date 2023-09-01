using AutoMapper;
using MagicVilla_API.Datos;
using MagicVilla_API.Modelos;
using MagicVilla_API.Modelos.Dtos;
using MagicVilla_API.Repositorio.IRepositorio;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;

namespace MagicVilla_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VillaController : ControllerBase
    {

        private readonly ILogger<VillaController> _logger;
        private readonly IVillaRepositorio _villaRepo;
        private readonly IMapper _mapper;

        public VillaController(ILogger<VillaController> logger, IVillaRepositorio villaRepo, IMapper mapper) 
        { 
            _logger = logger;
            _villaRepo = villaRepo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<VillaDto>>> GetVillas()
        {
            _logger.LogInformation("Obtener las villas");
            IEnumerable<Villa> villaList = await _villaRepo.ObtenerTodos();

            return Ok(_mapper.Map<IEnumerable<VillaDto>>(villaList));
        }

        [HttpGet("id:int", Name = "GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<VillaDto>> GetVilla(int id)
        {
            if (id == 0) {
                _logger.LogInformation($"Error al crear la villa con Id: { id} ");
                return BadRequest(string.Empty);
            }

            //var villa = VillaStore.villaList.FirstOrDefault(x => x.Id == id);
            var villa = await _villaRepo.Obtener(v => v.Id == id);

            if (villa == null) return NotFound();

            return Ok(_mapper.Map<VillaDto>(villa));

        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<VillaDto>> CrearVilla([FromBody] VillaCreateDto createDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (await _villaRepo.Obtener(v => v.Name.ToLower() == createDto.Name.ToLower()) != null)
            {
                ModelState.AddModelError("NombreExiste", "La villa con ese nombre ya existe");
                return BadRequest(ModelState);
            }

            if (createDto == null) return BadRequest(createDto);


            Villa modelo = _mapper.Map<Villa>(createDto);

            //Cargamos Los Datos
            await _villaRepo.Crear(modelo);

            return CreatedAtRoute("GetVilla", new { id = modelo.Id }, modelo);

        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> EliminarVilla(int id)
        {
            if(id == 0) 
            { 
                return BadRequest();
            }

            var villa = await _villaRepo.Obtener(x => x.Id == id);

            if (villa == null) return NotFound();

            //ANTES SIN ENTITY y sin bd
            //VillaStore.villaList.Remove(villa);

            await _villaRepo.Remover(villa);

            return NoContent();
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> UpdateVilla(int id, [FromBody] VillaUpdateDto updateDto)
        {
            if(updateDto == null || id != updateDto.Id) 
            {
                return BadRequest();
            }

            //var villa = VillaStore.villaList.First(x => x.Id == id);
            //villa.Name = villaDto.Name;
            //villa.Ocupantes = villaDto.Ocupantes;
            //villa.MetrosCuadrados = villaDto.MetrosCuadrados;

            Villa modelo = _mapper.Map<Villa>(updateDto);

             await _villaRepo.Actualizar(modelo);

            return NoContent();
        }

        [HttpPatch("{id:int}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> UpdatePartialVilla(int id, JsonPatchDocument<VillaUpdateDto> patchDto)
        {
            if (patchDto == null || id ==0)
            {
                return BadRequest();
            }
            //Antes del entity
            //var villa = VillaStore.villaList.First(x => x.Id == id);

            var villa = await _villaRepo.Obtener(x => x.Id == id, tracked:false);

            VillaUpdateDto villaUpdateDto = _mapper.Map<VillaUpdateDto>(villa);

            if (villa ==null ) return BadRequest();

            patchDto.ApplyTo(villaUpdateDto, ModelState);

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Villa modelo = _mapper.Map<Villa>(villaUpdateDto);

            await _villaRepo.Actualizar(modelo);

            return NoContent();
        }
    }
}
