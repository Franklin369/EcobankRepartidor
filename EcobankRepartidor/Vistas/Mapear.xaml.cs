using EcobankRepartidor.Datos;
using EcobankRepartidor.Modelo;
using EcobankRepartidor.VistaModelo;
using EcobankRepartidor.Vistas.Compras;
using Plugin.ExternalMaps;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using Xamarin.Forms.Xaml;

namespace EcobankRepartidor.Vistas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Mapear : ContentPage
    {
        Pin punto = new Pin();
        public static double latitud = 0;
        public static double longitud = 0;
        public static string IdRecolector;
        bool estadomostrar = true;
        string idcliente;
        string nombre;
        string direccion;
        string foto;
        string dni;
        string idsolicitud;
        public Mapear()
        {
            InitializeComponent();
        }
        protected override async void OnAppearing()
        {
            //if (estadomostrar == true)
            //{
            //    punto = new Pin()
            //    {
            //        Label = "Tu ubicación",
            //        Type = PinType.Place,
            //        Icon = (Device.RuntimePlatform == Device.Android) ? BitmapDescriptorFactory.FromBundle("camion.png") : BitmapDescriptorFactory.FromView(new Image() { Source = "camion.png", WidthRequest = 64, HeightRequest = 64 }),
            //        Position = new Position(0, 0),

            //        IsDraggable = true
            //    };
            //    map.Pins.Add(punto);
            //    await LocalizacionActual();
            //    await mapearSolicitudes();
            //}
            punto = new Pin()
            {
                Label = "Tu ubicación",
                Type = PinType.Place,
                Icon = (Device.RuntimePlatform == Device.Android) ? BitmapDescriptorFactory.FromBundle("camion.png") : BitmapDescriptorFactory.FromView(new Image() { Source = "camion.png", WidthRequest = 64, HeightRequest = 64 }),
                Position = new Position(0, 0),
                IsDraggable = true
            };
            map.Pins.Add(punto);
            await LocalizacionActual();
            await MapearSolicitudes();
        }
        private async Task MapearSolicitudes()
        {
            var funcion = new Dasignaciones();
            var parametros = new Masignaciones();
            parametros.idrecolector = IdRecolector;
            var Listasolicitudes = await funcion.MostrarclientesAsignados(parametros);
            foreach (var data in Listasolicitudes)
            {
                string coordenadas = data.Geolocalizacion;
                string label = data.Direccion + "|" + data.Turno + "|" + data.Idcliente + "|" + data.Idsolicitud;
                string[] separadas = coordenadas.Split(',');
                double latitud = Convert.ToDouble(separadas[0]);
                double longitud = Convert.ToDouble(separadas[1]);
                punto = new Pin()
                {
                    Label = label,
                    Type = PinType.Place,
                    Icon = (Device.RuntimePlatform == Device.Android) ? BitmapDescriptorFactory.FromBundle("pin1.png") : BitmapDescriptorFactory.FromView(new Image() { Source = "pin1.png", WidthRequest = 64, HeightRequest = 64 }),
                    Position = new Position(latitud, longitud),
                    IsDraggable = true
                };
                map.Pins.Add(punto);
            }
            estadomostrar = false;
        }
        //private async Task Geolocalizarclientes()
        //{

        //    punto = new Pin()
        //    {
        //        Label = "72705474/Pio",
        //        Type = PinType.Place,
        //        Icon = (Device.RuntimePlatform == Device.Android) ? BitmapDescriptorFactory.FromBundle("pin1.png") : BitmapDescriptorFactory.FromView(new Image() { Source = "pin1.png", WidthRequest = 64, HeightRequest = 64 }),
        //        Position = new Position(-14.084424, -75.728024),
        //        IsDraggable = true
        //    };
        //    map.Pins.Add(punto);
        //    punto = new Pin()
        //    {
        //        Label = "73587700/Franklin",
        //        Type = PinType.Place,
        //        Icon = (Device.RuntimePlatform == Device.Android) ? BitmapDescriptorFactory.FromBundle("pin1.png") : BitmapDescriptorFactory.FromView(new Image() { Source = "pin1.png", WidthRequest = 64, HeightRequest = 64 }),
        //        Position = new Position(-14.096042, -75.734992),
        //        IsDraggable = true
        //    };
        //    map.Pins.Add(punto);
        //}
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
                }

            }
            catch (Exception)
            {
                await DisplayAlert("Alerta", "Sin acceso al GPS", "OK");
            }
        }

        private async void btnconfirma_Clicked(object sender, EventArgs e)
        {
            try
            {
                string titulo = map.SelectedPin.Label.ToString();
                string[] separadas = titulo.Split('|');
                idcliente = separadas[2];
                string cx = map.SelectedPin.Position.Latitude.ToString();
                string cy = map.SelectedPin.Position.Longitude.ToString();
                var proceso = await CrossExternalMaps.Current.
                NavigateTo(titulo, Convert.ToDouble(cx), Convert.ToDouble(cy));
            }
            catch (Exception)
            {

                await DisplayAlert("Sin coordenadas", "Seleccione un destino", "OK");
            }
        }

        private async void btnver_Clicked(object sender, EventArgs e)
        {
            try
            {
                string titulo = map.SelectedPin.Label.ToString();
                string[] separadas = titulo.Split('|');

                idcliente = separadas[2];
                var funcion = new Dclientes();
                var parametros = new Mclientes();
                parametros.Idcliente = idcliente;
                var lista = await funcion.MostrarclientesXid(parametros);
                foreach (var data in lista)
                {
                    nombre = data.NombresApe;
                    direccion = data.Direccion;
                    dni = data.Identificacion;
                    foto = data.FotoFachada;
                    break;
                }

                Vercliente.nombre = nombre;
                Vercliente.direccion = direccion;
                Vercliente.dni = dni;
                Vercliente.foto = foto;
                await PopupNavigation.Instance.PushAsync(new Vercliente());
            }
            catch (Exception)
            {
                await DisplayAlert("No permitido", "Accion denegada", "OK");
            }

            //await Navigation.PushAsync(new Vercliente());
        }

        private async void btnComprar_Clicked(object sender, EventArgs e)
        {
            try
            {
                //string titulo = map.SelectedPin.Label.ToString();
                //string[] separadas = titulo.Split('|');

                //idcliente = separadas[2];
                //idsolicitud = separadas[3];
                idsolicitud = "-MicBCG0SulfHmtw47yZ";
                idcliente = "-MdUOsGKstWLGK5xPRo-";
                await EliminarcomprasIncomp();
                VMregCompras.IdsolicitudReco = idsolicitud;
                VMregCompras.idcliente = idcliente;
                VMregCompras.activadorProductos = true;
                VMregCompras.Idrecolector = IdRecolector;
                await Navigation.PushAsync(new RegCompras());
            }
            catch (Exception)
            {

                await DisplayAlert("No permitido", "Accion denegada", "OK");
            }


        }
        private async Task EliminarcomprasIncomp()
        {
            var funcion = new Ddetallecompra();
            var parametros = new Mdetallecompra();
            parametros.Idcliente = idcliente;
            await funcion.EliDcompraSinconfirmar(parametros);
        }
        private void btnvolver_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}