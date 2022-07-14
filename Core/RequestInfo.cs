using System.Collections.Generic;

using CSharpFunctionalExtensions;

namespace Core
{
    public sealed class RequestInfo: ValueObject
    {
        public Address SourceAddress { get; }
        public Address DestinationAddress { get; }

        private readonly List<CartonDimension> _cartonDimensions = new List<CartonDimension>();
        public ICollection<CartonDimension> CartonDimensions => _cartonDimensions;

        private RequestInfo(Address sourceAddress, Address destinationAddress,
            CartonDimension[] cartonDimensions)
        {
            SourceAddress = sourceAddress;
            DestinationAddress = destinationAddress;
            _cartonDimensions = new List<CartonDimension>(cartonDimensions);
        }

        public static Result<RequestInfo> Create(Address sourceAddress,
            Address destinationAddress, CartonDimension[] cartonDimensions)
        {
            if (sourceAddress == null)
                return Result.Failure<RequestInfo>("Please provide a source address");

            if (destinationAddress == null)
                return Result.Failure<RequestInfo>("Please provide a destination address");

            if (cartonDimensions == null || cartonDimensions.Length == 0)
                return Result.Failure<RequestInfo>("Please provide at least one carton dimension");

            return new RequestInfo(sourceAddress: sourceAddress,
                destinationAddress: destinationAddress, cartonDimensions: cartonDimensions);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return SourceAddress;
            yield return DestinationAddress;
        }
    }
}
