
namespace ctripRedisHelp
{
    class inTimeMethedEntity
    {
        string method = string.Empty;
        string payResult = string.Empty;

        public string PayResult
        {
            get { return payResult; }
            set { payResult = value; }
        }

        public string Method
        {
            get { return method; }
            set { method = value; }
        }
    }
}
