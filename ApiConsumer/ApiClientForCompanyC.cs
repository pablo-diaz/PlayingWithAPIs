using System.Threading.Tasks;

using CSharpFunctionalExtensions;

using Flurl;
using Flurl.Http;
using Flurl.Http.Xml;

namespace ApiConsumer
{
    public sealed class ApiClientForCompanyC: IApiClient
    {
        private readonly string _baseURL;
        private readonly string _apiKey;

        public ApiClientForCompanyC(string baseURL, string apiKey)
        {
            this._baseURL = baseURL;
            this._apiKey = apiKey;
        }

        public async Task<Result<string>> PerformRequest(string withXmlBody)
        {
            try
            {
                return await this._baseURL
                    .AppendPathSegment("quote.svc")
                    .WithHeader(name: "x-api-key", value: this._apiKey)
                    .PostXmlAsync(withXmlBody)
                    .ReceiveString();
            }
            catch (FlurlHttpException ex)
            {
                return Result.Failure<string>("Cannot get response from service. Error: " + ex.Message);
            }
        }
    }
}
