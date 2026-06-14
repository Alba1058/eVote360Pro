namespace eVote360Pro.Core.Domain.Exceptions
{
    public class NotFoundException : BusinessException
    {
        public NotFoundException(string message) : base(message)
        {
        }

        public NotFoundException(string entityName, int id) : base($"{entityName} con id {id} no fue encontrado")
        {
        }
    }
}
