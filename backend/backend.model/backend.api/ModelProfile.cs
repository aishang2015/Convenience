using AutoMapper;
using Backend.Entity.backend.api.Data;
using Backend.Entity.backend.api.Entity;
using Backend.Model.backend.api.Models.SaasManage;
using Backend.Model.backend.api.Models.SystemManage;

namespace backend.model.backend.api
{
    public class ModelProfile : Profile
    {
        public ModelProfile()
        {
            CreateMap<RoleViewModel, SystemRole>();
            CreateMap<SystemRole, RoleResult>();

            CreateMap<UserViewModel, SystemUser>();
            CreateMap<SystemUser, UserResult>().ForMember(user => user.Sex,
                ex => ex.MapFrom(result => (int)result.Sex));

            CreateMap<MenuViewModel, Menu>();
            CreateMap<Menu, MenuResult>().ForMember(menu => menu.Type,
                ex => ex.MapFrom(result => (int)result.Type));

            CreateMap<Tenant, TenantResult>();
            CreateMap<TenantViewModel, Tenant>();
        }
    }
}
