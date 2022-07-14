using System.Linq;
using System.Threading.Tasks;

using Core;

using Newtonsoft.Json;

using CSharpFunctionalExtensions;

namespace Application
{
    public sealed class ApiRequestorForCompanyA: IApiRequestor
    {
        private class RequestDTO
        {
            [JsonProperty("contactAddress")]
            public string ContactAddress { get; set; }

            [JsonProperty("warehouseAddress")]
            public string WarehouseAddress { get; set; }

            [JsonProperty("packageDimensions")]
            public PackageDimensionDTO[] PackageDimensions { get; set; }
        }

        private class PackageDimensionDTO
        {
            [JsonProperty("Width")]
            public decimal Width { get; set; }

            [JsonProperty("Height")]
            public decimal Height { get; set; }
        }

        private readonly IApiClient _apiClient;

        public ApiRequestorForCompanyA(IApiClient apiClient)
        {
            this._apiClient = apiClient;
        }

        public async Task<Result<decimal>> Request(RequestInfo forInfo)
        {
            if (forInfo == null)
                return Result.Failure<decimal>("Please provide a valid request info");

            var apiResponseResult = await this._apiClient.PerformRequest(withBody: BuildRequestBody(forInfo));
            if(apiResponseResult.IsFailure)
                return Result.Failure<decimal>(apiResponseResult.Error);

            return new ApiResponseParserForCompanyA().Parse(apiResponse: apiResponseResult.Value);
        }

        private string BuildRequestBody(RequestInfo basedOnInfo) =>
            JsonConvert.SerializeObject(MapToRequestDTO(basedOnInfo));

        private RequestDTO MapToRequestDTO(RequestInfo fromRequest) =>
            new RequestDTO {
                ContactAddress = fromRequest.SourceAddress.Value,
                WarehouseAddress = fromRequest.DestinationAddress.Value,
                PackageDimensions = fromRequest.CartonDimensions
                    .Select(cd => new PackageDimensionDTO {
                        Height = cd.Height,
                        Width = cd.Width
                    }).ToArray()
            };
    }
}
