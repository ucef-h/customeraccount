namespace Customer.Application
{
    public class ResponseBase<T>
    {
        public bool Success { get; set; }
        public string[] Errors { get; set; }
        public T Response { get; set; }

        public ResponseBase()
        {
        }

        public ResponseBase(bool success) : this()
        {
            Success = success;
        }

        public ResponseBase(string[] errors) : this(false)
        {
            Errors = errors;
        }

        public ResponseBase(T response, bool success) : this(success)
        {
            Response = response;
        }

        public ResponseBase(T response, params string[] errors) : this(response, false)
        {
            Errors = errors;
        }
    }
}