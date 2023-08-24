using AutoMapper;
using BankProject.Models.Address;
using BankProject.Models.Client.Request;
using BankProject.Models.Client.Response;

namespace BankProject.Entities
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Client, ClientDisplayDto>().ReverseMap();
            CreateMap<ClientDto, Client>().ReverseMap();
            CreateMap<AddressDto, Address>().ReverseMap();
            CreateMap<Client, AuthenticateResponse>().ReverseMap();
            CreateMap<Admin, AuthenticateResponse>().ReverseMap();
        }
    }
}
