using AutoMapper;
using backend.entity.backend.api;
using backend.model.backend.api.AccountViewModels;
using Backend.Model.backend.api.SystemManage;

namespace backend.model.backend.api
{
    public class ModelProfile : Profile
    {
        public ModelProfile()
        {
            
            CreateMap<RoleViewModel, SystemRole>();
            CreateMap<SystemRole, RoleResult>();
        }
    }
}
