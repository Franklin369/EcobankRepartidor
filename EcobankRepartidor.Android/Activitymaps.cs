#if DEBUG
using System;
using Android.App;
using Android.Runtime;
using Plugin.CurrentActivity;
using EcobankRepartidor.Conexiones;

[Application(Debuggable = true)]
#else
using Android.App;
using Android.Runtime;
using Bjornapp.VistasModelo;
using Plugin.CurrentActivity;
using System;

[Application(Debuggable = false)]
#endif
[MetaData("com.google.android.maps.v2.API_KEY",
			  Value = Constantes.GoogleMapsApiKey)]


public class Activitymaps:Application
    {
		public Activitymaps(IntPtr handle, JniHandleOwnership transer)
	  : base(handle, transer)
		{
		}

		public override void OnCreate()
		{
			base.OnCreate();
			CrossCurrentActivity.Current.Init(this);
		}
	}
