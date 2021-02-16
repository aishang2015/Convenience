using AppService.Service;

using AutoMapper;

using Convience.Entity.Data;
using Convience.Entity.Entity.Logs;
using Convience.EntityFrameWork.Repositories;
using Convience.Model.Models;
using Convience.Model.Models.SystemTool;
using Convience.Util.Extension;

using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Convience.Service.SystemTool
{
    public interface IOperateLogService : IBaseService
    {
        /// <summary>
        /// 获取操作日志配置
        /// </summary>
        public PagingResultModel<OperateLogSettingResultModel> GetPagingOperateLogSetting(OperateLogSettingQueryModel queryModel);

        /// <summary>
        /// 更新操作日志配置
        /// </summary>
        public Task<bool> UpdateOpreateLogSettingAsync(OperateLogSettingViewModel viewModel);

        /// <summary>
        /// 获取操作日志内容
        /// </summary>
        public PagingResultModel<OperateLogDetailResultModel> GetPagingOperateDetail(OperateLogDetailQueryModel queryModel);
    }

    public class OperateLogService : BaseService, IOperateLogService
    {
        private readonly IRepository<OperateLogSetting> _logSettingRepository;

        private readonly IRepository<OperateLogDetail> _logDetailRepository;

        private readonly ICachingService<OperateLogSetting> _logSettingCache;

        private readonly SystemIdentityDbUnitOfWork _unitOfWork;

        private readonly IMapper _mapper;

        private readonly ILogger _logger;

        public OperateLogService(
            IRepository<OperateLogSetting> logSettingRepository,
            IRepository<OperateLogDetail> logDetailRepository,
            ICachingService<OperateLogSetting> logSettingCache,
            SystemIdentityDbUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<OperateLogService> logger)
        {
            _logSettingRepository = logSettingRepository;
            _logDetailRepository = logDetailRepository;
            _logSettingCache = logSettingCache;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// 获取操作日志内容
        /// </summary>
        public PagingResultModel<OperateLogDetailResultModel> GetPagingOperateDetail(OperateLogDetailQueryModel queryModel)
        {
            // 查询
            var query = from detail in _logDetailRepository.Get(false)
                        join setting in _logSettingRepository.Get(false) on detail.SettingId equals setting.Id
                        select new OperateLogDetailResultModel
                        {
                            Id = detail.Id,
                            OperatorAccount = detail.OperatorAccount,
                            OperatorName = detail.OperatorName,
                            OperateAt = detail.OperateAt,
                            ResultCode = detail.ResultCode,
                            Uri = detail.Uri,
                            UpdateField = detail.UpdateField,
                            OldValue = detail.OldValue,
                            NewValue = detail.NewValue,
                            Content = detail.Content,

                            ModuleName = setting.ModuleName,
                            SubModuleName = setting.SubModuleName,
                            Function = setting.Function,
                            Method = setting.Method,
                            SaveTime = setting.SaveTime
                        };

            query = query
                .AndIfHaveValue(queryModel.Module, s => s.ModuleName.Contains(queryModel.Module))
                .AndIfHaveValue(queryModel.Submodule, s => s.SubModuleName.Contains(queryModel.Submodule))
                .AndIfHaveValue(queryModel.Operator, s => s.OperatorName.Contains(queryModel.Operator))
                .AndIfHaveValue(queryModel.StartAt, s => s.OperateAt > queryModel.StartAt)
                .AndIfHaveValue(queryModel.EndAt, s => s.OperateAt < queryModel.EndAt);

            // 排序
            var sortFieldDic = new Dictionary<string, Expression<Func<OperateLogDetailResultModel, object>>>();
            sortFieldDic["module"] = t => t.ModuleName;
            sortFieldDic["subModule"] = t => t.SubModuleName;
            sortFieldDic["operatorName"] = t => t.OperatorAccount;
            sortFieldDic["operateAt"] = t => t.OperateAt;
            sortFieldDic["id"] = t => t.Id;
            queryModel.Sort = JoinString(queryModel.Sort, "id");
            queryModel.Order = JoinString(queryModel.Order, "ascend");
            query = GetOrderQuery(query, queryModel.Sort, queryModel.Order, sortFieldDic);

            // 分页
            var skip = queryModel.Size * (queryModel.Page - 1);
            var result = query.Skip(skip).Take(queryModel.Size).ToList();
            return new PagingResultModel<OperateLogDetailResultModel>
            {
                Count = query.Count(),
                Data = result
            };
        }

        /// <summary>
        /// 获取操作日志配置
        /// </summary>
        public PagingResultModel<OperateLogSettingResultModel> GetPagingOperateLogSetting(OperateLogSettingQueryModel queryModel)
        {
            // 查询
            var query = _logSettingRepository.Get(false)
                .AndIfHaveValue(queryModel.Module, s => s.ModuleName.Contains(queryModel.Module))
                .AndIfHaveValue(queryModel.Submodule, s => s.SubModuleName.Contains(queryModel.Submodule));

            // 排序
            var sortFieldDic = new Dictionary<string, Expression<Func<OperateLogSetting, object>>>();
            sortFieldDic["module"] = t => t.ModuleName;
            sortFieldDic["subModule"] = t => t.SubModuleName;
            sortFieldDic["id"] = t => t.Id;
            queryModel.Sort = JoinString(queryModel.Sort, "id");
            queryModel.Order = JoinString(queryModel.Order, "ascend");
            query = GetOrderQuery(query, queryModel.Sort, queryModel.Order, sortFieldDic);

            // 取得数据
            var skip = queryModel.Size * (queryModel.Page - 1);
            var result = query.Skip(skip).Take(queryModel.Size).ToList();
            return new PagingResultModel<OperateLogSettingResultModel>
            {
                Count = query.Count(),
                Data = _mapper.Map<List<OperateLogSettingResultModel>>(result)
            };
        }

        /// <summary>
        /// 更新操作日志配置
        /// </summary>
        public async Task<bool> UpdateOpreateLogSettingAsync(OperateLogSettingViewModel viewModel)
        {
            try
            {
                var setting = await _logSettingRepository.GetAsync(viewModel.Id);
                setting.SaveTime = viewModel.SaveTime;
                setting.IsRecord = viewModel.IsRecord;
                _logSettingRepository.Update(setting);
                await _unitOfWork.SaveAsync();

                // 清除setting缓存
                _logSettingCache.ClearCacheData();
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                _logger.LogError(e.StackTrace);
            }
            return false;
        }
    }
}
