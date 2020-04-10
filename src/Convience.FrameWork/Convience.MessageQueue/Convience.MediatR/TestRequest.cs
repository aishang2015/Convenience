using MediatR;

namespace Convience.MediatRs
{
    public class TestRequest : IRequest<string>
    {
        public TestRequest(string message)
        {
            Message = message;
        }

        public string Message { get; set; }
    }
}
