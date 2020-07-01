using System.Net.WebSockets;
using System.Threading.Tasks;

namespace Convience.WebSockets
{
    public interface IWebSocketDataHandler
    {
        Task SendAllAsync(string data);

        Task HandleDataAsync(WebSocket webSocket, byte[] bytes);
    }
}
