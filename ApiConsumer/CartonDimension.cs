using System.Collections.Generic;

using CSharpFunctionalExtensions;

namespace ApiConsumer
{
    public sealed class CartonDimension: ValueObject
    {
        public decimal Height { get; }
        public decimal Width { get; }

        private CartonDimension(decimal height, decimal width)
        {
            Height = height;
            Width = width;
        }

        public static Result<CartonDimension> Create(decimal height, decimal width)
        {
            if (height <= 0)
                return Result.Failure<CartonDimension>("Please provide a valid Height");

            if (width <= 0)
                return Result.Failure<CartonDimension>("Please provide a valid Width");

            return new CartonDimension(height: height, width: width);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return this.Height;
            yield return this.Width;
        }
    }
}
