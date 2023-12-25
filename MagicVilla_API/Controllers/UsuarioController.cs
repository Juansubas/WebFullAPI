using Asp.Versioning;
using MagicVilla_API.Modelos;
using MagicVilla_API.Modelos.Dto;
using MagicVilla_API.Repositorio.IRepositorio;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace MagicVilla_API.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersionNeutral]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        private APIResponse _response;
        public UsuarioController(IUsuarioRepositorio usuarioRepositorio) 
        { 
            _usuarioRepositorio = usuarioRepositorio;
            _response = new APIResponse();
        }

        [HttpPost("login")] //api/usuario/login
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO modelo)
        {
            var loginResponse = await _usuarioRepositorio.Login(modelo);
            if (loginResponse.Usuario == null || string.IsNullOrEmpty(loginResponse.Token))
            {
                _response.statusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("UserName o PassWord son Incorrecto");
                return BadRequest(_response);
            }

            _response.IsSuccess = true;
            _response.statusCode = HttpStatusCode.OK;
            _response.Resultado = loginResponse;
            return Ok(_response);

        }

        [HttpPost("registrar")] //api/usuario/registrar
        public async Task<IActionResult> Registrar([FromBody] RegistroRequestDTO modelo)
        {
            bool isUsuarioUnico = _usuarioRepositorio.IsUsuarioUnico(modelo.UserName);

            if (!isUsuarioUnico)
            {
                _response.statusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Usuario ya Existe!");
                return BadRequest(_response);
            }

            var usuario = await _usuarioRepositorio.Registrar(modelo);

            if (usuario == null) 
            {
                _response.statusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Error al registrar Usuario");
                return BadRequest(_response);
            }

            _response.IsSuccess = true;
            _response.statusCode = HttpStatusCode.OK;
            return Ok(_response);
        }
    }
}
