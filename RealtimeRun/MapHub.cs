using Microsoft.AspNet.SignalR;

namespace RealtimeRun
{
    public class MapHub : Hub
    {
        public void Send(string lat, string lon, double? altitude, double? speed)
        {
            Clients.All.broadcastLatLon(lat, lon, altitude, speed);
        }
    }
}