using EcobankRepartidor.Modelo;
using EcobankRepartidor.VistaModelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
namespace EcobankRepartidor.Vistas.Confi
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ConfClientes : ContentPage
    {
        public ConfClientes()
        {
            InitializeComponent();
            BindingContext =new VMconfclientes(Navigation);
        }
    }
}