using AutoMapper;
using MagicVilla_API.Datos;
using MagicVilla_API.Modelos;
using MagicVilla_API.Modelos.Dto;
using MagicVilla_API.Repositorio.IRepositorio;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MagicVilla_API.Repositorio
{
    public class UsuarioRepositorio : IUsuarioRepositorio
    {
        private readonly ApplicationDbContext _db;
        private string secretKey;
        private readonly UserManager<UsuarioAplicacion> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        public UsuarioRepositorio(ApplicationDbContext db, 
            IConfiguration configuration, 
            UserManager<UsuarioAplicacion> userManager,
            RoleManager<IdentityRole> roleManager,
            IMapper mapper)
        {
            _db = db;
            secretKey = configuration.GetValue<string>("ApiSettings:Secret");
            _userManager = userManager;
            _mapper = mapper;
            _roleManager = roleManager;
        }
        public bool IsUsuarioUnico(string userName)
        {
            var usuario = _db.UsuarioAplicacion.FirstOrDefault(u => u.UserName.ToLower() == userName.ToLower());
            if(usuario == null)
            {
                return true;
            }

            return false;
        }

        public async Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO)
        {
            var usuario = await _db.UsuarioAplicacion.FirstOrDefaultAsync(
                u => u.UserName.ToLower() == loginRequestDTO.UserName.ToLower());

            bool isValido = await _userManager.CheckPasswordAsync(usuario, loginRequestDTO.Password);

            if(usuario == null || isValido == false)
            {
                return new LoginResponseDTO()
                {
                    Token = "",
                    Usuario = null
                };
            }
            // Si usuario existe Generamos el JW Token
            var roles = await _userManager.GetRolesAsync(usuario);
            var tokenHandler = new JwtSecurityTokenHandler();
            //Vamos a codificar nuestra key 
            var key = Encoding.ASCII.GetBytes(secretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, usuario.UserName),
                    new Claim(ClaimTypes.Role, roles.FirstOrDefault())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            LoginResponseDTO loginResponseDTO = new()
            {
                Token = tokenHandler.WriteToken(token),
                Usuario = _mapper.Map<UsuarioDto>(usuario),
            };
            return loginResponseDTO;

        }

        public async Task<UsuarioDto> Registrar(RegistroRequestDTO registroRequestDTO)
        {
            UsuarioAplicacion usuario = new UsuarioAplicacion()
            {
                UserName = registroRequestDTO.UserName,
                Email = registroRequestDTO.UserName,
                NormalizedEmail = registroRequestDTO.UserName.ToUpper(),
                Nombres = registroRequestDTO.Nombres,
            };

            try
            {
                var resultado = await _userManager.CreateAsync(usuario, registroRequestDTO.Password);
                if(resultado.Succeeded)
                {
                    if (!_roleManager.RoleExistsAsync("admin").GetAwaiter().GetResult())
                    {
                        await _roleManager.CreateAsync(new IdentityRole("admin"));
                        await _roleManager.CreateAsync(new IdentityRole("cliente"));
                    }
                    await _userManager.AddToRoleAsync(usuario, "admin");
                    var usuarioApp = _db.UsuarioAplicacion.FirstOrDefault(u=>u.UserName == registroRequestDTO.UserName);
                    return _mapper.Map<UsuarioDto>(usuarioApp);
                }
            }
            catch (Exception ex)
            {

                throw;
            }

            return new UsuarioDto();
        }
    }
}
