using AutoMapper;
using Convience.Entity.Data;
using Convience.Entity.Entity.WorkFlows;
using Convience.EntityFrameWork.Repositories;
using Convience.Model.Models.WorkFlowManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Convience.Service.WorkFlowManage
{
    public interface IWorkFlowFormService
    {
        WorkFlowFormResult GetWorkFlowForm(int workflowId);

        Task<bool> AddOrUpdateWorkFlowForm(WorkFlowFormViewModel viewModel, string userName);
    }

    public class WorkFlowFormService : IWorkFlowFormService
    {
        private readonly IRepository<WorkFlow> _workflowRepository;

        private readonly IRepository<WorkFlowForm> _formRepository;

        private readonly IRepository<WorkFlowFormControl> _formControlRepository;

        private readonly IUnitOfWork<SystemIdentityDbContext> _unitOfWork;

        private IMapper _mapper;

        public WorkFlowFormService(
            IRepository<WorkFlow> workflowRepository,
            IRepository<WorkFlowForm> formRepository,
            IRepository<WorkFlowFormControl> formControlRepository,
            IUnitOfWork<SystemIdentityDbContext> unitOfWork,
            IMapper mapper)
        {
            _workflowRepository = workflowRepository;
            _formRepository = formRepository;
            _formControlRepository = formControlRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public WorkFlowFormResult GetWorkFlowForm(int workflowId)
        {
            var form = _formRepository.Get(f => f.WorkFlowId == workflowId).FirstOrDefault();
            var formControls = _formControlRepository.Get(f => f.WorkFlowId == workflowId).ToArray();
            return new WorkFlowFormResult
            {
                FormResult = _mapper.Map<FormResult>(form),
                FormControlResults = _mapper.Map<IEnumerable<FormControlResult>>(formControls)
            };
        }

        public async Task<bool> AddOrUpdateWorkFlowForm(WorkFlowFormViewModel viewModel, string userName)
        {
            try
            {
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
                return false;
            }
        }
    }
}
