using System.Threading.Tasks;

using CSharpFunctionalExtensions;

namespace ApiConsumer
{
    public interface IApiClient
    {
        Task<Result<string>> PerformRequest(string withJsonBody);
    }
}
