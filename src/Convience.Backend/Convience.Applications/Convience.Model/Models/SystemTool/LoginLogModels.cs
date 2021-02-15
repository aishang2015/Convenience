using System;

namespace Convience.Model.Models.SystemTool
{
    public class LoginLogSettingViewModel
    {
        public int SaveTime { get; set; }
    }

    public class LoginLogSettingResultModel : LoginLogSettingViewModel { }

    public class LoginLogQueryModel : PageSortQueryModel
    {
        public string Account { get; set; }
    }

    public class LoginLogDetailResultModel
    {

        public string OperatorAccount { get; set; }

        public DateTime OperateAt { get; set; }

        public string IpAddress { get; set; }

        public bool IsSuccess { get; set; }
    }

}
