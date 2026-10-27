namespace MyKitchen_Core.Tools
{
    public class Response<T>
    {
        public enum responseStatus
        {
            success,
            warning,
            error
        }

        private T _data;
        private string _message;
        private responseStatus _status;

        public T data
        {
            get { return _data; }
            private set { _data = value; }
        }
        public string message
        {
            get { return _message; }
            private set { _message = value; }
        }
        public responseStatus status
        {
            get { return _status; }
            private set { _status = value; }
        }
        public static Response<T> createResponse(T data, string message, responseStatus status)
        {
        return new Response<T>()
        {
            data = data,
            message = message,
            status = status
        };
        }
    }
}