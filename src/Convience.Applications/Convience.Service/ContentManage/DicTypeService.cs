using AutoMapper;

using Convience.Entity.Data;
using Convience.Entity.Entity;
using Convience.EntityFrameWork.Repositories;
using Convience.Model.Models.ContentManage;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Convience.Service.ContentManage
{
    public class DicTypeService : IDicTypeService
    {
        private readonly IRepository<DicType> _dictypeRepository;

        private readonly IUnitOfWork<SystemIdentityDbContext> _unitOfWork;

        private readonly IMapper _mapper;

        public DicTypeService(IRepository<DicType> dictypeRepository,
          IUnitOfWork<SystemIdentityDbContext> unitOfWork,
          IMapper mapper)
        {
            _dictypeRepository = dictypeRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<DicTypeResult> GetByIdAsync(int id)
        {
            var dictype = await _dictypeRepository.GetAsync(id);
            return _mapper.Map<DicTypeResult>(dictype);
        }

        public async Task<bool> AddDicTypeAsync(DicTypeViewModel model)
        {
            try
            {
                var dictype = _mapper.Map<DicType>(model);
                await _dictypeRepository.AddAsync(dictype);
                await _unitOfWork.SaveAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteDicTypeAsync(int id)
        {
            try
            {
                await _dictypeRepository.RemoveAsync(id);
                await _unitOfWork.SaveAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> UpdateDicTypeAsync(DicTypeViewModel model)
        {
            try
            {
                var entity = await _dictypeRepository.GetAsync(model.Id);
                _mapper.Map(model, entity);
                await _unitOfWork.SaveAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public IEnumerable<DicTypeResult> GetDicTypes()
        {
            var dicTypeQuery = _dictypeRepository.Get().OrderBy(dicType => dicType.Sort);
            return _mapper.Map<IQueryable<DicType>, IEnumerable<DicTypeResult>>(dicTypeQuery);
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