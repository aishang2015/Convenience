using AutoMapper;
using Convience.Entity.Entity;
using Convience.Entity.Entity.Identity;
using Convience.Entity.Entity.Logs;
using Convience.Entity.Entity.WorkFlows;
using Convience.Model.Models.ContentManage;
using Convience.Model.Models.GroupManage;
using Convience.Model.Models.SystemManage;
using Convience.Model.Models.SystemTool;
using Convience.Model.Models.WorkFlowManage;

namespace Convience.Model
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<RoleViewModel, SystemRole>();
            CreateMap<SystemRole, RoleResultModel>();

            CreateMap<UserViewModel, SystemUser>();
            CreateMap<SystemUser, UserResultModel>().ForMember(user => user.Sex,
                ex => ex.MapFrom(result => (int)result.Sex));

            CreateMap<MenuViewModel, Menu>();
            CreateMap<Menu, MenuResultModel>().ForMember(menu => menu.Type,
                ex => ex.MapFrom(result => (int)result.Type));

            CreateMap<DepartmentViewModel, Department>();
            CreateMap<Department, DepartmentResultModel>();

            CreateMap<PositionViewModel, Position>();
            CreateMap<Position, PositionResultModel>();

            CreateMap<ColumnViewModel, Column>();
            CreateMap<Column, ColumnResultModel>();

            CreateMap<ArticleViewModel, Article>();
            CreateMap<Article, ArticleResultModel>();

            CreateMap<DicTypeViewModel, DicType>();
            CreateMap<DicType, DicTypeResultModel>();

            CreateMap<DicDataViewModel, DicData>();
            CreateMap<DicData, DicDataResultModel>();

            CreateMap<WorkFlowGroupViewModel, WorkFlowGroup>();
            CreateMap<WorkFlowGroup, WorkFlowGroupResultModel>();

            CreateMap<WorkFlowViewModel, WorkFlow>();
            CreateMap<WorkFlow, WorkFlowResultModel>();

            CreateMap<FormViewModel, WorkFlowForm>();
            CreateMap<WorkFlowForm, FormResultModel>();
            CreateMap<FormControlViewModel, WorkFlowFormControl>();
            CreateMap<WorkFlowFormControl, FormControlResultModel>();

            CreateMap<WorkFlowLinkViewModel, WorkFlowLink>();
            CreateMap<WorkFlowLink, WorkFlowLinkResultModel>();
            CreateMap<WorkFlowNodeViewModel, WorkFlowNode>();
            CreateMap<WorkFlowNode, WorkFlowNodeResultModel>();
            CreateMap<WorkFlowConditionViewModel, WorkFlowCondition>();
            CreateMap<WorkFlowCondition, WorkFlowConditionResultModel>();

            CreateMap<WorkFlowInstance, WorkFlowInstanceResultModel>();
            CreateMap<WorkFlowInstanceValueViewModel, WorkFlowInstanceValue>();
            CreateMap<WorkFlowInstanceValue, WorkFlowInstanceValueResultModel>();
            CreateMap<WorkFlowInstanceRoute, WorkflowinstanceRouteResultModel>();

            CreateMap<OperateLogDetailResultModel, OperateLogDetail>().ReverseMap();
            CreateMap<OperateLogSettingResultModel, OperateLogSetting>().ReverseMap();
        }
    }
}
