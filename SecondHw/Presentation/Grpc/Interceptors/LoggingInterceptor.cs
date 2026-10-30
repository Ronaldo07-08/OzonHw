using Grpc.Core;
using Grpc.Core.Interceptors;

namespace Presentation.Grpc.Interceptors
{
    public class LoggingInterceptor : Interceptor
    {
        private readonly ILogger<LoggingInterceptor> _logger;

        public LoggingInterceptor(ILogger<LoggingInterceptor> logger) 
        {
            _logger = logger;
        }

        public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
            TRequest request,
            ServerCallContext context,
            UnaryServerMethod<TRequest, TResponse> continuation)
        {
            _logger.LogInformation($"#INFO# METHOD - [{context.Method}] REQUEST - [{request}]");

            var response = await continuation(request, context);

            _logger.LogInformation($"#INFO# METHOD - [{context.Method}] RESPONSE - [{response}]");

            return response;
        }

    }
}
