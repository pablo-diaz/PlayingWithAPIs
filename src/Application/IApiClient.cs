using System.Threading.Tasks;

using CSharpFunctionalExtensions;

namespace Application
{
    public interface IApiClient
    {
        Task<Result<string>> PerformRequest(string withBody);
    }
}
