using AutofacTutorial.ViewModels;
using AutoMapper;
using ClientsDb.Entities;

namespace AutofacTutorial.Mapper
{
    public class ClientAppMappingProfile : Profile
    {
        public ClientAppMappingProfile()
        {
            CreateMap<Client, ClientViewModel>()
                .ReverseMap();
                //.ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Mail));
        }
    }
}
