using System.Collections.Generic;

using CSharpFunctionalExtensions;

namespace Core
{
    public sealed class Address: ValueObject
    {
        public string Value { get; }

        private Address(string value)
        {
            this.Value = value;
        }

        public static Result<Address> Create(string value)
        {
            if (string.IsNullOrEmpty(value))
                return Result.Failure<Address>("Please provide an address");

            if(value.Length < 5 || value.Length > 100)
                return Result.Failure<Address>("Address does not meet length requirements");

            return new Address(value);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return this.Value;
        }
    }
}
