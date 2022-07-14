using System;

using Core;

using FluentAssertions;

using NUnit.Framework;

namespace UnitTests.CoreTests
{
    public class CartonDimensionTests
    {
        [Test]
        public void Test_WhenCreatingCartonDimension_IfItIsValid_ItWorks()
        {
            var cartonDimensionResult = CartonDimension.Create(height: 15M, width: 35M);

            cartonDimensionResult.Should().NotBeNull();

            if (cartonDimensionResult.IsFailure)
                Console.WriteLine(cartonDimensionResult.Error);

            cartonDimensionResult.IsSuccess.Should().BeTrue();

            cartonDimensionResult.Value.Height.Should().Be(15M);
            cartonDimensionResult.Value.Width.Should().Be(35M);
        }

        [Test]
        public void Test_WhenCreatingCartonDimension_IfItIsInvalid_ItFails(
            [Values("0, 0", "-1, 10", "15, -6", "-5, -4", "10, 0", "0, 45")] string wrongCartonDimensionPattern)
        {
            (var withHeight, var withWidth) = UnwrapFromPattern(wrongCartonDimensionPattern);

            var cartonDimensionResult = CartonDimension.Create(height: withHeight, width: withWidth);

            cartonDimensionResult.IsFailure.Should().BeTrue();
        }

        private (decimal Height, decimal Width) UnwrapFromPattern(string cartonDimensionPattern)
        {
            var parts = cartonDimensionPattern.Split(",");
            return (Height: Convert.ToDecimal(parts[0]), Width: Convert.ToDecimal(parts[1]));
        }
    }
}
