using AutoMapper;
using Hotel.BLogicLayer.DTO;
using Hotel.BLogicLayer.Interfaces;
using Hotel.DAL.Models;
using Hotel.PRLAYER.Models;

namespace Hotel.BLogicLayer
{
    public class ContainedMapper : IMapperItem
    {
        //Realize property than represents mapper for whole project to map dto-s and models
        public IMapper Mapper { get; } = new MapperConfiguration(cfg =>
        {
            //Maps for stays
            cfg.CreateMap<StayDto, Stay>();
            cfg.CreateMap<Stay, StayDto>();
            cfg.CreateMap<StayDto, StayModel>();
            cfg.CreateMap<StayModel, StayDto>();

            //Maps for Rooms
            cfg.CreateMap<RoomDto, Room>();
            cfg.CreateMap<Room, RoomDto>();
            cfg.CreateMap<RoomDto, RoomModel>();
            cfg.CreateMap<RoomModel, RoomDto>();
            

            //Maps for Categories
            cfg.CreateMap<CategoryDate, CategoryDateDto>();
            cfg.CreateMap<CategoryDateDto, CategoryDate>();
            cfg.CreateMap<CategoryDateDto, CategoryDateModel>();
            cfg.CreateMap<CategoryDateModel, CategoryDateDto>();
            
            //Maps for guests
            cfg.CreateMap<Guest, GuestDto>();
            cfg.CreateMap<GuestDto, Guest>();
            cfg.CreateMap<GuestDto, GuestModel>();
            cfg.CreateMap<GuestModel, GuestDto>();

            
            //Maps for Categories
            cfg.CreateMap<CategoryDto, Category>();
            cfg.CreateMap<Category, CategoryDto>();
            cfg.CreateMap<CategoryDto, CategoryModel>();
            cfg.CreateMap<CategoryModel, CategoryDto>();

            //Maps for Roles
            cfg.CreateMap<Role, RoleDto>();
            cfg.CreateMap<RoleDto, Role>();
            cfg.CreateMap<RoleDto, RoleModel>();
            cfg.CreateMap<RoleModel, RoleDto>();
            
            
            //Maps for GuestRegisterInfos
            cfg.CreateMap<GuestRegisterInfo, GuestRegisterInfoDto>();
            cfg.CreateMap<GuestRegisterInfoDto, GuestRegisterInfo>();
            cfg.CreateMap<GuestRegisterInfoDto, GuestRegisterInfoModel>();
            cfg.CreateMap<GuestRegisterInfoModel, GuestRegisterInfoDto>();


            cfg.CreateMap<ProfitReportDto, ProfitReportModel>();
            cfg.CreateMap<ProfitReportModel, ProfitReportDto>();
        }).CreateMapper();
    }
}