using AutoMapper;

namespace BankProject.Models
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<ClientDisplayDto, Client>();
            CreateMap<ClientDto, Client>();
            CreateMap<AddressDto,Address>();
        }
    }
}
