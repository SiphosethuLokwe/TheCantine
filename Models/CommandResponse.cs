namespace TheCantine.Models
{
    public class CommandResponse<T>:CommandResponseBase
    {
        public T? Data { get; set; }
        // Constructor for generic type
        public CommandResponse() { }
        public CommandResponse(T? data, bool success, string? message)
        {
            Data = data;
            Success = success;
            Message = message;
        }

        // Constructor for non-generic type
        public CommandResponse(bool success, string? message)
        {
            Success = success;
            Message = message;
        }

    }
}
