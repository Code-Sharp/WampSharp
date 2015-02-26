namespace WampSharp.SignalR
{
    internal class ConnectionListenerSettings
    {
        public bool EnableCors { get; set; }
        public bool EnableJSONP { get; set; }
        public string PathMatch { get; set; }
    }
}