using EcobankRepartidor.VistaModelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using Xamarin.Forms.Xaml;

namespace EcobankRepartidor.Vistas.Confi
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Paglocalizar : ContentPage
    {
        Pin punto = new Pin();
        public static double latitud = 0;
        public static double longitud = 0;
        string ciudad;
        string pais;
        string Idlocalidad;
        public Paglocalizar()
        {
            InitializeComponent();
            BindingContext = new VMconfclientes(Navigation);
        }
        protected override async void OnAppearing()
        {
            punto = new Pin()
            {
                Label = "Tu ubicación",
                Type = PinType.Place,
                Icon = (Device.RuntimePlatform == Device.Android) ? BitmapDescriptorFactory.FromBundle("pin1.png") : BitmapDescriptorFactory.FromView(new Image() { Source = "pin1.png", WidthRequest = 64, HeightRequest = 64 }),
                Position = new Position(0, 0),
                IsDraggable = true
            };
            map.Pins.Add(punto);
            ApplicarTema();
            await LocalizacionActual();
        }

        private void ApplicarTema()
        {
            var assembly = typeof(MainPage).GetTypeInfo().Assembly;
            var stream = assembly.GetManifestResourceStream($"EcobankRepartidor.Recursos.Maps.MapTheme1.json");
            string themefile;
            using (var reader = new System.IO.StreamReader(stream))
            {
                themefile = reader.ReadToEnd();
                map.MapStyle = MapStyle.FromJson(themefile);
            }
        }

        public async Task LocalizacionActual()
        {
            try
            {
                var location = await Geolocation.GetLastKnownLocationAsync();
                if (location == null)
                {
                    location = await Geolocation.GetLocationAsync(new GeolocationRequest
                    {
                        DesiredAccuracy = GeolocationAccuracy.High,
                        Timeout = TimeSpan.FromSeconds(30)
                    });


                }
                if (location == null)
                {
                    await DisplayAlert("Alerta", "Sin acceso al GPS", "OK");
                }
                else
                {
                    latitud = location.Latitude;
                    longitud = location.Longitude;
                    var posicion = new Position(latitud, longitud);
                    punto.Position = new Position(latitud, longitud);
                    map.MoveToRegion(MapSpan.FromCenterAndRadius(posicion, Distance.FromMeters(500)));
                    //await ObtenerCiudadPais();
                }

            }
            catch (Exception)
            {
                await DisplayAlert("Alerta", "Sin acceso al GPS", "OK");
            }
        }
        private async void map_PinDragEnd(object sender, PinDragEventArgs e)
        {
            var positions = new Position(e.Pin.Position.Latitude, e.Pin.Position.Longitude);
            map.MoveToRegion(MapSpan.FromCenterAndRadius(positions, Distance.FromMeters(500)));
            latitud = e.Pin.Position.Latitude;
            longitud = e.Pin.Position.Longitude;
            VMconfclientes.Geolocalizacion = latitud + ";" + longitud;
        }

        private async void btnconfirma_Clicked(object sender, EventArgs e)
        {
            if (latitud != 0 && longitud != 0)
            {
                await Navigation.PopAsync();
            }
            else
            {
              await  DisplayAlert("Sin datos", "Ubique un punto", "OK");
            }

        }
    }
}