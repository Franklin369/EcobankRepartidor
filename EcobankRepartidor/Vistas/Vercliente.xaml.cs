using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Rg.Plugins.Popup.Services;
namespace EcobankRepartidor.Vistas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Vercliente : PopupPage
    {
        public static string nombre;
        public static string dni;
        public static string foto;
        public static string direccion;
        public  Vercliente()
        {
            InitializeComponent();
            Obtederdatos();
        }
        private void Obtederdatos()
        {
            lblnombre.Text = nombre;
            lbldireccion.Text = direccion;
            lbldni.Text = dni;
            fotofachada.Source = foto;
        }

        private async void btnvolver_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync(true);
        }
    }
}