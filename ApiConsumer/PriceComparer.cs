using System.Linq;
using System.Threading.Tasks;

using CSharpFunctionalExtensions;

namespace ApiConsumer
{
    public sealed class PriceComparer
    {
        private readonly IApiRequestor[] _thirdPartyServices;

        public PriceComparer(IApiRequestor[] thirdPartyServices)
        {
            this._thirdPartyServices = thirdPartyServices;
        }

        public async Task<Result<decimal>> GetBestPrice(RequestInfo forRequest)
        {
            var requestsResult = await Task.WhenAll(
                _thirdPartyServices.Select(s => s.Request(forRequest))
            );

            if (requestsResult.Any(r => r.IsFailure))
                return Result.Failure<decimal>(requestsResult.First(r => r.IsFailure).Error);

            return requestsResult.Select(r => r.Value).Min();
        }
    }
}
