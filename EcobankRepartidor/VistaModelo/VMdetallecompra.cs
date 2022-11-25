using EcobankRepartidor.Datos;
using EcobankRepartidor.Modelo;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace EcobankRepartidor.VistaModelo
{
    public class VMdetallecompra : BaseViewModel
    {
        private string Total;
        private string Cantidad;
        private double Ganancia;
        private double Precioventa;
        public static string Idcliente;
        public static string Idrecolector;
        public static string IdsolicitudReco;
        public VMdetallecompra(INavigation navigation, Mproductos product)
        {
            DependencyService.Get<VMstatusbar>().TransparentarStatusbar();
            Navigation = navigation;
            //DependencyService.Get<IStatusBarStyle>().ChangeTextColor(true);
            PopDetailPageCommand = new Command(async () => await ExecutePopDetailPageCommand());
            Product = product;

        }

        public Command PopDetailPageCommand { get; }
        public Mproductos Product { get; set; }
        public Mclientes Client { get; set; }

        private async Task ExecutePopDetailPageCommand()
        {
            CalcularTotal();
            await InsertarDetallecompra();
            VMregCompras.activadorProductos = false;
            await Navigation.PopAsync();

        }
        async Task InsertarDetallecompra()
        {
            var funcion = new Ddetallecompra();
            var parametros = new Mdetallecompra();
            parametros.Ganancia = Ganancia.ToString();
            parametros.Idcliente = Idcliente;
            parametros.Idproducto = Product.Idproducto;
            parametros.Cantidad = Cantidadtxt;
            parametros.Preciocompra = Product.Preciocompra;
            parametros.PrecioVenta = Product.Precioventa;
            parametros.Total = Totaltxt;
            parametros.Und = Product.Und;
            parametros.Puntos = "-";
            parametros.Estado = "SIN CONFIRMAR";
            await funcion.InsertarDetallecompra(parametros);
        }
      
        #region Propertiers
        public string Cantidadtxt
        {
            get { return this.Cantidad; }
            set { SetValue(ref this.Cantidad, value); }
        }
        public string Totaltxt
        {
            get { return this.Total; }
            set { SetValue(ref this.Total, value); }
        }
        #endregion
        #region Commands
       
        public ICommand CalcularCommand => new Command(async () => await CalcularTotal());
        #endregion
        #region Metodos

        public async Task CalcularTotal()
        {
            if (!string.IsNullOrEmpty(Cantidadtxt))
            {
                double cant = Convert.ToDouble(Cantidadtxt);
                double preciocomp = Convert.ToDouble(Product.Preciocompra);
                Totaltxt = (cant * preciocomp).ToString();
                Ganancia = cant * Precioventa - cant * preciocomp;
            }
            else
            {
               await Application.Current.MainPage.DisplayAlert("Error", "Ingrese un valor", "OK");
            }
        }
        #endregion
    }
}

