using Convience.Entity.Data;
using Convience.Entity.Entity.Logs;
using Convience.EntityFrameWork.Repositories;
using Convience.Model.Models;
using Convience.Model.Models.SystemTool;
using Convience.Util.Extension;

using Microsoft.Extensions.Logging;

using System.Linq;
using System.Threading.Tasks;

namespace Convience.Service.SystemTool
{
    public interface ILoginLogService
    {
        /// <summary>
        /// 获取操作日志配置
        /// </summary>
        public LoginLogSettingResultModel GetSetting();

        /// <summary>
        /// 更新操作日志配置
        /// </summary>
        public Task<bool> UpdateLoginLogSettingAsync(LoginLogSettingViewModel viewModel);

        /// <summary>
        /// 获取操作日志内容
        /// </summary>
        public PagingResultModel<LoginLogDetailResultModel> GetLoginLogDetail(LoginLogQueryModel queryModel);
    }

    public class LoginLogService : ILoginLogService
    {
        private readonly IRepository<LoginLogSetting> _logSettingRepository;

        private readonly IRepository<LoginLogDetail> _logDetailRepository;

        private readonly SystemIdentityDbUnitOfWork _unitOfWork;

        private readonly ILogger _logger;

        public LoginLogService(IRepository<LoginLogSetting> logSettingRepository,
            IRepository<LoginLogDetail> logDetailRepository,
            SystemIdentityDbUnitOfWork unitOfWork,
            ILogger<LoginLogService> logger)
        {
            _logSettingRepository = logSettingRepository;
            _logDetailRepository = logDetailRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public PagingResultModel<LoginLogDetailResultModel> GetLoginLogDetail(LoginLogQueryModel queryModel)
        {
            var logs = from detail in _logDetailRepository.Get(false)
                        .AndIfHaveValue(queryModel.Account, l => l.OperatorAccount.Contains(queryModel.Account))
                       orderby detail.OperateAt descending
                       select new LoginLogDetailResultModel
                       {
                           OperatorAccount = detail.OperatorAccount,
                           OperateAt = detail.OperateAt,
                           IpAddress = detail.IpAddress,
                           IsSuccess = detail.IsSuccess
                       };

            return new PagingResultModel<LoginLogDetailResultModel>
            {
                Count = logs.Count(),
                Data = logs.Skip((queryModel.Page - 1) * queryModel.Size).Take(queryModel.Size).ToList()
            };

        }

        public LoginLogSettingResultModel GetSetting()
        {
            var setting = _logSettingRepository.Get(false).FirstOrDefault();
            return new LoginLogSettingResultModel
            {
                SaveTime = setting.SaveTime
            };
        }

        public async Task<bool> UpdateLoginLogSettingAsync(LoginLogSettingViewModel viewModel)
        {
            var setting = _logSettingRepository.Get(true).FirstOrDefault();
            setting.SaveTime = viewModel.SaveTime;
            await _unitOfWork.SaveAsync();
            return true;
        }
    }
}
