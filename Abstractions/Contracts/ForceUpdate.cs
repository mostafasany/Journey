namespace Contracts
{
    public class ForceUpdate
    {
        public bool IsForceUpdate { get; set; }
        public bool IsShutdown { get; set; }
        public string Message { get; set; }
        public string Version { get; set; }
    }
}