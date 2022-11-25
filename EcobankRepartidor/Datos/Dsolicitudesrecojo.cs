using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Firebase.Database.Query;
using System.Linq;
using EcobankRepartidor.Modelo;
using EcobankRepartidor.Conexiones;
using EcobankRepartidor.VistaModelo;

namespace EcobankRepartidor.Datos
{
    public class Dsolicitudesrecojo
    {
        public async Task<List<Msolicitudesrecojo>> MostrarsolicitRecojo(Msolicitudesrecojo parametrospedir)
        {
            return (await Constantes.firebase
              .Child("Solicitudesrecojo")
              .OnceAsync<Msolicitudesrecojo>()).Where(a => a.Object.Estado == "Asignado").Where(b => b.Key == parametrospedir.Idsolicitud).Select(item => new Msolicitudesrecojo
              {
                  Idcliente = item.Object.Idcliente,
                  Fecha=item.Object.Fecha,
                  Estado=item.Object.Estado,
                  Idturno=item.Object.Idturno
              }
                ).ToList();



        }
        public async Task FinalizarSolicitud(Msolicitudesrecojo parametrospedir)
        {
            var data = (await Constantes.firebase
              .Child("Solicitudesrecojo")
              .OnceAsync<Msolicitudesrecojo>()).Where(a => a.Object.Idcliente == parametrospedir.Idcliente).FirstOrDefault();

            data.Object.Estado = "Finalizado";
            await Constantes.firebase
              .Child("Solicitudesrecojo")
              .Child(data.Key)
              .PutAsync(data.Object);


        }
        public async Task Eliminarsolicitud(Msolicitudesrecojo parametros)
        {
            var dataEliminar = (await Constantes.firebase
                .Child("Solicitudesrecojo")
                .OnceAsync<Msolicitudesrecojo>())
                .Where(a => a.Key == parametros.Idsolicitud)
                .FirstOrDefault();
            await Constantes.firebase.Child("Solicitudesrecojo")
                .Child(dataEliminar.Key).DeleteAsync();
        }
    }
}
