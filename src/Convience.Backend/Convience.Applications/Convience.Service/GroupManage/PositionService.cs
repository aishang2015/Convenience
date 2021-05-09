using AutoMapper;

using Convience.Entity.Data;
using Convience.Entity.Entity;
using Convience.Entity.Entity.Identity;
using Convience.EntityFrameWork.Repositories;
using Convience.JwtAuthentication;
using Convience.Model.Models;
using Convience.Model.Models.GroupManage;

using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Convience.Service.GroupManage
{
    public interface IPositionService
    {
        int Count();

        IEnumerable<PositionResultModel> GetAllPosition();

        IEnumerable<PositionResultModel> GetPositions(PositionQueryModel query);

        Task<PositionResultModel> GetPositionAsync(int id);

        Task<bool> AddPositionAsync(PositionViewModel model);

        Task<bool> UpdatePositionAsync(PositionViewModel model);

        Task<bool> DeletePositionAsync(int id);

        public IEnumerable<DicResultModel> GetPositionDic(string name);
    }

    public class PositionService : IPositionService
    {
        private readonly ILogger<PositionService> _logger;

        private readonly IRepository<Position> _positionRepository;

        private readonly IRepository<SystemUserClaim> _userClaimRepository;

        private readonly IUnitOfWork<SystemIdentityDbContext> _unitOfWork;

        private readonly IMapper _mapper;

        public PositionService(
            ILogger<PositionService> logger,
            IRepository<Position> positionRepository,
            IRepository<SystemUserClaim> userClaimRepository,
            IUnitOfWork<SystemIdentityDbContext> unitOfWork,
            IMapper mapper)
        {
            _logger = logger;
            _positionRepository = positionRepository;
            _userClaimRepository = userClaimRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<bool> AddPositionAsync(PositionViewModel model)
        {
            var entity = _mapper.Map<Position>(model);
            await _positionRepository.AddAsync(entity);
            await _unitOfWork.SaveAsync();
            return true;
        }

        public int Count()
        {
            return _positionRepository.Get().Count();
        }

        public async Task<bool> DeletePositionAsync(int id)
        {
            using var tran = await _unitOfWork.StartTransactionAsync();

            await _userClaimRepository.RemoveAsync(uc => uc.ClaimType == CustomClaimTypes.UserPosition &&
                uc.ClaimValue == id.ToString());

            await _positionRepository.RemoveAsync(id);
            await _unitOfWork.SaveAsync();
            await _unitOfWork.CommitAsync(tran);
            return true;
        }

        public IEnumerable<PositionResultModel> GetAllPosition()
        {
            var positions = _positionRepository.Get().OrderBy(p => p.Sort).ToArray();
            return _mapper.Map<Position[], IEnumerable<PositionResultModel>>(positions);
        }

        public async Task<PositionResultModel> GetPositionAsync(int id)
        {
            var entity = await _positionRepository.GetAsync(id);
            return _mapper.Map<PositionResultModel>(entity);
        }

        public IEnumerable<DicResultModel> GetPositionDic(string name)
        {
            var dic = from position in _positionRepository.Get()
                      where position.Name.Contains(name)
                      select new DicResultModel
                      {
                          Key = position.Id.ToString(),
                          Value = position.Name,
                      };
            return dic.Take(10);
        }

        public IEnumerable<PositionResultModel> GetPositions(PositionQueryModel query)
        {
            var positions = _positionRepository.Get(p => true, p => p.Sort,
                query.Page, query.Size).ToArray();
            return _mapper.Map<Position[], IEnumerable<PositionResultModel>>(positions);
        }

        public async Task<bool> UpdatePositionAsync(PositionViewModel model)
        {
            var entity = _mapper.Map<Position>(model);
            _positionRepository.Update(entity);
            await _unitOfWork.SaveAsync();
            return true;
        }
    }
}
