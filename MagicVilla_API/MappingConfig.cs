using AutoMapper;
using MagicVilla_API.Modelos;
using MagicVilla_API.Modelos.Dtos;

namespace MagicVilla_API
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<Villa, VillaDto>().ReverseMap();

            CreateMap<Villa, VillaCreateDto>().ReverseMap();
            CreateMap<Villa, VillaUpdateDto>().ReverseMap();
        }
    }
}
