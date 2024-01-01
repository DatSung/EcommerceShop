using AutoMapper;
using EcommerceShop.Data;
using EcommerceShop.ViewModels;

namespace EcommerceShop.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile() 
        {
            CreateMap<RegisterViewModel, KhachHang>();
                //.ForMember(kh => kh.HoTen, option => option
                //.MapFrom(RegisterViewModel => RegisterViewModel.HoTen))
                //.ReverseMap();
        }
    }
}
