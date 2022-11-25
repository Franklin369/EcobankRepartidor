using Acr.UserDialogs;
using EcobankRepartidor;
using EcobankRepartidor.Conexiones;
using EcobankRepartidor.Datos;
using EcobankRepartidor.Modelo;
using EcobankRepartidor.Vistas.Compras;
using Firebase.Database.Query;
using Plugin.SharedTransitions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace EcobankRepartidor.VistaModelo
{
    public class VMregCompras : BaseViewModel
    {

        private VMproductos services;
        public object listViewSource;
        public List<Mdetallecompra> listadetallecliente;

        private string total;
        private string Cantidad;
        private string Ganancia;
        private string Preciocompra;
        private string buscar;
        private object listaclientes;

        public bool productosvisible;
        public bool contadorvisible;
        public bool listaclientesvisible;
        public static string idcliente;
        private string idclienteReceptor;
        private bool activadorTotal;
        public static bool activadorProductos;
        public static string Idrecolector;
        public static string IdsolicitudReco;

        public VMregCompras(INavigation navigation)
        {
            DependencyService.Get<VMstatusbar>().TransparentarStatusbar();
            services = new VMproductos();
            Navigation = navigation;
            NavigateToDetailPageCommand = new Command<Mproductos>(async (param) => await ExeccuteNavigateToDetailPageCommand(param));
            AgregarDetallecompraCommand = new Command<Mdetallecompra>(async (Mdetallecompra) => await agregarDetallecompra(Mdetallecompra));
            GuardarCommand = new Command(async () => await EliSolicitudAsignacion());

            //SeleccionarclienteCommand = new Command<Mclientes>(async (Mclientes) => await EjecutarSelecCliente());  
            idclienteReceptor = idcliente;
            Idcliente = idclienteReceptor;
            ActivadorProductos = activadorProductos;
            Mostrarproductos();
            MostrarDetallecompra();
            Sumartotal();
        }
        async Task EliSolicitudAsignacion()
        {
            try
            {
                var funcion = new Dsolicitudesrecojo();
                var parametros = new Msolicitudesrecojo();
                parametros.Idsolicitud = IdsolicitudReco;
                await funcion.Eliminarsolicitud(parametros);
                //*
                var funcionA = new Dasignaciones();
                var parametrosA = new Masignaciones();
                parametrosA.idsolicitud = IdsolicitudReco;
                await funcionA.Eliminarasignacion(parametrosA);
                await Navigation.PopAsync();
            }
            catch (Exception ex)
            {

                await DisplayAlert("s", ex.Message, "s");
            }


        }
        private async Task MostrarDetallecompra()
        {

            var funcion = new Ddetallecompra();
            Listadetallecompra = await funcion.MostrarDcompra(Idcliente);

        }
        private async Task Sumartotal()
        {
            var funcion = new Ddetallecompra();
            Listadetallecompra = await funcion.SumarTotal(Idcliente);
            decimal total = 0;
            //var dt = await funcion.MostrarDcompra(idcliente);
            foreach (var hobit in Listadetallecompra)
            {
                total += Convert.ToDecimal(hobit.Total);
            }
            lblTotal = total.ToString();
        }
        public bool Productosvisible
        {
            get { return this.productosvisible; }
            set
            {
                SetValue(ref this.productosvisible, value);
            }
        }
        public bool Contadorvisible
        {
            get { return this.contadorvisible; }
            set
            {
                SetValue(ref this.contadorvisible, value);
            }
        }
        public bool Listaclientesvisible
        {
            get { return this.listaclientesvisible; }
            set
            {
                SetValue(ref this.listaclientesvisible, value);
            }
        }
        private async Task EjecutarSelecCliente()
        {
            //idcliente = product.Idcliente;
            await Mostrarproductos();
            Productosvisible = true;
            Contadorvisible = true;
            Listaclientesvisible = true;
            await MostrarDetallecompra();
        }


        public string SearchedText
        {
            get
            {
                return buscar;
            }
            set
            {
                buscar = value;
                OnPropertyChanged("SearchedText");
                SearchCommandExecute();
            }
        }
        private async void SearchCommandExecute()
        {

            if (SearchedText.Length == 8)
            {
                UserDialogs.Instance.ShowLoading("Validando datos...");
                var funcion = new Dclientes();
                var parametros = new Mclientes();
                parametros.Identificacion = SearchedText;
                Listaclientes = await funcion.ValidarCliente(parametros);
                Listaclientesvisible = true;
                UserDialogs.Instance.HideLoading();
            }
            else if (SearchedText.Length == 0)
            {
                Productosvisible = false;
                Contadorvisible = false;
                Listaclientesvisible = false;
            }

        }

        #region Propertiers
        public string Idcliente
        {
            get { return this.idclienteReceptor; }
            set { SetValue(ref this.idclienteReceptor, value); }
        }
        public bool ActivadorProductos
        {
            get { return this.activadorTotal; }
            set { SetValue(ref this.activadorTotal, value); }
        }
        public string Cantidadtxt
        {
            get { return this.Cantidad; }
            set { SetValue(ref this.Cantidad, value); }
        }
        public string lblTotal
        {
            get { return this.total; }
            set { SetValue(ref this.total, value); }
        }
        #endregion
        #region Commands
      
      
        #endregion
        #region Metodos

       
        #endregion
        public async Task agregarDetallecompra(Mdetallecompra parametros)
        {
            var funcion = new Ddetallecompra();
            await funcion.InsertarDetallecompra(parametros);
        }
        public Command GuardarCommand { get; }
        public Command NavigateToDetailPageCommand { get; }
        public Command SelectCategoryCommand { get; }
        public Command AgregarDetallecompraCommand { get; }
        public Command SeleccionarclienteCommand { get; }
        //public ObservableCollection<Category> Categories { get; set; }

        private async Task ExeccuteNavigateToDetailPageCommand(Mproductos product)
        {
            var page = (App.Current.MainPage as SharedTransitionNavigationPage).CurrentPage;
            SharedTransitionNavigationPage.SetTransitionSelectedGroup(page, product.Idproducto);
            VMdetallecompra.Idcliente = Idcliente;
            VMdetallecompra.Idrecolector = Idrecolector;
            VMdetallecompra.IdsolicitudReco = IdsolicitudReco;
            await Navigation.PushAsync(new Pagregarcompra(product));
            //await DisplayAlert("s", "s", "s");
            //await MostrarDetallecompra();

        }
        private ObservableCollection<Mproductos> funcionProductos = new ObservableCollection<Mproductos>();
        public ObservableCollection<Mproductos> Productos
        {
            get { return funcionProductos; }
            set
            {
                funcionProductos = value;
                OnPropertyChanged();
            }

        }
        public async Task Mostrarproductos()
        {
            Productosvisible = true;
            Productosvisible = true;
            Contadorvisible = true;
            Listaclientesvisible = true;

            var funcion = new VMproductos();
            this.ListViewSource = await funcion.MostrarProductos2();
            ActivadorProductos = false;
            activadorProductos = false;



        }
        public object ListViewSource
        {
            get { return this.listViewSource; }
            set
            {
                SetValue(ref this.listViewSource, value);
            }
        }
        public List<Mdetallecompra> Listadetallecompra
        {
            get { return this.listadetallecliente; }
            set
            {
                SetValue(ref this.listadetallecliente, value);
            }
        }
        public object Listaclientes
        {
            get { return this.listaclientes; }
            set
            {
                SetValue(ref this.listaclientes, value);
            }
        }

    }
}
