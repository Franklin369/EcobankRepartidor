using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Firebase.Database.Query;
using System.Linq;
using EcobankRepartidor.Modelo;
using EcobankRepartidor.Conexiones;
using System.Collections.ObjectModel;
using Firebase.Database;

namespace EcobankRepartidor.VistaModelo
{
   public class VMproductos
    {
        FirebaseClient client;
        public VMproductos()
        {
            client = new FirebaseClient("https://ecob-3563a-default-rtdb.firebaseio.com/");
        }
        public async Task Insertarproductos(Mproductos parametros)
        {

            await Constantes.firebase
                .Child("Productos")
                .PostAsync(new Mproductos()
                {
                    Descripcion = parametros.Descripcion,
                    Preciocompra = parametros.Preciocompra,
                    Precioventa = parametros.Precioventa
                });
        }
        public async Task<List<Mproductos>> MostrarProductos2()
        {

            return (await client
              .Child("Productos")
              .OnceAsync<Mproductos>()).Where(a=>a.Key !="Modelo").Select(item => new Mproductos
              {
                  
                  Preciocompra=item.Object.Preciocompra,
                  PreciocompraString= "Precio de compra por"+ item.Object.Und +" = S/." + item.Object.Preciocompra,
                  Descripcion = item.Object.Descripcion,
                  Icono = item.Object.Icono,
                  Color=item.Object.Color,
                  Und=item.Object.Und
                  ,Idproducto=item.Key
                 
              }).ToList();
        }
        public async Task<List<Mproductos>> MostrarProductoXid(Mproductos parametrosPedir)
        {

            return (await client
              .Child("Productos")
              .OnceAsync<Mproductos>()).Where(a => a.Key == parametrosPedir.Idproducto).Select(item => new Mproductos
              {

                  Preciocompra = item.Object.Preciocompra,
                  PreciocompraString = "Precio de compra por" + item.Object.Und + " = S/." + item.Object.Preciocompra,
                  Descripcion = item.Object.Descripcion,
                  Icono = item.Object.Icono,
                  Color = item.Object.Color,
                  Und = item.Object.Und
                  ,
                  Idproducto = item.Key

              }).ToList();
        }
        public ObservableCollection<Mproductos> MostrarProductos()
        {
            var data = client
                .Child("Productos")
                .AsObservable<Mproductos>()
                .AsObservableCollection();
            return data;
            

             
            //var Movimientos = new List<Mproductos>();
            //var data = (await Constantes.firebase
            //    .Child("Productos")
            //    .OrderByKey()
            //    .OnceAsync<Mproductos>());
            //foreach (var dt in data)
            //{
            //    var parametros = new Mproductos();
            //    parametros.Descripcion = dt.Object.Descripcion;
            //    parametros.Preciocompra ="S/. "+ dt.Object.Preciocompra + " x Kg";
            //    parametros.Precioventa = dt.Object.Precioventa;
            //    parametros.Und = dt.Object.Und;
            //    parametros.Idproducto = dt.Key;
            //    parametros.Color = dt.Object.Color;
            //    parametros.Icono = dt.Object.Icono;
            //    Movimientos.Add(parametros);
            //}
            //return Movimientos;
        }

    }
}
