using System;
using System.Linq;

using Core;

using FluentAssertions;

using NUnit.Framework;

namespace UnitTests.CoreTests
{
    public class RequestInfoTests
    {
        [Test]
        public void Test_WhenCreatingRequestInfo_IfItIsValid_ItWorks()
        {
            var sourceAddress = Address.Create("A source address").Value;
            var destinationAddress = Address.Create("A destination address").Value;
            var cartonDimensions = new CartonDimension[] {
                CartonDimension.Create(height: 15M, width: 30M).Value,
                CartonDimension.Create(height: 10M, width: 25.6M).Value
            };

            var newRequestInfoResult = RequestInfo.Create(sourceAddress: sourceAddress,
                destinationAddress: destinationAddress, cartonDimensions: cartonDimensions);

            newRequestInfoResult.Should().NotBeNull();

            if (newRequestInfoResult.IsFailure)
                Console.WriteLine(newRequestInfoResult.Error);

            newRequestInfoResult.IsSuccess.Should().BeTrue();

            newRequestInfoResult.Value.SourceAddress.Should().Be(sourceAddress);
            newRequestInfoResult.Value.DestinationAddress.Should().Be(destinationAddress);

            newRequestInfoResult.Value.CartonDimensions.Should().HaveCount(2);

            var firstCartonDimension = newRequestInfoResult.Value.CartonDimensions.FirstOrDefault(cd => cd.Height == 15M);
            firstCartonDimension.Should().NotBeNull();
            firstCartonDimension.Width.Should().Be(30M);

            var secondCartonDimension = newRequestInfoResult.Value.CartonDimensions.FirstOrDefault(cd => cd.Height == 10M);
            secondCartonDimension.Should().NotBeNull();
            secondCartonDimension.Width.Should().Be(25.6M);
        }

        [Test]
        public void Test_WhenCreatingRequestInfo_IfWrongSourceAddressIsProvided_ItFails()
        {
            var destinationAddress = Address.Create("A destination address").Value;
            var cartonDimensions = new CartonDimension[] {
                CartonDimension.Create(height: 15M, width: 30M).Value,
                CartonDimension.Create(height: 10M, width: 25.6M).Value
            };

            var newRequestInfoResult = RequestInfo.Create(sourceAddress: null,
                destinationAddress: destinationAddress, cartonDimensions: cartonDimensions);

            newRequestInfoResult.IsFailure.Should().BeTrue();
        }

        [Test]
        public void Test_WhenCreatingRequestInfo_IfWrongDestinationAddressIsProvided_ItFails()
        {
            var sourceAddress = Address.Create("A source address").Value;
            var cartonDimensions = new CartonDimension[] {
                CartonDimension.Create(height: 15M, width: 30M).Value,
                CartonDimension.Create(height: 10M, width: 25.6M).Value
            };

            var newRequestInfoResult = RequestInfo.Create(sourceAddress: sourceAddress,
                destinationAddress: null, cartonDimensions: cartonDimensions);

            newRequestInfoResult.IsFailure.Should().BeTrue();
        }

        [Test]
        public void Test_WhenCreatingRequestInfo_IfWrongCartonDimensionsAreProvided_ItFails(
            [Values(true, false)] bool shouldItSendANull)
        {
            var sourceAddress = Address.Create("A source address").Value;
            var destinationAddress = Address.Create("A destination address").Value;

            var cartonDimensions = shouldItSendANull ? null : new CartonDimension[0];

            var newRequestInfoResult = RequestInfo.Create(sourceAddress: sourceAddress,
                destinationAddress: null, cartonDimensions: cartonDimensions);

            newRequestInfoResult.IsFailure.Should().BeTrue();
        }
    }
}
