using EcobankRepartidor.Modelo;
using EcobankRepartidor.VistaModelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EcobankRepartidor.Vistas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Menuprincipal : ContentPage
    {
        private VMmenuprincipal vm;
        public Menuprincipal()
        {
            InitializeComponent();
            vm = new VMmenuprincipal(Navigation);
            BindingContext = vm;
            this.Appearing += Listazonas_Appearing;
        }

        private async void Listazonas_Appearing(object sender, EventArgs e)
        {
           await  vm.ContarAsignaciones();
        }
    }
}