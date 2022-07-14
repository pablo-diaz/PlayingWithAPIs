using System;
using System.Threading.Tasks;

using Core;
using Application;

using FluentAssertions;

using NUnit.Framework;

using NSubstitute;

namespace UnitTests.ApplicationTests
{
    public class PriceComparerTests
    {
        [Test]
        public async Task Test_WhenComparingPrices_IfServicesAreAvailable_TheBestPriceIsFound()
        {
            var priceComparerService = new PriceComparer(thirdPartyServices: BuildFakeThirdPartyServices());
            var bestPriceResult = await priceComparerService.GetBestPrice(forRequest: BuildSampleRequestInfo());

            bestPriceResult.Should().NotBeNull();

            if (bestPriceResult.IsFailure)
                Console.WriteLine(bestPriceResult.Error);

            bestPriceResult.IsSuccess.Should().BeTrue();

            bestPriceResult.Value.Should().Be(17.58M);
        }

        [Test]
        public async Task Test_WhenComparingPrices_IfSomeServiceDoesNotReturnTheRequiredResponse_ItFails()
        {
            var priceComparerService = new PriceComparer(thirdPartyServices:
                BuildFakeThirdPartyServicesWithWrongResultsBeingReturnedFromServices());

            var bestPriceResult = await priceComparerService.GetBestPrice(forRequest: BuildSampleRequestInfo());

            bestPriceResult.IsFailure.Should().BeTrue();
        }

        #region Helpers

        private IApiRequestor[] BuildFakeThirdPartyServices()
        {
            var companyAServiceMock = Substitute.For<IApiClient>();
            companyAServiceMock.PerformRequest(Arg.Any<string>()).Returns("{ 'total': 30 }");

            var companyBServiceMock = Substitute.For<IApiClient>();
            companyBServiceMock.PerformRequest(Arg.Any<string>()).Returns("{ 'amount': 17.58 }");

            var companyCServiceMock = Substitute.For<IApiClient>();
            companyCServiceMock.PerformRequest(Arg.Any<string>()).Returns(@"<?xml version=""1.0"" ?> <quote total=""50"" />");

            return new IApiRequestor[] {
                new ApiRequestorForCompanyA(apiClient: companyAServiceMock),
                new ApiRequestorForCompanyB(apiClient: companyBServiceMock),
                new ApiRequestorForCompanyC(apiClient: companyCServiceMock)
            };
        }

        private IApiRequestor[] BuildFakeThirdPartyServicesWithWrongResultsBeingReturnedFromServices()
        {
            var companyAServiceMock = Substitute.For<IApiClient>();
            companyAServiceMock.PerformRequest(Arg.Any<string>()).Returns("{ 'total': 30 }");

            var companyBServiceMock = Substitute.For<IApiClient>();
            companyBServiceMock.PerformRequest(Arg.Any<string>()).Returns("{ 'amount': abc }");  // this service returns response with no standard, thus parsing it should fail

            var companyCServiceMock = Substitute.For<IApiClient>();
            companyCServiceMock.PerformRequest(Arg.Any<string>()).Returns(@"<?xml version=""1.0"" ?> <quote total=""50"" />");

            return new IApiRequestor[] {
                new ApiRequestorForCompanyA(apiClient: companyAServiceMock),
                new ApiRequestorForCompanyB(apiClient: companyBServiceMock),
                new ApiRequestorForCompanyC(apiClient: companyCServiceMock)
            };
        }

        private static RequestInfo BuildSampleRequestInfo() =>
            RequestInfo.Create(
                sourceAddress: Address.Create("A source address").Value,
                destinationAddress: Address.Create("A destination address").Value,
                cartonDimensions: new CartonDimension[] {
                    CartonDimension.Create(height: 15M, width: 25M).Value,
                    CartonDimension.Create(height: 10M, width: 36.59M).Value
                }).Value;

        #endregion
    }
}
