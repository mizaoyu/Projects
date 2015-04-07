using System;
using System.Text;

using Android.App;
using Android.Bluetooth;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using Android.OS;
using Android.Util;
using Java.Lang;
//using Android.Content.BroadcastReceiver;  

namespace CE2
{
	public class MainReceiver : BroadcastReceiver
    {
		public Action<string> SendApdu;
		 
		public override void OnReceive(Context context, Intent intent)
        {
			Console.WriteLine ("ccc");
 	    	string str = intent.GetStringExtra("_apdu");
			if (SendApdu != null) {
				SendApdu (str);
			}
	    }
	}

}

