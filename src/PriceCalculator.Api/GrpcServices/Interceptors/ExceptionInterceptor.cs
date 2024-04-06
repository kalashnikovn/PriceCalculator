using Grpc.Core;
using Grpc.Core.Interceptors;
using PriceCalculator.Bll.Exceptions;

namespace PriceCalculator.Api.GrpcServices.Interceptors;

public sealed class ExceptionInterceptor : Interceptor
{
    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request, ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        try
        {
            return await continuation(request, context);
        }
        catch (OneOrManyCalculationsNotFoundException)
        {
            throw new RpcException(
                new Status(StatusCode.NotFound, "One or many calculations not found"));
        }
        catch (OneOrManyCalculationsBelongsToAnotherUserException)
        {
            throw new RpcException(
                new Status(StatusCode.PermissionDenied, "One or many calculations belongs to another user"));
        }
        catch (RpcException)
        {
            throw;
        }
        catch (Exception exception)
        {
            throw new RpcException(new Status(StatusCode.Internal, exception.Message));
        }
    }
}