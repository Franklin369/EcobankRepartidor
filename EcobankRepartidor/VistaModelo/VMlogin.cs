using EcobankRepartidor.Modelo;
using Plugin.SharedTransitions;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Essentials;
using Firebase.Auth;
using EcobankRepartidor.Vistas.Compras;
using EcobankRepartidor.Conexiones;
using Newtonsoft.Json;
using Acr.UserDialogs;
using EcobankRepartidor.Vistas;
using EcobankRepartidor.Datos;

namespace EcobankRepartidor.VistaModelo
{
    public class VMlogin : BaseViewModel
    {

        private string Correo;
        private string Pass;
        private string nombre;
        private string idrecolector;
        bool estado;
        public bool visibleInicio = true;
        public bool visiblefinal = false;
        public bool sininternetV = false;

        public VMlogin()
        {
            DependencyService.Get<VMstatusbar>().TransparentarStatusbar();
            ValidarConexInternet();
            IniciarSesioncommand = new Command(async (f) => await EjecutarIniciarSesion());
        }
        private async Task EjecutarIniciarSesion()
        {

            await IniciarSesion();
            await Ingresar();
        }
        private async Task Ingresar()
        {
            if (estado == true)
            {
                //await Obtenerdatos(recolector);
                Application.Current.MainPage = new SharedTransitionNavigationPage(new Menuprincipal());
                UserDialogs.Instance.HideLoading();

            }
        }
       
        public Command IniciarSesioncommand { get; set; }
        private async Task<bool> IniciarSesion()
        {
            try
            {
                UserDialogs.Instance.ShowLoading("Validando datos...");
                var authProvider = new FirebaseAuthProvider(new FirebaseConfig(Constantes.WebapyFirebase));
                var auth = await authProvider.SignInWithEmailAndPasswordAsync(txtCorreo, txtPass);
                var serializartoken = JsonConvert.SerializeObject(auth);
                Preferences.Set("MyFirebaseRefreshToken", serializartoken);
                return estado = true;

            }
            catch (Exception)
            {
                UserDialogs.Instance.HideLoading();
                await App.Current.MainPage.DisplayAlert("Error", "Datos incorrectos", "OK");
                return estado = false;
            }
        }
        #region Propertiers
        public string txtCorreo
        {
            get { return this.Correo; }
            set { SetValue(ref this.Correo, value); }
        }
        public string txtPass
        {
            get { return this.Pass; }
            set { SetValue(ref this.Pass, value); }
        }
        public string txtNombre
        {
            get { return this.nombre; }
            set { SetValue(ref this.nombre, value); }
        }
        public string Idrecolector
        {
            get { return this.idrecolector; }
            set { SetValue(ref this.idrecolector, value); }
        }

        #endregion


        #region verificarInternet
        public bool VisibleInicio
        {
            get { return this.visibleInicio; }
            set
            {
                SetValue(ref this.visibleInicio, value);
            }
        }
        public bool VisibleFinal
        {
            get { return this.visiblefinal; }
            set
            {
                SetValue(ref this.visiblefinal, value);
            }
        }
        public bool Sininternetv
        {
            get { return this.sininternetV; }
            set
            {
                SetValue(ref this.sininternetV, value);
            }
        }
        private void ValidarConexInternet()
        {
            VisibleFinal = false;
            Sininternetv = false;
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                VisibleInicio = false;
                Sininternetv = true;
            }
            else
            {
                VisibleInicio = true;
                Sininternetv = false;
            }
        }
        #endregion
    }
}
