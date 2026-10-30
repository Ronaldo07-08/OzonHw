using Grpc.Core.Interceptors;

namespace Presentation.Interceptors
{
    public class LoggingInterceptor : Interceptor
    {
        private readonly ILogger _logger;

        public LoggingInterceptor(ILogger logger) 
        {
            _logger = logger;
        }

        //public override async Task<> UnaryServerHandler()
        //{

        //}

    }
}
