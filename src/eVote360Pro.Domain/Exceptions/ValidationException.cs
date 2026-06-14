namespace eVote360Pro.Core.Domain.Exceptions
{
    public class ValidationException : BusinessException
    {
        public ValidationException(string message) : base(message)
        {
        }

        public ValidationException(List<string> errors) : base(string.Join(", ", errors))
        {
            Errors = errors;
        }

        public List<string> Errors { get; set; } = new List<string>();
    }
}
