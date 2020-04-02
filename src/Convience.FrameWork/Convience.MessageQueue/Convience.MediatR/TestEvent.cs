using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Convience.MediatRs
{
    public class TestEvent : INotification
    {
        public string Message { get; set; }

        public TestEvent(string message)
        {
            Message = message;
        }
    }
}
