using System.Numerics;

namespace CourseService.Domain.Exceptions
{
    public class DomainValidationException : DomainException
    {
        public DomainValidationException(string message) : base(message) { }

        public static void ThrowIfStringOutOfLength(string value, int minLength, int maxLength, string? fieldName = null)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new DomainValidationException($"Field {fieldName} cannot be empty string or white space");
            }

            int length = value.Length;

            if (length < minLength || length > maxLength)
            {
                throw new DomainValidationException($"Field {fieldName} must be between {minLength} and {maxLength}");
            }
        }

        public static void ThrowIfNegative<T>(T value, string? fieldName = null) where T : INumberBase<T>
        {
            if (T.IsNegative(value))
            {
                throw new DomainValidationException($"Field {fieldName} must not negative.");
            }
        }
    }
}
