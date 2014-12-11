using System;
using System.Diagnostics;
using Owin;

namespace RealtimeRun
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            try
            {
                app.MapSignalR();
            }
            catch (Exception err)
            {
                Debug.WriteLine(err.Message);
            }

            // GlobalHost.Configuration.ConnectionTimeout = TimeSpan.FromSeconds(600);
        }
    }
}