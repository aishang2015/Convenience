using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;

namespace Convience.WebSockets
{
    public class WebSocketConnectionManager
    {
        private static ConcurrentDictionary<Guid, WebSocket> _webSocketDic =
            new ConcurrentDictionary<Guid, WebSocket>();

        public void Add(WebSocket ws)
        {
            _webSocketDic.TryAdd(Guid.NewGuid(), ws);
        }

        public ConcurrentDictionary<Guid, WebSocket> All()
        {
            return _webSocketDic;
        }

        public void Remove(WebSocket ws)
        {
            var key = _webSocketDic.FirstOrDefault(pair => pair.Value == ws).Key;
            _webSocketDic.Remove(key, out WebSocket webSocket);
        }


    }
}
