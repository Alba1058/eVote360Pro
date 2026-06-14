namespace eVote360Pro.Core.Domain.Exceptions
{
    public class UnauthorizedException : BusinessException
    {
        public UnauthorizedException(string message) : base(message)
        {
        }

        public UnauthorizedException() : base("No tiene autorización para realizar esta acción")
        {
        }
    }
}
