using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Firebase.Database.Query;
using System.Linq;
using EcobankRepartidor.Modelo;
using EcobankRepartidor.Conexiones;

namespace EcobankRepartidor.Datos
{
   public  class Dturnosrecojos
    {
        public async Task<List<Mturnosrecojo>> Mostrarturnosrecojo(Mturnosrecojo parametrosPedir)
        {

            return (await Constantes.firebase
              .Child("Turnosrecojo")
              .OnceAsync<Mturnosrecojo>()).Where(a=>a.Key==parametrosPedir.Idturno).Select(item => new Mturnosrecojo
              {
                  Idturno = item.Key,
                  Turno = item.Object.Turno
              }).ToList();

        }
    }
}
