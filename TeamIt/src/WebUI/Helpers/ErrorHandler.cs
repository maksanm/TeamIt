using Blazorise;

namespace WebUI.Helpers
{
    public static class ErrorHandler
    {
        public static async Task HandleError(INotificationService service, string message = "Unhandled error happend. Please reload page and try again") => await service.Error(message, "Error");
    }
}
