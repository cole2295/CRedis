using StackExchange.Redis;
using System;
using System.Threading;
namespace ctripRedisHelp
{
    public interface IhelpBase
    {
        void Dispose();
        string get(string key);
        StackExchange.Redis.ISubscriber getSubscriber();
        bool IsConnected { get; set; }
        string InValue { get; set; }
        event EventHandler<onValueEventArgs> onValueArrive;
        long publish(string publishItem, string value);
        long publishAndSet(string publishItem, string value);
        bool set(string key, string value, TimeSpan ts);
        bool setnx(string key, string value, TimeSpan ts);
        void subscriber(string SubscribeItem);
        void unSubscriber(string SubscribeItem);
        void valueProcessor(string value);
        bool Wait(int timeOut);
    }

    public class onValueEventArgs : EventArgs
    {
        string value = string.Empty;

        public string Value
        {
            get { return this.value; }
            set { this.value = value; }
        }
    }
}
