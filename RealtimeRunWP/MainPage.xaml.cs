using System;
using System.Diagnostics;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows.Navigation;
using Windows.Devices.Geolocation;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.Phone.Controls;

namespace RealtimeRunWP
{
    public partial class MainPage : PhoneApplicationPage
    {
        public const string SiteUrl = "(the web app root URL, e.g. http://mymachine/rtr)";
        private IHubProxy _hub;
        private HubConnection _hubConnection;
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            Loaded += (s, e) => StartSignalRHub();

            Loaded += (s, e) => StartGeolocation();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            StartGeolocation();
        }

        public void StartGeolocation()
        {
            if (App.Geolocator != null) return;

            App.Geolocator = new Geolocator { DesiredAccuracy = PositionAccuracy.High, MovementThreshold = 10 }; // 10 meters (to limit data transmission)
            App.Geolocator.PositionChanged += Geolocator_PositionChanged;
        }

        private void Geolocator_PositionChanged(Geolocator sender, PositionChangedEventArgs args)
        {
            var roundedLat = Math.Round(args.Position.Coordinate.Latitude, 6);
            var roundedLon = Math.Round(args.Position.Coordinate.Longitude, 6);
            var altitude = args.Position.Coordinate.Altitude;
            var speed = args.Position.Coordinate.Speed != null && Double.IsNaN((double)args.Position.Coordinate.Speed) ? 0 : args.Position.Coordinate.Speed;

            Debug.WriteLine("{0}, {1} altitude: {2}, speed: {3}", roundedLat, roundedLon, altitude, speed);

            SendMessage(roundedLat.ToString(CultureInfo.InvariantCulture),
                roundedLon.ToString(CultureInfo.InvariantCulture), altitude, speed);

            Dispatcher.BeginInvoke(() =>
                TblockLatLonBlock.Text =
                    String.Format("{0}, {1} altitude: {2}, speed: {3}", roundedLat, roundedLon, altitude, speed));
        }

        public async Task StartSignalRHub()
        {
            try
            {
                _hubConnection = new HubConnection(SiteUrl);

                _hub = _hubConnection.CreateHubProxy("MapHub");

                await _hubConnection.Start();
            }
            catch (Exception err)
            {
                Debug.WriteLine(err.Message);
            }
        }

        public void SendMessage(string lat, string lon, double? altitude, double? speed)
        {
            try
            {
                _hub.Invoke("send", lat, lon, altitude, speed);
            }
            catch (Exception err)
            {
                Debug.WriteLine(err.Message);
            }
        }
    }
}