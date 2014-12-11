using Microsoft.AspNet.SignalR;

namespace RealtimeRun
{
    public class MapHub : Hub
    {
        // The Send method is used on a client to deliver data to the server
        public void Send(string lat, string lon, double? altitude, double? speed)
        {
            // In return, the server broadcasts the data to all clients. 
            // Those that have the broadcastLatLon method will use the data on the client
            Clients.All.broadcastLatLon(lat, lon, altitude, speed);
        }
    }
}