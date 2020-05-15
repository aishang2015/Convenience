using AutoMapper;

using Convience.Entity.Data;
using Convience.Entity.Entity;
using Convience.EntityFrameWork.Repositories;
using Convience.Model.Models;
using Convience.Model.Models.ContentManage;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Convience.Service.ContentManage
{
    public class DicDataService : IDicDataService
    {
        private readonly IRepository<DicType> _dictypeRepository;

        private readonly IRepository<DicData> _dicdataRepository;

        private readonly IUnitOfWork<SystemIdentityDbContext> _unitOfWork;

        private readonly IMapper _mapper;

        public DicDataService(
            IRepository<DicType> _dictypeRepository,
            IRepository<DicData> dicdataRepository,
          IUnitOfWork<SystemIdentityDbContext> unitOfWork,
          IMapper mapper)
        {
            _dicdataRepository = dicdataRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<DicDataResult> GetByIdAsync(int id)
        {
            var dicdata = await _dicdataRepository.GetAsync(id);
            return _mapper.Map<DicDataResult>(dicdata);
        }

        public IEnumerable<DicDataResult> GetByDicTypeIdAsync(int dicTypeId)
        {
            var result = _dicdataRepository.Get(dicData => dicData.DicTypeId == dicTypeId)
                .OrderBy(dicdata => dicdata.Sort);
            return _mapper.Map<IQueryable<DicData>, IEnumerable<DicDataResult>>(result);
        }

        public IEnumerable<DicModel> GetDicDataDic(string dicTypeCode)
        {
            var dictype = _dictypeRepository.Get().Where(dicType => dicType.Code == dicTypeCode)
                .Include(dicType => dicType.DicDatas).FirstOrDefault();
            return dictype.DicDatas.Select(dicData => new DicModel
            {
                Key = dicData.Id.ToString(),
                Value = dicData.Name
            });
        }

        public async Task<bool> AddDicDataAsync(DicDataViewModel model)
        {
            try
            {
                var dicdata = _mapper.Map<DicData>(model);
                await _dicdataRepository.AddAsync(dicdata);
                await _unitOfWork.SaveAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteDicDataAsync(int id)
        {
            try
            {
                await _dicdataRepository.RemoveAsync(id);
                await _unitOfWork.SaveAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> UpdateDicDataAsync(DicDataViewModel model)
        {
            try
            {
                var entity = _mapper.Map<DicData>(model);
                _dicdataRepository.Update(entity);
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