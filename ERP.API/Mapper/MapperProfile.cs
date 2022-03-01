using AutoMapper;

using ERP.Domain.Dtos;
using ERP.Domain.Entity;

namespace ERP.API.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<ContactDto, Contact>().ReverseMap();

            CreateMap<ContactIdDto, Contact>().ReverseMap(); 

        }
    }
}
