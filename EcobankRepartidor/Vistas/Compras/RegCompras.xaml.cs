using EcobankRepartidor.Modelo;
using EcobankRepartidor.VistaModelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.ComponentModel;

namespace EcobankRepartidor.Vistas.Compras
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [DesignTimeVisible(false)]

    public partial class RegCompras : ContentPage
    {
        List<Mproductos> dt = new List<Mproductos>();
        string imagen;
        public RegCompras()
        {
            InitializeComponent();
            //DependencyService.Get<VMstatusbar>().TransparentarStatusbar();

            //Mostrarproductos();
        }


        protected override void OnAppearing()
        {
            BindingContext = new VMregCompras(Navigation);
            //await EfectoAgregarProducto(imagen);
        }
        private void btnguardar_Clicked(object sender, EventArgs e)
        {

        }

        async Task Mostrarproductos()
        {
            //var funcion = new VMproductos();
            //dt = await funcion.MostrarProductos();
            //ListaProductos.ItemsSource = dt;
        }

        private async void btnagregar_Clicked(object sender, EventArgs e)
        {
            //imagen = ((sender as Button).CommandParameter).ToString();

            //await EfectoAgregarProducto(imagen);
            //await PopupNavigation.Instance.PushAsync(new Pagregarcompra());
            //await  Navigation.PushAsync(new Pagregarcompra());
        }
        private async Task EfectoAgregarProducto(string imagen)
        {
            var caja = new ImageButton();
            caja.Source = imagen;
            caja.BackgroundColor = Color.Transparent;
            caja.WidthRequest = 60;
            caja.HeightRequest = 60;
            //caja.VerticalOptions = LayoutOptions.Center;
            //caja.HorizontalOptions = LayoutOptions.Center;
            caja.CornerRadius = 30;
            caja.Scale = 0;

            gridprincipal.Children.Add(caja);
            await caja.ScaleTo(1, 200, Easing.SpringIn);
            await Task.WhenAll(
                caja.TranslateTo(0, PanelcontadorP.Y, 300),
                caja.ScaleTo(0, 500, Easing.CubicIn)
                );
            gridprincipal.Children.Remove(caja);
        }
        private async Task MostrarPanelDv()
        {

            uint duracion = 700;
            await Task.WhenAll(
                //PanelEncabezado.FadeTo(0, 500),
                //Panelcontador.FadeTo(0, 500),
                gridprincipal.TranslateTo(0, -800, duracion, Easing.CubicIn),
                PanelDetalleventa.TranslateTo(0, 0, duracion, Easing.CubicIn)
                );
            PanelDetalleventa.IsVisible = true;

        }
        private async Task OcultarPanelDv()
        {

            PanelDetalleventa.IsVisible = false;
            uint duracion = 700;
            await Task.WhenAll(
                //Panelcontador.FadeTo(1, 500),
                gridprincipal.TranslateTo(0, 0, duracion, Easing.CubicIn),
                PanelDetalleventa.TranslateTo(0, 0, duracion, Easing.CubicIn)

                );

        }
        private async void DeslizarMostrarPanelDV(object sender, SwipedEventArgs e)
        {
            await MostrarPanelDv();
        }

        private async void DeslizarOcultarPanelDV(object sender, SwipedEventArgs e)
        {
            await OcultarPanelDv();
        }

        private void btnEnviar_Clicked(object sender, EventArgs e)
        {

        }

        private void CarouselPositionChanged(object sender, PositionChangedEventArgs e)
        {
            //var carousel = sender as CarouselView;
            //var views = carousel.VisibleViews;
            ////color = ((CarouselView)sender).AutomationId.ToString();
            //if (views.Count > 0)
            //{
            //    foreach (var view in views)
            //    {

            //        var img = view.FindByName<Image>("MenuImg");
            //        var grid = view.FindByName<Grid>("Gridcontenedor");
            //        var framegeneral = view.FindByName<Frame>("FrameGeneral");
            //        var gradiente1 = view.FindByName<GradientStop>("Gra1");

            //        //gradiente1.Color = Color.FromHex(grid.AutomationId);
            //        //framegeneral.ScaleTo(0.1, 250);
            //        //grid.BackgroundColor = Color.FromHex(grid.AutomationId);
            //        ViewExtensions.CancelAnimations(img);
            //        Task.Run(async () => await img.RelRotateTo(360, 5000, Easing.BounceOut));
            //    }
            //}
        }


    }
}
