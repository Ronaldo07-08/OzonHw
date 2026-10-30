using Domain.Exceptions;
using FluentValidation;
using Grpc.Core;
using Grpc.Core.Interceptors;

namespace Presentation.Grpc.Interceptors
{
    public class ExceptionHandlingInterceptor : Interceptor
    {
        public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
            TRequest request,
            ServerCallContext context,
            UnaryServerMethod<TRequest, TResponse> continuation)
        {
            try
            {
                return await continuation(request, context);
            }
            catch (CannotGetProductException ex)
            {
                throw new RpcException(new Status(StatusCode.NotFound, ex.Message));
            }
            catch (CannotCreateProductException ex)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, ex.Message));
            }
            catch (Exception ex) when (ex is not RpcException)
            {
                throw new RpcException(new Status(StatusCode.Internal, ex.Message));
            }

        }
    }
}
