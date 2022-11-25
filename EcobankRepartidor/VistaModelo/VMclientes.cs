using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Firebase.Database.Query;
using System.Linq;
using EcobankRepartidor.Modelo;
using EcobankRepartidor.Conexiones;

namespace EcobankRepartidor.VistaModelo
{
   public class VMclientes
    {
        public async Task Insertarclientes(Mclientes parametros)
        {

            await Constantes.firebase
                .Child("Clientes")
                .PostAsync(new Mclientes()
                {
                    Direccion = parametros.Direccion,
                    FotoFachada = parametros.FotoFachada,
                    Geo = parametros.Geo,
                    IdDepa = parametros.IdDepa,
                    IdDis = parametros.IdDis,
                    IdPais=parametros.IdPais,
                    IdPro=parametros.IdPro,
                    IdZona=parametros.IdZona,
                    Identificacion=parametros.Identificacion,
                    NombresApe=parametros.NombresApe
                }) ;
        }
       
    }
}
