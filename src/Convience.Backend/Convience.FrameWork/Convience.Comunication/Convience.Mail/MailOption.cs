namespace Convience.Mail
{
    public class MailOption
    {
        // 主机地址
        public string Host { get; set; }

        // 端口号
        public int Port { get; set; }

        // 是否启用ssl
        public bool UseSSL { get; set; }

        // 邮箱
        public string Name { get; set; }

        // 用户名
        public string Username { get; set; }

        // 邮箱地址
        public string EmailAddress { get; set; }

        // 邮箱密码
        public string Password { get; set; }
    }
}
