using System;

using ApiConsumer;

using FluentAssertions;

using NUnit.Framework;

namespace UnitTests
{
    public class AddressTests
    {
        [Test]
        public void Test_WhenCreatingAddress_IfItIsValid_ItWorks(
            [Values("A valid address", "++5++", "++100++")] string validAddressPattern)
        {
            var validAddress = Util.Utilities.ReturnValueAccordingToPatternString(validAddressPattern);

            var addressResult = Address.Create(validAddress);

            addressResult.Should().NotBeNull();

            if (addressResult.IsFailure)
                Console.WriteLine(addressResult.Error);

            addressResult.IsSuccess.Should().BeTrue();

            addressResult.Value.Value.Should().Be(validAddress);
        }

        [Test]
        public void Test_WhenCreatingAddress_IfItIsInvalid_ItFails(
            [Values(null, "", "++4++", "++101++")] string wrongAddressPattern)
        {
            var wrongAddress = Util.Utilities.ReturnValueAccordingToPatternString(wrongAddressPattern);

            var addressResult = Address.Create(wrongAddress);

            addressResult.IsFailure.Should().BeTrue();
        }
    }
}
