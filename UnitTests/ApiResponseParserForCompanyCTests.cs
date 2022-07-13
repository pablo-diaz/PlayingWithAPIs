using System;

using ApiConsumer;

using FluentAssertions;

using NUnit.Framework;

namespace UnitTests
{
    public class ApiResponseParserForCompanyCTests
    {
        [Test]
        public void Test_WhenParsingApiResponse_IfResponseIsValid_ItWorks(
            [Values(@"<?xml version=""1.0"" ?> <quote total=""12.45"" />",
                    @"<?xml version=""1.0"" ?> <Quote Total=""12.45"" />",
                    "<?xml version='1.0' ?> <quote total='12.45' />")] string goodResponse)
        {
            var parsedResult = new ApiResponseParserForCompanyC().Parse(goodResponse);

            parsedResult.Should().NotBeNull();

            if (parsedResult.IsFailure)
                Console.WriteLine(parsedResult.Error);

            parsedResult.IsSuccess.Should().BeTrue();

            parsedResult.Value.Should().Be(12.45M);
        }

        [Test]
        public void Test_WhenParsingApiResponse_IfResponseIsInvalid_ItShouldBeCaught(
            [Values(null, "", "not an xml document",
                    @"<?xml version=""1.0"" ?> <quotes total=""12.45"" />",
                    @"<?xml version=""1.0"" ?> <quotes><quote total=""12.45"" /></quotes>",
                    @"<?xml version=""1.0"" ?> <quote total=12.45 />",
                    @"<?xml version=""1.0"" ?> <quote total=""abc"" />")] string wrongResponse)
        {
            var parsedResult = new ApiResponseParserForCompanyC().Parse(wrongResponse);
            parsedResult.IsFailure.Should().BeTrue();
        }
    }
}
