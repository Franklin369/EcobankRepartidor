using System;
using System.Collections.Generic;
using System.Text;

namespace EcobankRepartidor.Modelo
{
   public class Msolicitudesrecojo
    {
        public string Idsolicitud { get; set; }
        public string Fecha { get; set; }
        public string Idcliente { get; set; }
        public string Idturno { get; set; }
        public string Estado { get; set; }
        //
        public string Nombrecliente { get; set; }
        public string Direccion { get; set; }
        public string Geolocalizacion { get; set; }
        public string Turno { get; set; }
        public bool IsBusy { get; set; }
    }
}
