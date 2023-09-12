using AutoMapper;
using MagicVilla_Web.Models.Dto;

namespace MagicVilla_Web
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<VillaDto, VillaCreateDto>().ReverseMap();
            CreateMap<VillaCreateDto, VillaUpdateDto>().ReverseMap();

            CreateMap<NumeroVillaDto, NumeroVillaCreateDto>().ReverseMap();
            CreateMap<NumeroVillaDto, NumeroVillaUpdateDto>().ReverseMap();
        }
    }
}
