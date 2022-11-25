using EcobankRepartidor.Modelo;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace EcobankRepartidor.VistaModelo
{
    public class VMfoto:Mfotomodel
    {
        public VMfoto()
        {
            Capturarcommand = new Command(TomarFoto);
        }
        public Command Capturarcommand { get; set; }
        private async void TomarFoto()
        {
            var camara = new StoreCameraMediaOptions();
            camara.PhotoSize = PhotoSize.Medium;
            camara.SaveToAlbum = true;
            var foto = await Plugin.Media.CrossMedia.Current.TakePhotoAsync(camara);
            if(foto!=null)
            {
                Foto = ImageSource.FromStream(() => {
                    return foto.GetStream();
                      });
            }
        }
    }
}
