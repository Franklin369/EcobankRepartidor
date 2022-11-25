using Firebase.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace EcobankRepartidor.Conexiones
{
   public class Constantes
    {
        public static FirebaseClient firebase = new FirebaseClient("https://ecob-3563a-default-rtdb.firebaseio.com/");
		public const string WebapyFirebase = "AIzaSyDrVBbR4GbM4PNWfVUZ6iLvDmBT_p9w_-c";
		public const string GoogleMapsApiKey = "AIzaSyAXouLv21vdv0fjnsN9tYBxbAAMZc1xfPA";
	
	}
}
