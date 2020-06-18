using AutoMapper;

using Convience.Entity.Data;
using Convience.Entity.Entity;
using Convience.Entity.Entity.WorkFlows;
using Convience.Model.Models.ContentManage;
using Convience.Model.Models.GroupManage;
using Convience.Model.Models.SaasManage;
using Convience.Model.Models.SystemManage;
using Convience.Model.Models.WorkFlowManage;

namespace Convience.Model
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

            CreateMap<TenantViewModel, Tenant>();
            CreateMap<Tenant, TenantResult>();

            CreateMap<DepartmentViewModel, Department>();
            CreateMap<Department, DepartmentResult>();

            CreateMap<PositionViewModel, Position>();
            CreateMap<Position, PositionResult>();

            CreateMap<ColumnViewModel, Column>();
            CreateMap<Column, ColumnResult>();

            CreateMap<ArticleViewModel, Article>();
            CreateMap<Article, ArticleResult>();

            CreateMap<DicTypeViewModel, DicType>();
            CreateMap<DicType, DicTypeResult>();

            CreateMap<DicDataViewModel, DicData>();
            CreateMap<DicData, DicDataResult>();

            CreateMap<WorkFlowGroupViewModel, WorkFlowGroup>();
            CreateMap<WorkFlowGroup, WorkFlowGroupResult>();

            CreateMap<WorkFlowViewModel, WorkFlow>();
            CreateMap<WorkFlow, WorkFlowResult>();

            CreateMap<FormViewModel, WorkFlowForm>();
            CreateMap<WorkFlowForm, FormResult>();
            CreateMap<FormControlViewModel, WorkFlowFormControl>();
            CreateMap<WorkFlowFormControl, FormControlResult>();

            CreateMap<WorkFlowLinkViewModel, WorkFlowLink>();
            CreateMap<WorkFlowLink, WorkFlowLinkResult>();
            CreateMap<WorkFlowNodeViewModel, WorkFlowNode>();
            CreateMap<WorkFlowNode, WorkFlowNodeResult>();
            CreateMap<WorkFlowConditionViewModel, WorkFlowCondition>();
            CreateMap<WorkFlowCondition, WorkFlowConditionResult>();

            CreateMap<WorkFlowInstance, WorkFlowInstanceResult>();
        }
    }
}
