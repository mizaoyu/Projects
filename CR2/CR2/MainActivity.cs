using System;
using Android.App;
using Android.Bluetooth;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using Android.OS;
using Android.Util;
using Java.Lang;

//using CommonSampleLibrary;

namespace CR2 {
	/**
	 * A simple launcher activity containing a summary sample description, sample log and a custom
	 * Fragment which can display a view.
	 * <p>
	 * For devices with displays with a width of 720dp or greater, the sample log is always visible,
	 * on other devices it's visibility is controlled by an item on the Action Bar.
	 */
	[Activity (Label = "@string/app_name", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity //SampleActivityBase
	{

		public const string TAG = "MainActivity";

		// Whether the Log Fragment is currently shown
		private bool mLogShown;
		private TextView mAccountField;

		private const bool Debug = true;

		// Message types sent from the BluetoothChatService Handler
		// TODO: Make into Enums
		public const int MESSAGE_STATE_CHANGE = 1;
		public const int MESSAGE_READ = 2;
		public const int MESSAGE_WRITE = 3;
		public const int MESSAGE_DEVICE_NAME = 4;
		public const int MESSAGE_TOAST = 5;

		// Key names received from the BluetoothChatService Handler
		public const string DEVICE_NAME = "device_name";
		public const string TOAST = "toast";

		// Intent request codes
		// TODO: Make into Enums
		private const int REQUEST_CONNECT_DEVICE = 1;
		private const int REQUEST_ENABLE_BT = 2;

		// Layout Views
		private ListView conversationView;

		// Name of the connected device
		protected string connectedDeviceName = null;
		// Array adapter for the conversation thread
		public ArrayAdapter<string> conversationArrayAdapter;
		// String buffer for outgoing messages
		private StringBuffer outStringBuffer;
		// Local Bluetooth adapter
		private BluetoothAdapter bluetoothAdapter = null;
		// Member object for the chat services
		public static BluetoothChatService chatService = null;



		protected override void OnCreate(Bundle savedInstanceState) {
			base.OnCreate(savedInstanceState);
			this.SetContentView (Resource.Layout.activity_main);

			FragmentTransaction transaction = FragmentManager.BeginTransaction ();
			CardReaderFragment fragment = new CardReaderFragment();
			transaction.Replace(Resource.Id.sample_content_fragment, fragment);
			transaction.Commit();

			// Get local Bluetooth adapter
			bluetoothAdapter = BluetoothAdapter.DefaultAdapter;

			// If the adapter is null, then Bluetooth is not supported
			if (bluetoothAdapter == null) {
				Toast.MakeText (this, "Bluetooth is not available", ToastLength.Long).Show ();
				Finish ();
				return;
			}
		}

		/*public override bool OnCreateOptionsMenu(IMenu menu)
		{
			MenuInflater.Inflate(Resource.Menu.main, menu);
			return true;
		}*/

		/*public override bool OnPrepareOptionsMenu(IMenu menu) {

		IMenuItem logToggle = menu.FindItem(Resource.Id.menu_toggle_log);
		logToggle.SetVisible(FindViewById(Resource.Id.sample_output) is ViewAnimator);
		logToggle.SetTitle(mLogShown ? Resource.String.sample_hide_log : Resource.String.sample_show_log);

		return base.OnPrepareOptionsMenu(menu);
	}*/

	/*public override bool OnOptionsItemSelected(IMenuItem item) {
	switch(item.ItemId) {
	case Resource.Id.menu_toggle_log:
		mLogShown = !mLogShown;
		ViewAnimator output = (ViewAnimator) FindViewById(Resource.Id.sample_output);
		if (mLogShown) {
			output.DisplayedChild = 1;
		} else {
			output.DisplayedChild = 0;
		}
		SupportInvalidateOptionsMenu();
		return true;
	}
	return base.OnOptionsItemSelected (item);
}*/

/** Create a chain of targets that will receive log data */
/*public override void InitializeLogging()
		{
			// Wraps Android's native log framework
			LogWrapper logWrapper = new LogWrapper ();

	        Log.LogNode = logWrapper;

			// Filter strips out everything except the message text
			MessageOnlyLogFilter msgFilter = new MessageOnlyLogFilter ();
			logWrapper.NextNode = msgFilter;

			// On screen logging via a fragment with a TextView
			LogFragment logFragment = (LogFragment)SupportFragmentManager.FindFragmentById (Resource.Id.log_fragment);
			msgFilter.NextNode = logFragment.LogView;

			Log.Info (TAG, "Ready");

		}  */


protected override void OnStart ()
{
	base.OnStart ();

	if (Debug)
		Android.Util.Log.Error (TAG, "++ ON START ++");

	// If BT is not on, request that it be enabled.
	// setupChat() will then be called during onActivityResult
	if (!bluetoothAdapter.IsEnabled) {
		Intent enableIntent = new Intent (BluetoothAdapter.ActionRequestEnable);
		StartActivityForResult (enableIntent, REQUEST_ENABLE_BT);
		// Otherwise, setup the chat session
	} else {
		if (chatService == null)
			SetupChat ();
	}
}

protected override void OnResume ()
{
	base.OnResume ();

	// Performing this check in onResume() covers the case in which BT was
	// not enabled during onStart(), so we were paused to enable it...
	// onResume() will be called when ACTION_REQUEST_ENABLE activity returns.
	if (chatService != null) {
		// Only if the state is STATE_NONE, do we know that we haven't started already
		if (chatService.GetState () == BluetoothChatService.STATE_NONE) {
			// Start the Bluetooth chat services
			chatService.Start ();
		}
	}
}

private void SetupChat ()
{
	Android.Util.Log.Debug (TAG, "SetupChat()");

	// Initialize the array adapter for the conversation thread
	conversationArrayAdapter = new ArrayAdapter<string> (this, Resource.Layout.message);
	conversationView = FindViewById<ListView> (Resource.Id.@in);
	conversationView.Adapter = conversationArrayAdapter;

	// Initialize the BluetoothChatService to perform bluetooth connections
	chatService = new BluetoothChatService (this, new MyHandler (this));

	// Initialize the buffer for outgoing messages
	outStringBuffer = new StringBuffer ("");
}

protected override void OnPause ()
{
	base.OnPause ();

	if (Debug)
		Android.Util.Log.Error (TAG, "- ON PAUSE -");
}

protected override void OnStop ()
{
	base.OnStop ();

	if(Debug)
		Android.Util.Log.Error (TAG, "-- ON STOP --");
}

protected override void OnDestroy ()
{
	base.OnDestroy ();

	// Stop the Bluetooth chat services
	if (chatService != null)
		chatService.Stop ();

	if (Debug)
		Android.Util.Log.Error (TAG, "--- ON DESTROY ---");
}

private void EnsureDiscoverable ()
{
	if (Debug)
		Android.Util.Log.Debug (TAG, "ensure discoverable");

	if (bluetoothAdapter.ScanMode != ScanMode.ConnectableDiscoverable) {
		Intent discoverableIntent = new Intent (BluetoothAdapter.ActionRequestDiscoverable);
		discoverableIntent.PutExtra (BluetoothAdapter.ExtraDiscoverableDuration, 300);
		StartActivity (discoverableIntent);
	}
}

/// <summary>
/// Sends a message.
/// </summary>
/// <param name='message'>
/// A string of text to send.
/// </param>
private void SendMessage (Java.Lang.String message)
{
	// Check that we're actually connected before trying anything
	if (chatService.GetState () != BluetoothChatService.STATE_CONNECTED) {
		Toast.MakeText (this, Resource.String.not_connected, ToastLength.Short).Show ();
		return;
	}

	// Check that there's actually something to send
	if (message.Length () > 0) {
		// Get the message bytes and tell the BluetoothChatService to write
		byte[] send = message.GetBytes ();
		chatService.Write (send);

		// Reset out string buffer to zero and clear the edit text field
		outStringBuffer.SetLength (0);
	}
}


// The Handler that gets information back from the BluetoothChatService inner class
private class MyHandler : Handler
{
	MainActivity bluetoothChat;

	public MyHandler (MainActivity chat)
	{
		bluetoothChat = chat;	
	}

	public override void HandleMessage (Message msg)
	{
		switch (msg.What) {
		case MESSAGE_STATE_CHANGE:
			if (Debug)
				Log.Info (TAG, "MESSAGE_STATE_CHANGE: " + msg.Arg1);
			switch (msg.Arg1) {
			case BluetoothChatService.STATE_CONNECTED:
				Log.Info (TAG, "MESSAGE_STATE_CHANGE: STATE_CONNECTED");
				bluetoothChat.conversationArrayAdapter.Clear ();
				//bluetoothChat.conversationArrayAdapter.Clear ();
				break;
			case BluetoothChatService.STATE_CONNECTING:
				Log.Info (TAG, "MESSAGE_STATE_CHANGE: STATE_CONNECTING");
				break;
			case BluetoothChatService.STATE_LISTEN:
			case BluetoothChatService.STATE_NONE:
				Log.Info (TAG, "MESSAGE_STATE_CHANGE: STATE_LISTEN/None");
				break;
			}
			break;
		case MESSAGE_WRITE:
			byte[] writeBuf = (byte[])msg.Obj;
			// construct a string from the buffer
			var writeMessage = new Java.Lang.String (writeBuf);
			bluetoothChat.conversationArrayAdapter.Add ("Me: " + writeMessage);
			//bluetoothChat.conversationArrayAdapter.Add ("Me: " + writeMessage);
			break;
		case MESSAGE_READ:
			byte[] readBuf = (byte[])msg.Obj;
			// construct a string from the valid bytes in the buffer
			var readMessage = new Java.Lang.String (readBuf, 0, msg.Arg1);
			bluetoothChat.conversationArrayAdapter.Add (bluetoothChat.connectedDeviceName + ":  " + readMessage);
			//*********sendback to card...........
			//bluetoothChat.conversationArrayAdapter.Add (bluetoothChat.connectedDeviceName + ":  " + readMessage);
			break;
		case MESSAGE_DEVICE_NAME:
			// save the connected device's name
			bluetoothChat.connectedDeviceName = msg.Data.GetString (DEVICE_NAME);
			Toast.MakeText (Application.Context, "Connected to " + bluetoothChat.connectedDeviceName, ToastLength.Short).Show ();
			break;
		case MESSAGE_TOAST:
			Toast.MakeText (Application.Context, msg.Data.GetString (TOAST), ToastLength.Short).Show ();
			break;
		}
	}
}

protected override void OnActivityResult (int requestCode, Result resultCode, Intent data)
{
	if (Debug)
		Android.Util.Log.Debug (TAG, "onActivityResult " + resultCode);

	switch(requestCode)
	{
	case REQUEST_CONNECT_DEVICE:
		// When DeviceListActivity returns with a device to connect
		if( resultCode == Result.Ok)
		{
			// Get the device MAC address
			var address = data.Extras.GetString(DeviceListActivity.EXTRA_DEVICE_ADDRESS);
			// Get the BLuetoothDevice object
			BluetoothDevice device = bluetoothAdapter.GetRemoteDevice(address);
			// Attempt to connect to the device
			chatService.Connect(device);
		}
		break;
	case REQUEST_ENABLE_BT:
		// When the request to enable Bluetooth returns
		if(resultCode == Result.Ok)
		{
			// Bluetooth is now enabled, so set up a chat session
			SetupChat();	
		}
		else
		{
			// User did not enable Bluetooth or an error occured
			Android.Util.Log.Debug(TAG, "BT not enabled");
			Toast.MakeText(this, Resource.String.bt_not_enabled_leaving, ToastLength.Short).Show();
			Finish();
		}
		break;
	}
}

public override bool OnCreateOptionsMenu (IMenu menu)
{
	var inflater = MenuInflater;
	inflater.Inflate(Resource.Menu.option_menu, menu);
	return true;
}

public override bool OnOptionsItemSelected (IMenuItem item)
{
	switch (item.ItemId) 
	{
	case Resource.Id.scan:
		// Launch the DeviceListActivity to see devices and do scan
		var serverIntent = new Intent(this, typeof(DeviceListActivity));
		StartActivityForResult(serverIntent, REQUEST_CONNECT_DEVICE);
		return true;
	case Resource.Id.discoverable:
		// Ensure this device is discoverable by others
		EnsureDiscoverable();
		return true;
	}
	return false;
}


}
}