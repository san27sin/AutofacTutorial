using AutofacTutorial.ViewModels;
using AutoMapper;
using ClientsDb.Entities;

namespace AutofacTutorial.Mapper
{
    public class OrderAppMappingProfile : Profile
    {
        public OrderAppMappingProfile() 
        {
            CreateMap<Order, OrderViewModel>()
                .ReverseMap();
        }
    }
}
