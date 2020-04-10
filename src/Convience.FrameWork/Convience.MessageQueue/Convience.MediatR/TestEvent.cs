using MediatR;

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
