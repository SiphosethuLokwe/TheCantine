namespace Cantina.Application.DTO { 
    public class CommandResponse<T>:CommandResponseBase
    {
        public T? Data { get; set; }
        public CommandResponse() { }
    }
}
