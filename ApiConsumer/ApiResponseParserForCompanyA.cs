using CSharpFunctionalExtensions;

using Newtonsoft.Json;

namespace ApiConsumer
{
    public sealed class ApiResponseParserForCompanyA
    {
        private class ResponseDTO
        {
            [JsonProperty("total")]
            public decimal? Total { get; set; }
        }

        public Result<decimal> Parse(string apiResponse)
        {
            if (string.IsNullOrEmpty(apiResponse))
                return Result.Failure<decimal>("API response was not provided");

            try
            {
                var result = JsonConvert.DeserializeObject<ResponseDTO>(apiResponse);
                if(!result.Total.HasValue)
                    return Result.Failure<decimal>("API response did not come with the expected format");

                return result.Total.Value;
            }
            catch (JsonReaderException)
            {
                return Result.Failure<decimal>("API response did not come with the expected format");
            }
        }
    }
}
