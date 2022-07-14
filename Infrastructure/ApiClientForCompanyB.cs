using System.Threading.Tasks;

using Application;

using CSharpFunctionalExtensions;

using Flurl;
using Flurl.Http;

namespace Infrastrcuture
{
    public sealed class ApiClientForCompanyB: IApiClient
    {
        private readonly string _baseURL;
        private readonly string _apiKey;

        public ApiClientForCompanyB(string baseURL, string apiKey)
        {
            this._baseURL = baseURL;
            this._apiKey = apiKey;
        }

        public async Task<Result<string>> PerformRequest(string withJsonBody)
        {
            try
            {
                return await this._baseURL
                    .AppendPathSegment("get-quote")
                    .WithHeader(name: "x-api-key", value: this._apiKey)
                    .PostJsonAsync(withJsonBody)
                    .ReceiveString();
            }
            catch (FlurlHttpException ex)
            {
                return Result.Failure<string>("Cannot get response from service. Error: " + ex.Message);
            }
        }
    }
}
