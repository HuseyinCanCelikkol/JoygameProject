using AutoMapper;
using JoygameProject.Application.DTOs;
using JoygameProject.Domain.Entities;

namespace JoygameProject.Application.Mappings
{
    public class ProductsProfile : Profile
    {
        public ProductsProfile()
        {
            CreateMap<Product, ProductDto>()
                .ReverseMap();
        }
    }
}
