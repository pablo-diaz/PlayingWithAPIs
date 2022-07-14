using System;

using Core;

using FluentAssertions;

using NUnit.Framework;

namespace UnitTests.CoreTests
{
    public class ApiResponseParserForCompanyBTests
    {
        [Test]
        public void Test_WhenParsingApiResponse_IfResponseIsValid_ItWorks(
            [Values("{ 'amount': 55.89 }",
                    "{ 'Amount': 55.89 }",
                    "{ amount: 55.89 }",
                    "{ 'amount': '55.89' }",
                    "{ 'AMOUNT': 55.89 }" )] string goodResponse)
        {
            var parsedResult = new ApiResponseParserForCompanyB().Parse(goodResponse);

            parsedResult.Should().NotBeNull();

            if (parsedResult.IsFailure)
                Console.WriteLine(parsedResult.Error);

            parsedResult.IsSuccess.Should().BeTrue();

            parsedResult.Value.Should().Be(55.89M);
        }

        [Test]
        public void Test_WhenParsingApiResponse_IfResponseIsInvalid_ItShouldBeCaught(
            [Values("{ 'amountx': 55.89 }",
                    "{ 'cantidad': 55.89 }",
                    "{ 'amount': abc }",
                    "amount: 55.89",
                    null, "" )] string wrongResponse)
        {
            var parsedResult = new ApiResponseParserForCompanyB().Parse(wrongResponse);
            parsedResult.IsFailure.Should().BeTrue();
        }
    }
}
