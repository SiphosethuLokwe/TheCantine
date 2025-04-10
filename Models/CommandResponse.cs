namespace TheCantine.Models
{
    public class CommandResponse<T>:CommandResponseBase
    {
        public T? Data { get; set; }
        // Constructor for generic type
        public CommandResponse() { }
    }
}
