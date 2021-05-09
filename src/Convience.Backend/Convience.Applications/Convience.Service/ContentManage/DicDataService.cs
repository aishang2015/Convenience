using AutoMapper;

using Convience.Entity.Data;
using Convience.Entity.Entity;
using Convience.EntityFrameWork.Repositories;
using Convience.Model.Models;
using Convience.Model.Models.ContentManage;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Convience.Service.ContentManage
{
    public interface IDicDataService
    {
        Task<DicDataResultModel> GetByIdAsync(int id);

        /// <summary>
        /// 根据字典类型编码取得对应字典数据
        /// </summary>
        /// <param name="dicTypeCode">字典类型编码</param>
        /// <returns>字典数据</returns>
        IEnumerable<DicResultModel> GetDicDataDic(string dicTypeCode);

        IEnumerable<DicDataResultModel> GetByDicTypeIdAsync(int dicTypeId);

        Task<bool> AddDicDataAsync(DicDataViewModel model);

        Task<bool> UpdateDicDataAsync(DicDataViewModel model);

        Task<bool> DeleteDicDataAsync(int id);
    }

    public class DicDataService : IDicDataService
    {
        private readonly ILogger<DicDataService> _logger;

        private readonly IRepository<DicType> _dictypeRepository;

        private readonly IRepository<DicData> _dicdataRepository;

        private readonly IUnitOfWork<SystemIdentityDbContext> _unitOfWork;

        private readonly IMapper _mapper;

        public DicDataService(
            ILogger<DicDataService> logger,
            IRepository<DicType> dictypeRepository,
            IRepository<DicData> dicdataRepository,
            IUnitOfWork<SystemIdentityDbContext> unitOfWork,
            IMapper mapper)
        {
            _logger = logger;
            _dictypeRepository = dictypeRepository;
            _dicdataRepository = dicdataRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<DicDataResultModel> GetByIdAsync(int id)
        {
            var dicdata = await _dicdataRepository.GetAsync(id);
            return _mapper.Map<DicDataResultModel>(dicdata);
        }

        public IEnumerable<DicDataResultModel> GetByDicTypeIdAsync(int dicTypeId)
        {
            var result = _dicdataRepository.Get(dicData => dicData.DicTypeId == dicTypeId)
                .OrderBy(dicdata => dicdata.Sort);
            return _mapper.Map<IQueryable<DicData>, IEnumerable<DicDataResultModel>>(result);
        }

        public IEnumerable<DicResultModel> GetDicDataDic(string dicTypeCode)
        {
            var dictype = _dictypeRepository.Get().Where(dicType => dicType.Code == dicTypeCode)
                .Include(dicType => dicType.DicDatas).FirstOrDefault();
            return dictype.DicDatas.Select(dicData => new DicResultModel
            {
                Key = dicData.Id.ToString(),
                Value = dicData.Name
            });
        }

        public async Task<bool> AddDicDataAsync(DicDataViewModel model)
        {
            var dicdata = _mapper.Map<DicData>(model);
            await _dicdataRepository.AddAsync(dicdata);
            await _unitOfWork.SaveAsync();
            return true;
        }

        public async Task<bool> DeleteDicDataAsync(int id)
        {
            await _dicdataRepository.RemoveAsync(id);
            await _unitOfWork.SaveAsync();
            return true;
        }

        public async Task<bool> UpdateDicDataAsync(DicDataViewModel model)
        {
            var entity = _mapper.Map<DicData>(model);
            _dicdataRepository.Update(entity);
            await _unitOfWork.SaveAsync();
            return true;
        }
    }
}