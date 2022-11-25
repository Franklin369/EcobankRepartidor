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
    public class Ddetallecompra
    {
        List<Mdetallecompra> Catalogo = new List<Mdetallecompra>();
        public async Task InsertarDetallecompra(Mdetallecompra parametros)
        {


            await Constantes.firebase
                    .Child("Detallecompra")
                    .PostAsync(new Mdetallecompra()
                    {
                        Estado = parametros.Estado,
                        fecha = DateTime.Now.ToString(),
                        Ganancia = parametros.Ganancia,
                        Idcliente = parametros.Idcliente,
                        Idproducto = parametros.Idproducto,
                        Cantidad = parametros.Cantidad,
                        Preciocompra = parametros.Preciocompra,
                        PrecioVenta = parametros.PrecioVenta,
                        Total = parametros.Total,
                        Und = parametros.Und,
                        Puntos = parametros.Puntos
                    });
        }
        public async Task<List<Mdetallecompra>> MostrarDcompra(string Idcliente)
        {
            decimal total = 0;
            var data = (await Constantes.firebase
           .Child("Detallecompra")
           .OrderByKey()

           .OnceAsync<Mdetallecompra>()).Where(a => a.Object.Idcliente == Idcliente);

            foreach (var dino in data)
            {
                var parametros = new Mdetallecompra();

                parametros.Preciocompra = "Precio de compra por " + dino.Object.Und + " = S/." + dino.Object.Preciocompra;
                parametros.Cantidad = dino.Object.Cantidad;
                parametros.Total = "S/. " + dino.Object.Total;
                parametros.Und = dino.Object.Und;
                parametros.fecha = dino.Object.fecha;
                total += Convert.ToDecimal(dino.Object.Total);
                parametros.Suma = total;
                var funcionProductos = new VMproductos();
                var parametrosProduc = new Mproductos();
                parametrosProduc.Idproducto = dino.Object.Idproducto;
                var dt = await funcionProductos.MostrarProductoXid(parametrosProduc);
                foreach (var dtpro in dt)
                {
                    parametros.Producto = dtpro.Descripcion + " (" + parametros.Cantidad + " " + parametros.Und + ")";
                    parametros.ProducIcono = dtpro.Icono;
                    parametros.Color = dtpro.Color;
                }

                Catalogo.Add(parametros);
            }
            return Catalogo.ToList();

        }
        public async Task<List<Mdetallecompra>> SumarTotal(string Idcliente)
        {

            return (await Constantes.firebase
              .Child("Detallecompra")
              .OnceAsync<Mdetallecompra>()).Where(a => a.Object.Idcliente == Idcliente).Select(item => new Mdetallecompra
              {
                  Total = item.Object.Total,
              }).ToList();
        }
        public async Task ConfirmarDC(Msolicitudesrecojo parametrospedir)
        {
            var data = (await Constantes.firebase
              .Child("Detallecompra")
              .OnceAsync<Msolicitudesrecojo>()).Where(a => a.Object.Idcliente == parametrospedir.Idcliente).Where(b => b.Object.Estado == "SIN CONFIRMAR");
            foreach (var item in data)
            {
                item.Object.Estado = "PAGADO";
                await Constantes.firebase
             .Child("Solicitudesrecojo")
             .Child(item.Key)
             .PutAsync(item.Object);
            }
          


        }
       
        public async Task EliDcompraSinconfirmar(Mdetallecompra parametros)
        {
            var dataEliminar = (await Constantes.firebase
                .Child("Detallecompra")
                .OnceAsync<Msolicitudesrecojo>())
                .Where(a => a.Object.Idcliente == parametros.Idcliente)
                .Where(b => b.Object.Estado == "SIN CONFIRMAR")
                .FirstOrDefault();
            await Constantes.firebase.Child("Detallecompra").Child(dataEliminar.Key).DeleteAsync();
        }
    }
}
