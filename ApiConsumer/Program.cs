using System;
using System.Threading.Tasks;

namespace ApiConsumer
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var priceComparer = new PriceComparer(thirdPartyServices: GetRealThirdPartyServices());
            var bestPriceResult = await priceComparer.GetBestPrice(forRequest: BuildSampleRequestInfo());
            if(bestPriceResult.IsFailure)
                Console.WriteLine($"There was an issue trying to get the best price. Error: {bestPriceResult.Error}");

            Console.WriteLine($"Best price is: {bestPriceResult.Value}");
        }

        private static IApiRequestor[] GetRealThirdPartyServices() =>
            new ThirdPartyServiceFactory(new ServicesConfig {
                CompanyAService = new ServicesConfig.ServiceInfo {
                    BaseUrl = "https://www.companya-com",
                    apiKey = "d97ef962-6cef-48c4-bc5f-d11d77cd122e"
                },

                CompanyBService = new ServicesConfig.ServiceInfo
                {
                    BaseUrl = "https://www.companyb-com",
                    apiKey = "9361df50-0a63-4ac9-a860-2b06e090f273"
                },

                CompanyCService = new ServicesConfig.ServiceInfo
                {
                    BaseUrl = "https://www.companyc-com",
                    apiKey = "b7735eb2-0bde-426f-ace4-40c073a5f12a"
                }
            }).GetAvailableThirdPartyServices();

        // we can turn this part into something that interacts with user to get user input
        // for the moment I'm hard-conding it
        private static RequestInfo BuildSampleRequestInfo() =>
            RequestInfo.Create(
                sourceAddress: Address.Create("A source address").Value,
                destinationAddress: Address.Create("A destination address").Value,
                cartonDimensions: new CartonDimension[] {
                    CartonDimension.Create(height: 15M, width: 25M).Value,
                    CartonDimension.Create(height: 10M, width: 36.59M).Value
                }).Value;
    }
}
