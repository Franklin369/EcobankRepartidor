using Acr.UserDialogs;
using EcobankRepartidor.Datos;
using EcobankRepartidor.Modelo;
using EcobankRepartidor.Vistas;
using Firebase.Storage;
using Plugin.Media.Abstractions;
using Plugin.SharedTransitions;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace EcobankRepartidor.VistaModelo
{
    public class VMconfclientes : BaseViewModel
    {
        #region variables
        public List<Mubicaciones> listpais = new List<Mubicaciones>();
        public List<Mubicaciones> listdepa = new List<Mubicaciones>();
        public List<Mubicaciones> listprov = new List<Mubicaciones>();
        public List<Mubicaciones> listdist = new List<Mubicaciones>();
        public List<Mubicaciones> listzona = new List<Mubicaciones>();
        Mubicaciones selectPais;
        Mubicaciones selectDepa;
        Mubicaciones selectProv;
        Mubicaciones selectDist;
        Mubicaciones selectZona;
        public bool panelGeolocalizacion = false;
        public bool panelRegistro = true;
        public bool panelRegistrado = false;
        private string Idpais;
        private string Iddepa;
        private string Idprov;
        private string Iddist;
        private string Idzona;
        string rutafoto;
        MediaFile foto;
        public string Direccion;
        public string FotoFachada;
        public static string Geolocalizacion;

        public string IdDepa;
        public string IdDis;
        public string IdPais;
        public string IdPro;
        public string IdZona;
        public string Identificacion;
        public string NombresApe;
        #endregion

        #region Mostrardatos
        public async Task Mostrarpais()
        {
            var funcion = new Dubicaciones();
            this.Listpais = await funcion.Mostrarpais();
        }
        public async Task MostrarDepa()
        {
            var funcion = new Dubicaciones();
            this.ListDepa = await funcion.Mostrardepartamento();
        }
        public async Task MostrarProv()
        {
            var funcion = new Dubicaciones();
            this.ListProv = await funcion.MostrarProvincia();
        }
        public async Task MostrarDist()
        {
            var funcion = new Dubicaciones();
            this.ListDistr = await funcion.Mostrardistrito();
        }
        public async Task MostrarZonas()
        {
            var funcion = new Dubicaciones();
            this.ListZona = await funcion.MostrarZona();
        }
        #endregion

        #region Asignaciones
        public List<Mubicaciones> Listpais
        {
            get { return this.listpais; }
            set
            {
                SetValue(ref this.listpais, value);
            }
        }
        public List<Mubicaciones> ListDepa
        {
            get { return this.listdepa; }
            set
            {
                SetValue(ref this.listdepa, value);
            }
        }
        public List<Mubicaciones> ListProv
        {
            get { return this.listprov; }
            set
            {
                SetValue(ref this.listprov, value);
            }
        }
        public List<Mubicaciones> ListDistr
        {
            get { return this.listdist; }
            set
            {
                SetValue(ref this.listdist, value);
            }
        }
        public List<Mubicaciones> ListZona
        {
            get { return this.listzona; }
            set
            {
                SetValue(ref this.listzona, value);
            }
        }

        public bool PanelGeolocalizacion
        {
            get { return this.panelGeolocalizacion; }
            set
            {
                SetValue(ref this.panelGeolocalizacion, value);
            }
        }
        public bool Panelregistro
        {
            get { return this.panelRegistro; }
            set
            {
                SetValue(ref this.panelRegistro, value);
            }
        }
        public bool PanelRegistrado
        {
            get { return this.panelRegistrado; }
            set
            {
                SetValue(ref this.panelRegistrado, value);
            }
        }
        public string Direcciontxt
        {
            get
            {
                return Direccion;
            }
            set
            {
                SetProperty(ref Direccion, value);
            }
        }
        public string Identificaciontxt
        {
            get
            {
                return Identificacion;
            }
            set
            {
                SetProperty(ref Identificacion, value);
            }
        }
        public string NombresApellidtxt
        {
            get
            {
                return NombresApe;
            }
            set
            {
                SetProperty(ref NombresApe, value);
            }
        }
        //public static string Geolocalizaciontxt
        //{
        //    get
        //    {
        //        return Geolocalizacion;
        //    }
        //    set
        //    {
        //        SetProperty(ref Geolocalizacion, value);
        //    }
        //}
        #endregion

        #region Constructor
        public VMconfclientes(INavigation navigation)
        {

            Navigation = navigation;
            DependencyService.Get<VMstatusbar>().TransparentarStatusbar();
            VolverdeLocalizarcomman = new Command(async (param) => await VolverdeLocalizar());
            Volvercomman = new Command(async (param) => await Volver());
            NavegarPagLocalicommand = new Command(async (param) => await EjecutarNavLocali());
            MostrarpanelGeoCommand = new Command(EjecutarMostrarpanelGeo);
            MostrarpanelRegistrocommand = new Command(EjecutarMostrarpanelReg);
            Agregarclientecommand = new Command<Mclientes>(async (param) => await EjecutarInsertarCliente(param));
            Capturarcommand = new Command(TomarFoto);
            Panelregistro = true;
            PanelGeolocalizacion = false;
            PanelRegistrado = false;
            Mostrarpais();
            MostrarDepa();
            MostrarDist();
            MostrarProv();
            MostrarZonas();
        }
        #endregion

        private async Task Volver()
        {
            await Navigation.PopAsync();
        }
        private async Task VolverdeLocalizar()
        {
            await Navigation.PopAsync();
        }
        #region comandos
        public Command Capturarcommand { get; set; }
        public Command NavegarPagLocalicommand { get; }
        public Command MostrarpanelGeoCommand { get; }
        public Command MostrarpanelRegistrocommand { get; }

        public Command Volvercomman { get; }
        public Command VolverdeLocalizarcomman { get; }
        public Command Agregarclientecommand { get; }


        #endregion
        #region Ejecucion de comandos
        private async Task EjecutarInsertarCliente(Mclientes parametros)
        {
            await AgregarCliente(parametros);
        }

        private async Task EjecutarNavLocali()
        {
            await Navigation.PushAsync(new Vistas.Confi.Paglocalizar());
        }
        #endregion
        #region Crud
        public async Task AgregarCliente(Mclientes parametros)
        {
            UserDialogs.Instance.ShowLoading("Guardando datos...");
            await Subifotofachada();
            var funcion = new Dclientes();
            var parametrosS = new Mclientes();
            parametrosS.Direccion = Direcciontxt;
            parametrosS.FotoFachada = rutafoto;
            parametrosS.Geo = Geolocalizacion;
            parametrosS.IdDepa = Iddepa;
            parametrosS.IdDis = Iddist;
            parametrosS.IdPais = Idpais;
            parametrosS.IdPro = Idprov;
            parametrosS.IdZona = Idzona;
            parametrosS.Identificacion = Identificaciontxt;
            parametrosS.NombresApe = NombresApellidtxt;
            parametrosS.Kgacumulados = "0";
            parametrosS.Puntos = "0";
            parametrosS.Totalcobrado = "0";
            parametrosS.Totalporcobrar = "0";
            await funcion.Insertarclientes(parametrosS);
            UserDialogs.Instance.HideLoading();
            PanelRegistrado = true;
            PanelGeolocalizacion = false;
            Panelregistro = false;
            //await Application.Current.MainPage.DisplayAlert("Ok", "exito", "ok");
        }
        public async Task Subifotofachada()
        {
            var funcion = new Dclientes();
            rutafoto = await funcion.Subirfotofachada(foto.GetStream(), Identificaciontxt);
        }
        //public async Task<string> Subirfotofachada(Stream imageStream, string Identificacion)
        //{
        //    var stroageImage = await new FirebaseStorage("bjorn-8ab44.appspot.com")
        //        .Child("Negocios")
        //        .Child(Identificacion + ".jpg")
        //        .PutAsync(imageStream);
        //         rutafoto = stroageImage;
        //    return rutafoto;

        //}
        #endregion


        #region selectores
        public Mubicaciones SelectPais
        {
            get { return selectPais; }
            set
            {
                SetProperty(ref selectPais, value);
                Idpais = selectPais.Idpais;

            }
        }
        public Mubicaciones SelectDepa
        {
            get { return selectDepa; }
            set
            {
                SetProperty(ref selectDepa, value);
                Iddepa = selectDepa.Iddepartamento;

            }
        }
        public Mubicaciones SelectProv
        {
            get { return selectProv; }
            set
            {
                SetProperty(ref selectProv, value);
                Idprov = selectProv.Idprovincia;

            }
        }
        public Mubicaciones SelectDist
        {
            get { return selectDist; }
            set
            {
                SetProperty(ref selectDist, value);
                Iddist = selectDist.Iddistrito;

            }
        }
        public Mubicaciones SelectZona
        {
            get { return selectZona; }
            set
            {
                SetProperty(ref selectZona, value);
                Idzona = selectZona.Idzona;

            }
        }
        #endregion
        #region procesos
        private async void  TomarFoto()
        {
            var camara = new StoreCameraMediaOptions();
            camara.PhotoSize = PhotoSize.Medium;
            camara.SaveToAlbum = true;
            foto = await Plugin.Media.CrossMedia.Current.TakePhotoAsync(camara);
            if (foto != null)
            {
                //Foto.GetStream();
                Foto = ImageSource.FromStream(() =>
                {
                    return foto.GetStream();
                });
            }
            
        }
        private void EjecutarMostrarpanelGeo()
        {
            PanelGeolocalizacion = true;
            Panelregistro = false;
        }
        private void EjecutarMostrarpanelReg()
        {
            PanelGeolocalizacion = false;
            Panelregistro = true;
        }
        #endregion
    }
}
