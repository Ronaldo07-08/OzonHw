using FluentValidation;
using Grpc.Core;
using Grpc.Core.Interceptors;

namespace Presentation.Grpc.Interceptors
{
    public class ValidationInterceptor : Interceptor
    {

        public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
            TRequest request,
            ServerCallContext context,
            UnaryServerMethod<TRequest, TResponse> continuation)
        {

            var validator = context.GetHttpContext().RequestServices.GetService<IValidator<TRequest>>();

            if (validator is not null)
            { 
                var validationResult = await validator.ValidateAsync(request, context.CancellationToken);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                    throw new RpcException(new Status(StatusCode.InvalidArgument, errors));
                }
            }

            return await continuation(request, context);
        }

    }
}
