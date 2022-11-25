using EcobankRepartidor.Conexiones;
using EcobankRepartidor.Modelo;
using Firebase.Database.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcobankRepartidor.Datos
{
    public class Dasignaciones
    {
        public async Task<List<Masignaciones>> MostrarAsignaciones(Masignaciones parametrospedir)
        {

            return (await Constantes.firebase
                 .Child("Asignaciones")
                 .OrderByKey()
                 .OnceAsync<Masignaciones>()).Where(a => a.Object.idrecolector == parametrospedir.idrecolector).Select(item => new Masignaciones
                 {
                     idsolicitud = item.Object.idsolicitud
                 }).ToList();

        }
        public async Task<List<Msolicitudesrecojo>> MostrarclientesAsignados(Masignaciones parametrosPedir)
        {
            var Listasolicitudes = new List<Msolicitudesrecojo>();
            var funcionasignaciones = new Dasignaciones();
            var parametrosasignaciones = new Masignaciones();
            parametrosasignaciones.idrecolector = parametrosPedir.idrecolector;
            var listaasignaciones = await funcionasignaciones.MostrarAsignaciones(parametrosasignaciones);
            foreach (var itemasig in listaasignaciones)
            {
                var funcion = new Dsolicitudesrecojo();
                var parametros = new Msolicitudesrecojo();
                parametros.Idsolicitud = itemasig.idsolicitud;
                var listasolicitudes = await funcion.MostrarsolicitRecojo(parametros);
                string Nombrecliente = "";
                string Direccion = "";
                string Fecha = "";
                string Geolocalizacion = "";
                string Turno = "";
                string Idturno = "";
                string Estado = "";

                var fclientes = new Dclientes();
                var pclientes = new Mclientes();

                //en evaluacion
                Estado = listasolicitudes[0].Estado;
                Fecha = listasolicitudes[0].Fecha;
                Idturno = listasolicitudes[0].Idturno;
                pclientes.Idcliente = listasolicitudes[0].Idcliente;
                //
                var fturno = new Dturnosrecojos();
                var pturno = new Mturnosrecojo();
                pturno.Idturno = Idturno;
                var listaturnos = await fturno.Mostrarturnosrecojo(pturno);
                //en Ev
                Turno = listaturnos[0].Turno;
                //
                var listaclientes = await fclientes.MostrarclientesXid(pclientes);
                //en eva
                Geolocalizacion = listaclientes[0].Geo;
                Nombrecliente = listaclientes[0].NombresApe;
                Direccion = listaclientes[0].Direccion;
                //
                parametros.Estado = Estado;
                parametros.Nombrecliente = Nombrecliente;
                parametros.Direccion = Direccion;
                parametros.Fecha = Fecha;
                Console.WriteLine(pclientes.Idcliente);
                parametros.Geolocalizacion = Geolocalizacion;
                parametros.Turno = Turno;
                parametros.Idcliente = pclientes.Idcliente;
                Listasolicitudes.Add(parametros);
            }
            return Listasolicitudes;

        }
        public async Task Eliminarasignacion(Masignaciones parametros)
        {
            var dataEliminar = (await Constantes.firebase
                .Child("Asignaciones")
                .OnceAsync<Masignaciones>())
                .Where(a => a.Object.idsolicitud == parametros.idsolicitud)
                .FirstOrDefault();
            await Constantes.firebase
                .Child("Asignaciones")
                .Child(dataEliminar.Key)
                .DeleteAsync();
        }

    }
}
