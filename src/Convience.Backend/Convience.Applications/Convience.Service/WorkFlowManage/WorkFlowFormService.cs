using AutoMapper;

using Convience.Entity.Data;
using Convience.Entity.Entity.WorkFlows;
using Convience.EntityFrameWork.Repositories;
using Convience.JwtAuthentication;
using Convience.Model.Models;
using Convience.Model.Models.WorkFlowManage;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Convience.Service.WorkFlowManage
{
    public interface IWorkFlowFormService
    {
        WorkFlowFormResultModel GetWorkFlowForm(int workflowId);

        Task<bool> AddOrUpdateWorkFlowForm(WorkFlowFormViewModel viewModel);

        IEnumerable<DicResultModel> GetWorkFlowFormControlDic(int WorkFlowId);
    }

    public class WorkFlowFormService : IWorkFlowFormService
    {
        private readonly ILogger<WorkFlowFormService> _logger;

        private readonly IRepository<WorkFlow> _workflowRepository;

        private readonly IRepository<WorkFlowForm> _formRepository;

        private readonly IRepository<WorkFlowFormControl> _formControlRepository;

        private readonly IUnitOfWork<SystemIdentityDbContext> _unitOfWork;

        private readonly IHttpContextAccessor _httpContextAccessor;

        private IMapper _mapper;

        public WorkFlowFormService(
            ILogger<WorkFlowFormService> logger,
            IRepository<WorkFlow> workflowRepository,
            IRepository<WorkFlowForm> formRepository,
            IRepository<WorkFlowFormControl> formControlRepository,
            IUnitOfWork<SystemIdentityDbContext> unitOfWork,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper)
        {
            _logger = logger;
            _workflowRepository = workflowRepository;
            _formRepository = formRepository;
            _formControlRepository = formControlRepository;
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        public WorkFlowFormResultModel GetWorkFlowForm(int workflowId)
        {
            var form = _formRepository.Get(f => f.WorkFlowId == workflowId).FirstOrDefault();
            var formControls = _formControlRepository.Get(f => f.WorkFlowId == workflowId).ToArray();
            return new WorkFlowFormResultModel
            {
                FormResult = _mapper.Map<FormResultModel>(form),
                FormControlResults = _mapper.Map<IEnumerable<FormControlResultModel>>(formControls)
            };
        }

        public async Task<bool> AddOrUpdateWorkFlowForm(WorkFlowFormViewModel viewModel)
        {
            try
            {
                var userName = _httpContextAccessor.HttpContext?.User?.GetUserName();

                viewModel.FormViewModel.WorkFlowId = viewModel.WorkFlowId;
                foreach (var control in viewModel.FormControlViewModels)
                {
                    control.WorkFlowId = viewModel.WorkFlowId;
                }

                var workflow = await _workflowRepository.GetAsync(viewModel.WorkFlowId);
                workflow.UpdatedTime = DateTime.Now;
                workflow.UpdatedUser = userName;

                await _formRepository.RemoveAsync(f => f.WorkFlowId == viewModel.WorkFlowId);
                await _formRepository.AddAsync(_mapper.Map<WorkFlowForm>(viewModel.FormViewModel));
                await _formControlRepository.RemoveAsync(f => f.WorkFlowId == viewModel.WorkFlowId);
                await _formControlRepository.AddAsync(_mapper.Map<List<WorkFlowFormControl>>(viewModel.FormControlViewModels));
                await _unitOfWork.SaveAsync();
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(e.StackTrace);
                return false;
            }
        }

        public IEnumerable<DicResultModel> GetWorkFlowFormControlDic(int WorkFlowId)
        {
            return _formControlRepository.Get().Where(control => control.WorkFlowId == WorkFlowId)
                .Select(control => new DicResultModel
                {
                    Key = control.Id.ToString(),
                    Value = control.Name,
                });
        }
    }
}
