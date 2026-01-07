namespace Core.Exceptions
{
    public class ForbiddenException : DomainException
    {
        public ForbiddenException(string message) : base(message)
        {
        }
    }
}
