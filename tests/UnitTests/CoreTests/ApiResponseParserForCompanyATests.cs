using System;

using Core;

using FluentAssertions;

using NUnit.Framework;

namespace UnitTests.CoreTests
{
    public class ApiResponseParserForCompanyATests
    {
        [Test]
        public void Test_WhenParsingApiResponse_IfResponseIsValid_ItWorks(
            [Values("{ 'total': 45.6 }",
                    "{ 'Total': 45.6 }",
                    "{ total: 45.6 }",
                    "{ 'total': '45.6' }",
                    "{ 'TOTAL': 45.6 }" )] string goodResponse)
        {
            var parsedResult = new ApiResponseParserForCompanyA().Parse(goodResponse);

            parsedResult.Should().NotBeNull();

            if (parsedResult.IsFailure)
                Console.WriteLine(parsedResult.Error);

            parsedResult.IsSuccess.Should().BeTrue();

            parsedResult.Value.Should().Be(45.6M);
        }

        [Test]
        public void Test_WhenParsingApiResponse_IfResponseIsInvalid_ItShouldBeCaught(
            [Values("{ 'totalx': 45.6 }",
                    "{ 'totales': 45.6 }",
                    "{ 'total': abc }",
                    "total: 45.6",
                    null, "" )] string wrongResponse)
        {
            var parsedResult = new ApiResponseParserForCompanyA().Parse(wrongResponse);
            parsedResult.IsFailure.Should().BeTrue();
        }
    }
}