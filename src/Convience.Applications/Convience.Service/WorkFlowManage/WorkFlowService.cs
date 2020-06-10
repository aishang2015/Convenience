using AutoMapper;
using Convience.Entity.Data;
using Convience.Entity.Entity.WorkFlows;
using Convience.EntityFrameWork.Infrastructure;
using Convience.EntityFrameWork.Repositories;
using Convience.Model.Models.WorkFlowManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Convience.Service.WorkFlowManage
{
    public interface IWorkFlowService
    {
        Task<WorkFlowResult> GetByIdAsync(int id);

        (IEnumerable<WorkFlowResult>, int) GetWorkFlows(WorkFlowQuery query);

        Task<bool> AddWorkFlowAsync(WorkFlowViewModel model, string userName);

        Task<bool> UpdateWorkFlowAsync(WorkFlowViewModel model, string userName);

        Task<bool> DeleteWorkFlowAsync(int id);
    }


    public class WorkFlowService : IWorkFlowService
    {
        private readonly IRepository<WorkFlow> _workFlowRepository;

        private readonly IUnitOfWork<SystemIdentityDbContext> _unitOfWork;

        private IMapper _mapper;

        public WorkFlowService(IRepository<WorkFlow> workFlowRepository,
            IUnitOfWork<SystemIdentityDbContext> unitOfWork, IMapper mapper)
        {
            _workFlowRepository = workFlowRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<bool> AddWorkFlowAsync(WorkFlowViewModel model, string userName)
        {
            try
            {
                var workflow = _mapper.Map<WorkFlow>(model);
                workflow.CreatedTime = DateTime.Now;
                workflow.UpdatedTime = DateTime.Now;
                workflow.CreatedUser = userName;
                workflow.UpdatedUser = userName;

                await _workFlowRepository.AddAsync(workflow);
                await _unitOfWork.SaveAsync();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteWorkFlowAsync(int id)
        {
            try
            {
                await _workFlowRepository.RemoveAsync(id);
                await _unitOfWork.SaveAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<WorkFlowResult> GetByIdAsync(int id)
        {
            var result = await _workFlowRepository.GetAsync(id);
            return _mapper.Map<WorkFlowResult>(result);
        }

        public (IEnumerable<WorkFlowResult>, int) GetWorkFlows(WorkFlowQuery query)
        {
            var workFlowQuery = _workFlowRepository.Get(wf => wf.WorkFlowGroupId == query.WorkFlowGroupId)
                .Skip((query.Page - 1) * query.Size).Take(query.Size).ToArray();

            return (_mapper.Map<WorkFlow[], IEnumerable<WorkFlowResult>>(workFlowQuery), workFlowQuery.Count());

        }

        public async Task<bool> UpdateWorkFlowAsync(WorkFlowViewModel model, string userName)
        {
            try
            {
                var wf = _workFlowRepository.Get(wf => wf.Id == model.Id).FirstOrDefault();
                _mapper.Map(model, wf);
                wf.UpdatedTime = DateTime.Now;
                wf.UpdatedUser = userName;
                _workFlowRepository.Update(wf);
                await _unitOfWork.SaveAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
