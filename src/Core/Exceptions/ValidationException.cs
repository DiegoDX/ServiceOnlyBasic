namespace Core.Exceptions
{
    public class ValidationException: DomainException
    {
        public IDictionary<string, string[]> Errors { get; }

        public ValidationException(IDictionary<string, string[]> errors)
            : base("One or more validation errors occurred.")
        {
            Errors = errors;
        }

        public ValidationException(string propertyName, string errorMessage)
            :base($"Validation failder for '{propertyName}' : {errorMessage}")
        {
            Errors = new Dictionary<string, string[]>
            {
                { propertyName, new[] { errorMessage } }
            };
        }
    }
}
