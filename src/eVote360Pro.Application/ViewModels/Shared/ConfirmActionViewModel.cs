namespace eVote360Pro.Core.Application.ViewModels.Shared
{
    public class ConfirmActionViewModel
    {
        public string Title { get; set; } = "Confirmar acción";
        public string Message { get; set; } = string.Empty;
        public string Controller { get; set; } = string.Empty;
        public string PostAction { get; set; } = string.Empty;
        public string CancelAction { get; set; } = "Index";
        public int EntityId { get; set; }
        public string ConfirmButtonText { get; set; } = "Aceptar";
        public string CancelButtonText { get; set; } = "Cancelar";
        public string ConfirmButtonClass { get; set; } = "btn-primary";
    }
}
