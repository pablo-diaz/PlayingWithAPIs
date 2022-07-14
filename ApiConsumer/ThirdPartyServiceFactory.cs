using Application;
using Infrastrcuture;

namespace ApiConsumer
{
    public sealed class ThirdPartyServiceFactory
    {
        private readonly ServicesConfig _servicesConfiguration;

        public ThirdPartyServiceFactory(ServicesConfig servicesConfiguration)
        {
            this._servicesConfiguration = servicesConfiguration;
        }

        public IApiRequestor[] GetAvailableThirdPartyServices() =>
            new IApiRequestor[] {
                new ApiRequestorForCompanyA(apiClient: new ApiClientForCompanyA(
                    baseURL: this._servicesConfiguration.CompanyAService.BaseUrl,
                    apiKey: this._servicesConfiguration.CompanyAService.apiKey)),

                new ApiRequestorForCompanyB(apiClient: new ApiClientForCompanyB(
                    baseURL: this._servicesConfiguration.CompanyBService.BaseUrl,
                    apiKey: this._servicesConfiguration.CompanyBService.apiKey)),

                new ApiRequestorForCompanyC(apiClient: new ApiClientForCompanyC(
                    baseURL: this._servicesConfiguration.CompanyCService.BaseUrl,
                    apiKey: this._servicesConfiguration.CompanyCService.apiKey))
            };
    }
}
