using CSharpFunctionalExtensions;

using Newtonsoft.Json;

namespace ApiConsumer
{
    public sealed class ApiResponseParserForCompanyB
    {
        private class ResponseDTO
        {
            [JsonProperty("amount")]
            public decimal? Amount { get; set; }
        }

        public Result<decimal> Parse(string apiResponse)
        {
            if(string.IsNullOrEmpty(apiResponse))
                return Result.Failure<decimal>("API response was not provided");

            try
            {
                var result = JsonConvert.DeserializeObject<ResponseDTO>(apiResponse);
                if (!result.Amount.HasValue)
                    return Result.Failure<decimal>("API response did not come with the expected format");

                return result.Amount.Value;
            }
            catch (JsonReaderException)
            {
                return Result.Failure<decimal>("API response did not come with the expected format");
            }
        }
    }
}
