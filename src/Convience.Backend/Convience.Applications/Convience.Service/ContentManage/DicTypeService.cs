using AutoMapper;

using Convience.Entity.Data;
using Convience.Entity.Entity;
using Convience.EntityFrameWork.Repositories;
using Convience.Model.Models.ContentManage;

using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Convience.Service.ContentManage
{
    public interface IDicTypeService
    {
        Task<DicTypeResultModel> GetByIdAsync(int id);

        IEnumerable<DicTypeResultModel> GetDicTypes();

        bool HasSameCode(string code);

        bool HasSameCode(int id, string code);

        Task<bool> AddDicTypeAsync(DicTypeViewModel model);

        Task<bool> UpdateDicTypeAsync(DicTypeViewModel model);

        Task<bool> DeleteDicTypeAsync(int id);
    }

    public class DicTypeService : IDicTypeService
    {
        private readonly ILogger<DicTypeService> _logger;

        private readonly IRepository<DicType> _dictypeRepository;

        private readonly IUnitOfWork<SystemIdentityDbContext> _unitOfWork;

        private readonly IMapper _mapper;

        public DicTypeService(
            ILogger<DicTypeService> logger,
            IRepository<DicType> dictypeRepository,
            IUnitOfWork<SystemIdentityDbContext> unitOfWork,
            IMapper mapper)
        {
            _logger = logger;
            _dictypeRepository = dictypeRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<DicTypeResultModel> GetByIdAsync(int id)
        {
            var dictype = await _dictypeRepository.GetAsync(id);
            return _mapper.Map<DicTypeResultModel>(dictype);
        }

        public async Task<bool> AddDicTypeAsync(DicTypeViewModel model)
        {
            var dictype = _mapper.Map<DicType>(model);
            await _dictypeRepository.AddAsync(dictype);
            await _unitOfWork.SaveAsync();
            return true;
        }

        public async Task<bool> DeleteDicTypeAsync(int id)
        {
            await _dictypeRepository.RemoveAsync(id);
            await _unitOfWork.SaveAsync();
            return true;
        }

        public async Task<bool> UpdateDicTypeAsync(DicTypeViewModel model)
        {
            var entity = await _dictypeRepository.GetAsync(model.Id);
            _mapper.Map(model, entity);
            await _unitOfWork.SaveAsync();
            return true;
        }

        public IEnumerable<DicTypeResultModel> GetDicTypes()
        {
            var dicTypeQuery = _dictypeRepository.Get().OrderBy(dicType => dicType.Sort);
            return _mapper.Map<IQueryable<DicType>, IEnumerable<DicTypeResultModel>>(dicTypeQuery);
        }

        public bool HasSameCode(string code)
        {
            return _dictypeRepository.Get().Any(dt => dt.Code.ToLower() == code.ToLower());
        }

        public bool HasSameCode(int id, string code)
        {
            return _dictypeRepository.Get()
                .Any(dt => dt.Code.ToLower() == code.ToLower() && dt.Id != id);
        }
    }
}