using AutoMapper;
using CSBI.Test.API.Dto;
using CSBI.Test.Domain.Models;

namespace CSBI.Test.API.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            this.CreateMap<Client, ClientDto>().ReverseMap();
        }
    }
}
