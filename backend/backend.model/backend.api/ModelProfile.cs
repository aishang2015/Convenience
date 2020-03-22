using AutoMapper;

using Backend.Model.backend.api.Models.SystemManage;
using Backend.Repository.backend.api.Data;

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
        }
    }
}
