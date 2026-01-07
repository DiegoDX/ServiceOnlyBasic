using Core.Exceptions;

namespace Core.ValueObjects
{
    /// <summary>
    /// Represents an ISO country code (2 or 3 letters).
    /// </summary>
    public sealed class  CountryCode
    {
        public string Value { get; }

        public CountryCode(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ValidationException("Code", "Country code cannot be empty.");
            }

            value = value.Trim().ToUpperInvariant();

            if (value.Length is < 2 or > 3)
            {
                throw new ValidationException("Code", "Country code must be 2 or 3 letters.");
            }

            Value = value.ToUpperInvariant();
        }

        public override string ToString() => Value;
        public override bool Equals(Object? obj) => obj is CountryCode other && Value == other.Value;
        public override int GetHashCode() => Value.GetHashCode();
    }
}
