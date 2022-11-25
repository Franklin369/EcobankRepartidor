using EcobankRepartidor.Conexiones;
using EcobankRepartidor.Datos;
using EcobankRepartidor.Modelo;
using EcobankRepartidor.Vistas;
using EcobankRepartidor.Vistas.Compras;
using EcobankRepartidor.Vistas.Confi;
using Firebase.Auth;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace EcobankRepartidor.VistaModelo
{
    public class VMmenuprincipal : BaseViewModel

    {
        #region variables
        public List<Msolicitudesrecojo> listViewSource;
        bool isBusy = false;
        bool sinsolicitudes;
        bool consolicitudes;
        string idrecolector;
        string nombre;
        string contadorAsignaciones;
        #endregion

        #region constructor
        public VMmenuprincipal(INavigation navigation)
        {
            Navigation = navigation;
            DependencyService.Get<VMstatusbar>().TransparentarStatusbar();
            obtenerdatosRecolector();
        }
        #endregion

        #region procesos
        private async Task obtenerdatosRecolector()
        {
            try
            {
                var authProvider = new FirebaseAuthProvider(new FirebaseConfig(Constantes.WebapyFirebase));
                var savedfirebaseauth = JsonConvert.DeserializeObject<Firebase.Auth.FirebaseAuth>(Preferences.Get("MyFirebaseRefreshToken", ""));
                var RefreshedContent = await authProvider.RefreshAuthAsync(savedfirebaseauth);
                Preferences.Set("MyFirebaseRefreshToken", JsonConvert.SerializeObject(RefreshedContent));
                string correo = savedfirebaseauth.User.Email;
                var funcion = new Drecolectores();
                var parametros = new Mrecolector();
                parametros.Correo = correo;
                var data = await funcion.MostrarRecolectorXcorreo(parametros);
                var contador = data.Count;
                var contador2 = data.Count;

                Idrecolector = data[0].Idrecolector;
                txtNombre = data[0].Nombre;
                await ContarAsignaciones();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await App.Current.MainPage.DisplayAlert("Alerta", "Oh no !  sesion expirada", "OK");
            }
        }
        private async Task Ircomprar()
        {
            await Navigation.PushAsync(new RegCompras());
        }
        private async Task IrClientes()
        {
            await Navigation.PushAsync(new ConfClientes());
        }
        private async Task Irmapear()
        {
            if (txtContadorAsig != "0")
            {
                Mapear.IdRecolector = Idrecolector;
                await Navigation.PushAsync(new Mapear());
            }
            else
            {
                await DisplayAlert("Sin asignaciones", "No tienes clientes asignados", "OK");
            }

        }
        public async Task ContarAsignaciones()
        {

            var funcion = new Dasignaciones();
            var parametros = new Masignaciones();
            parametros.idrecolector = Idrecolector;
            Listasolicitudes = await funcion.MostrarclientesAsignados(parametros);
            txtContadorAsig = Listasolicitudes.Count.ToString();
        }
        #endregion
        #region objetos
        public Mrecolector Recolector { get; set; }
        public List<Msolicitudesrecojo> Listasolicitudes
        {
            get { return this.listViewSource; }
            set
            {
                SetValue(ref this.listViewSource, value);
            }
        }
        public bool Sinsolicitudes
        {
            get { return this.sinsolicitudes; }
            set
            {
                SetValue(ref this.sinsolicitudes, value);
            }
        }
        public bool Consolicitudes
        {
            get { return this.consolicitudes; }
            set
            {
                SetValue(ref this.consolicitudes, value);
            }
        }
        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                SetValue(ref sinsolicitudes, value);
            }
        }
        public string txtNombre
        {
            get { return this.nombre; }
            set { SetValue(ref this.nombre, value); }
        }
        public string txtContadorAsig
        {
            get { return this.contadorAsignaciones; }
            set { SetValue(ref this.contadorAsignaciones, value); }
        }
        public string Idrecolector
        {
            get { return this.idrecolector; }
            set { SetValue(ref this.idrecolector, value); }
        }
        #endregion

        public ICommand Comprarcommand => new Command(async () => await Ircomprar());
        public ICommand Afiliarcommand => new Command(async () => await IrClientes());
        public ICommand Mapearcommand => new Command(async () => await Irmapear());

    }
}
