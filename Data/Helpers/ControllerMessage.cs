namespace ubank_api.Data.Helpers
{
    public class ControllerMessage
    {
        public string Message { get; set; } = string.Empty;

        public ControllerMessage(string message)
        {
            Message = message;
        }
    }
}
