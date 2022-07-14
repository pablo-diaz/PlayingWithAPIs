using System.Threading.Tasks;

using Core;

using CSharpFunctionalExtensions;

namespace Application
{
    public interface IApiRequestor
    {
        Task<Result<decimal>> Request(RequestInfo forInfo);
    }
}
