using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Nfc.CardEmulators;
using Android.Service;
using Java.Lang;

namespace CE2
{
	[Service(Exported=true, Enabled=true, Permission="android.permission.BIND_NFC_SERVICE"),
		IntentFilter(new []{"android.nfc.cardemulation.action.HOST_APDU_SERVICE"}),
		MetaData( "android.nfc.cardemulation.host_apdu_service", 
			Resource = "@xml/hceservice")]
	public class CardService : HostApduService 
	{
		private const string SAMPLE_LOYALTY_CARD_AID = "F222222222";
		// ISO-DEP command HEADER for selecting an AID.
		// Format: [Class | Instruction | Parameter 1 | Parameter 2]
		private const string SELECT_APDU_HEADER = "00A40400";
		// "OK" status word sent in response to SELECT AID command (0x9000)
		private readonly byte[] SELECT_OK_SW = HexStringToByteArray("9000");
		// "UNKNOWN" status word sent in response to invalid APDU command (0x0000)
		private readonly byte[] UNKNOWN_CMD_SW = HexStringToByteArray("0000");
		private readonly byte[] SELECT_APDU = BuildSelectApdu(SAMPLE_LOYALTY_CARD_AID);

		private MainReceiver mReceiver;

		public override void OnCreate() {
			base.OnCreate ();
			Console.WriteLine ();
			mReceiver = new MainReceiver()
			{
				SendApdu = (s) =>
				{
					SendResponseApdu (Encoding.UTF8.GetBytes(s));
				}
			};
			RegisterReceiver(mReceiver, new IntentFilter("CE2.cardservice.sendApdu"));
		}
			


		public override void OnDeactivated(DeactivationReason reason) 
		{
			UnregisterReceiver(mReceiver);

			return;
		}

		public override byte[] ProcessCommandApdu(byte[] commandApdu, Bundle extras) 
		{
			Console.WriteLine ("Received APDU:{0}",ByteArrayToHexString(commandApdu));
			Console.WriteLine ("SELECT_APDU:{0}",ByteArrayToHexString(SELECT_APDU));
			//Android.OS.Debug.WaitForDebugger(); 
			bool arrayEquals;
			if (SELECT_APDU.Length == commandApdu.Length) {
				arrayEquals = true;
				for (int i = 0; i < SELECT_APDU.Length; i++) {
					if (SELECT_APDU [i] != commandApdu [i]) {
						arrayEquals = false;
						break;
					}
				}
			} else {
				arrayEquals = false;
			}
			MainActivity.chatService.Write (Encoding.UTF8.GetBytes(ByteArrayToHexString(commandApdu)));
			Console.WriteLine ("arrayEquals:{0}",arrayEquals);
			if (arrayEquals) {
				Console.WriteLine ("aaa");
				string account = AccountStorage.GetAccount (this);
				byte[] accountBytes = Encoding.UTF8.GetBytes (account);

				MainActivity.chatService.Write (Encoding.UTF8.GetBytes(ByteArrayToHexString(commandApdu)));
				//return null;
				return ConcatArrays (accountBytes, SELECT_OK_SW);
			} else {
				return UNKNOWN_CMD_SW;
			}
		}

		public static byte[] BuildSelectApdu(string aid) {
			// Format: [CLASS | INSTRUCTION | PARAMETER 1 | PARAMETER 2 | LENGTH | DATA]
			return  HexStringToByteArray (SELECT_APDU_HEADER + (aid.Length / 2).ToString("X2") + aid);
				//HexStringToByteArray(SELECT_APDU_HEADER + string.Format("%02X", aid.Length / 2) + aid);
		}

		public static string ByteArrayToHexString(byte[] bytes) {
			string s = "";
			for (int i = 0; i < bytes.Length; i++) {
				s += bytes[i].ToString ("X2");
			}
			return s;

			/*char[] hexArray = {'0','1','2','3','4','5','6','7','8','9','A','B','C','D','E','F'};
			char[] hexChars = new char[bytes.Length * 2]; // Each byte has two hex characters (nibbles)
			int v;
			for (int j = 0; j < bytes.Length; j++) {
				v = bytes[j] & 0xFF; // Cast bytes[j] to int, treating as unsigned value
				//v = (uint) v;s
				hexChars[j * 2] = hexArray[v >> 4]; // Select hex character from upper nibble
				hexChars[j * 2 + 1] = hexArray[v & 0x0F]; // Select hex character from lower nibble
			}
			return new string(hexChars);*/
		}

		public static byte[] HexStringToByteArray(string s) {
			int len = s.Length;
			if (len % 2 == 1) {
				throw new ArgumentException ("Hex string must have even number of characters");
			}
			byte[] data = new byte[len / 2]; // Allocate 1 byte per 2 hex characters
			for (int i = 0; i < len; i += 2) {
				ushort val, val2;
				// Convert each chatacter into an unsigned integer (base-16)
				val = (ushort)Convert.ToInt32 (s[i].ToString() + "0", 16);
				val2 = (ushort)Convert.ToInt32 ("0" + s [i + 1].ToString(), 16);
				data [i / 2] = (byte)(val + val2);
				//data[i / 2] = (byte) (( Character.Digit(s[i], 16) << 4)
				//	+ Character.Digit(s[i+1], 16));
			}
			return data;
		}

		public static byte[] ConcatArrays(byte[] first, params byte[][] rest) {
			int totalLength = first.Length;
			foreach (byte[] array in rest) {
				totalLength += array.Length;
			}
			byte[] result= new byte[totalLength];
			first.CopyTo (result, 0);
			int offset = first.Length;
			foreach (byte[] array in rest) {
				array.CopyTo (result, offset);
				offset += array.Length;
			}
			return result;
		}
	}
}

