using System.Threading.Tasks;

namespace Convience.WebSockets
{
    public interface IWebSocketService
    {
        /// <summary>
        /// 返回当前服务需要处理的websocket路径
        /// </summary>
        public string GetPath();

        /// <summary>
        /// 取得需要发送的数据
        /// </summary>
        public Task<byte[]> GetSendData(object data);

        /// <summary>
        /// 处理接受的数据
        /// </summary>
        public Task<object> HandleReceivedData(object data);
    }
}
