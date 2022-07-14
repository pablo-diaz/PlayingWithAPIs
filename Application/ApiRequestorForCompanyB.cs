using System.Linq;
using System.Threading.Tasks;

using Core;

using Newtonsoft.Json;

using CSharpFunctionalExtensions;

namespace Application
{
    public sealed class ApiRequestorForCompanyB: IApiRequestor
    {
        private class RequestDTO
        {
            [JsonProperty("consignee")]
            public string Consignee { get; set; }

            [JsonProperty("consignor")]
            public string Consignor { get; set; }

            [JsonProperty("cartons")]
            public CartonDTO[] Cartons { get; set; }
        }

        private class CartonDTO
        {
            [JsonProperty("Width")]
            public decimal Width { get; set; }

            [JsonProperty("Height")]
            public decimal Height { get; set; }
        }

        private readonly IApiClient _apiClient;

        public ApiRequestorForCompanyB(IApiClient apiClient)
        {
            this._apiClient = apiClient;
        }

        public async Task<Result<decimal>> Request(RequestInfo forInfo)
        {
            if (forInfo == null)
                return Result.Failure<decimal>("Please provide a valid request info");

            var apiResponseResult = await this._apiClient.PerformRequest(withBody: BuildRequestBody(forInfo));
            if (apiResponseResult.IsFailure)
                return Result.Failure<decimal>(apiResponseResult.Error);

            return new ApiResponseParserForCompanyB().Parse(apiResponse: apiResponseResult.Value);
        }

        private string BuildRequestBody(RequestInfo basedOnInfo) =>
            JsonConvert.SerializeObject(MapToRequestDTO(basedOnInfo));

        private RequestDTO MapToRequestDTO(RequestInfo fromRequest) =>
            new RequestDTO
            {
                Consignee = fromRequest.SourceAddress.Value,
                Consignor = fromRequest.DestinationAddress.Value,
                Cartons  = fromRequest.CartonDimensions
                    .Select(cd => new CartonDTO {
                        Height = cd.Height,
                        Width = cd.Width
                    }).ToArray()
            };
    }
}
