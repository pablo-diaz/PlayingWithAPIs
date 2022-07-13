using System.Threading.Tasks;

using CSharpFunctionalExtensions;

namespace ApiConsumer
{
    public interface IApiRequestor
    {
        Task<Result<decimal>> Request(RequestInfo forInfo);
    }
}
