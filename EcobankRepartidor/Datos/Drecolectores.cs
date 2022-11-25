using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Firebase.Database.Query;
using System.Linq;
using EcobankRepartidor.Modelo;
using EcobankRepartidor.Conexiones;
using System.IO;
using Firebase.Storage;
using Xamarin.Forms;

namespace EcobankRepartidor.Datos
{
   public class Drecolectores
    {
        public async Task<List<Mrecolector>> MostrarRecolectorXcorreo(Mrecolector parametrosPedir)
        {

            return (await Constantes.firebase
              .Child("Recolectores")
              .OnceAsync<Mrecolector>()).Where(a => a.Object.Correo == parametrosPedir.Correo).Select(item => new Mrecolector
              {
                  Nombre = item.Object.Nombre,
                  Idrecolector=item.Key
              }).ToList();
        }
    }
}
