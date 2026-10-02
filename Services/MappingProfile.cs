using AutoMapper;
using WebProject.DTOs;
using WebProject.Models;

namespace WebProject.Services;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Product, ProductDto>()
            .ForCtorParam("ImagePath", opt => opt.MapFrom(p => p.Images.OrderByDescending(i => i.IsPrimary).Select(i => i.ImagePath).FirstOrDefault() ?? "/uploads/sample-product.svg"))
            .ForCtorParam("EffectivePrice", opt => opt.MapFrom(p => p.DiscountPrice ?? p.Price))
            .ForCtorParam("AverageRating", opt => opt.MapFrom(p => p.Reviews.Any() ? p.Reviews.Average(r => r.Rating) : 0));
    }
}
