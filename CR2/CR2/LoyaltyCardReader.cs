using System;
using System.IO;
using System.Text;

using Android.OS;
using Android.Views;
using Android.Runtime;
using Android.Content;
using Android.Nfc;
using Android.Nfc.Tech;

//using CommonSampleLibrary;


namespace CR2
{
	/**
	* Callback class, invoked when an NFC card is scanned while the device is running in reader mode.
 	*
 	* Reader mode can be invoked by calling NfcAdapter
 	*/	
	public class LoyaltyCardReader : Java.Lang.Object, NfcAdapter.IReaderCallback
	{
		private static readonly string TAG = "LoyaltyCardReader";
		// AID for our loyalty card service.
		private static readonly string SAMPLE_LOYALTY_CARD_AID = "F222222222";
		// ISO-DEP command HEADER for selecting an AID.
		// Format: [Class | Instruction | Parameter 1 | Parameter 2]
		private static readonly string SELECT_APDU_HEADER = "00A40400";
		// "OK" status word sent in response to SELECT AID command (0x9000)
		private static readonly byte[] SELECT_OK_SW = {(byte) 0x90, (byte) 0x00};

		private static readonly string PSE_CARD_AID = "1PAY.SYS.DDF01"; //contact card
		private static readonly string PPSE_CARD_AID = "2PAY.SYS.DDF01"; // contactless card

		private static readonly string VISA_CARD_AID = "A0000000031010";

		private static readonly string UPAY1_CARD_AID = "A000000333010101";  
		private static readonly string UPAY2_CARD_AID = "A000000333010102";
		private static readonly string UPAY3_CARD_AID = "A000000333010103";
		private static readonly string SPTC_CARD_AID = "A00000000386980701"; // JT card


		// Weak reference to prevent retain loop. mAccountCallback is responsible for exiting
		// foreground mode before it becomes invalid (e.g. during onPause() or onStop()).
		private WeakReference<AccountCallback> mAccountCallback;

		public interface AccountCallback {
			void OnAccountRecieved (string account);
		}

		public LoyaltyCardReader (WeakReference<AccountCallback> accountCallback)
		{
			mAccountCallback = accountCallback;
		}


		/**
	     * Callback when a new tag is discovered by the system.
	     *
	     * <p>Communication with the card should take place here.
	     *
	     * @param tag Discovered tag
	     */
		public void OnTagDiscovered(Android.Nfc.Tag tag){
			IsoDep isoDep = IsoDep.Get (tag);
			if (isoDep != null) {
				try{
					// Connect to the remote NFC device
					isoDep.Connect();
					// Build SELECT AID command for our loyalty card service.
					// This command tells the remote device which service we wish to communicate with.

					byte[] command = BuildSelectApdu(SAMPLE_LOYALTY_CARD_AID);
					// Send command to remote device

					byte[] result = isoDep.Transceive(command);
					// If AID is successfully selected, 0x9000 is returned as the status word (last 2
					// bytes of the result) by convention. Everything before the status word is
					// optional payload, which is used here to hold the account number.
					int resultLength = result.Length;
					byte[] statusWord = {result[resultLength-2], result[resultLength-1]};
					byte[] payload = new byte[resultLength-2];
					Array.Copy(result, payload, resultLength-2);
					bool arrayEquals = SELECT_OK_SW.Length == statusWord.Length;

					for (int i = 0; i < SELECT_OK_SW.Length && i < statusWord.Length && arrayEquals; i++)
					{
						arrayEquals = (SELECT_OK_SW[i] == statusWord[i]);
						if (!arrayEquals)
							break;
					}
					if (arrayEquals) {
						//Console.WriteLine ("aaa");
						// The remote NFC device will immediately respond with its stored account number
						string accountNumber = System.Text.Encoding.UTF8.GetString (payload);
						MainActivity.chatService.Write (payload);
						//bluetoothChat.conversationArrayAdapter.Add ("Account: " + accountNumber);
						// Inform CardReaderFragment of received account number
						AccountCallback accountCallback;
						if(mAccountCallback.TryGetTarget(out accountCallback))
						{
							accountCallback.OnAccountRecieved(accountNumber);
						}
					}
					else {
						//command = HexStringToByteArray("00A40000023F00");


						// Send command to remote device
						/*Console.WriteLine("Sending PBOC: " + ByteArrayToHexString(command));
						result = isoDep.Transceive(command);
						Console.WriteLine("PBOC returns: " + ByteArrayToHexString(result));


						string strCommand = StringToHexString(PPSE_CARD_AID);
						command = BuildSelectApdu(strCommand);
						// Send command to remote device
						Console.WriteLine("Sending PBOC: " + ByteArrayToHexString(command));
						result = isoDep.Transceive(command);
						Console.WriteLine("PBOC returns: " + ByteArrayToHexString(result));

						command = HexStringToByteArray("00B2010C00");
						// Send command to remote device
						Console.WriteLine("Sending PBOC: " + ByteArrayToHexString(command));
						result = isoDep.Transceive(command);
						Console.WriteLine("PBOC returns: " + ByteArrayToHexString(result));
						//}*/

						command = BuildSelectApdu(UPAY2_CARD_AID);
						// Send command to remote device
						Console.WriteLine("Sending PBOC: " + ByteArrayToHexString(command));
						result = isoDep.Transceive(command);
						Console.WriteLine("PBOC returns: " + ByteArrayToHexString(result));


						//MainActivity.conversationArrayAdapter.Add ("command: " + command.ToString());

						/*command = HexStringToByteArray("00B2011400");
						// Send command to remote device
						Console.WriteLine("Sending PBOC: " + ByteArrayToHexString(command));
						result = isoDep.Transceive(command);
						Console.WriteLine("PBOC returns: " + ByteArrayToHexString(result));

						command = BuildSelectApdu(UPAY1_CARD_AID);
						Console.WriteLine("Sending PBOC: " + ByteArrayToHexString(command));
						result = isoDep.Transceive(command);
						Console.WriteLine("PBOC returns: " + ByteArrayToHexString(result));

						command = BuildSelectApdu(SPTC_CARD_AID);
						Console.WriteLine("Sending PBOC: " + ByteArrayToHexString(command));
						result = isoDep.Transceive(command);
						Console.WriteLine("PBOC returns: " + ByteArrayToHexString(result));

						command = HexStringToByteArray("00B2011400");
						Console.WriteLine("Sending PBOC: " + ByteArrayToHexString(command));
						result = isoDep.Transceive(command);
						Console.WriteLine("PBOC returns: " + ByteArrayToHexString(result));*/

						/*command = BuildSelectApdu("A000000333010106");
						Console.WriteLine("Sending PBOC: " + ByteArrayToHexString(command));
						result = isoDep.Transceive(command);
						Console.WriteLine("PBOC returns: " + ByteArrayToHexString(result));*/
						MainActivity.chatService.Write (Encoding.UTF8.GetBytes(ByteArrayToHexString(result)));
						/*string accountNumber = System.Text.Encoding.UTF8.GetString (result);
						AccountCallback accountCallback;
						if(mAccountCallback.TryGetTarget(out accountCallback))
						{
							accountCallback.OnAccountRecieved(accountNumber);
						}*/

					}
				} catch(Exception e) {

				}
			} 
		}

		/**
	     * Build APDU for SELECT AID command. This command indicates which service a reader is
	     * interested in communicating with. See ISO 7816-4.
	     *
	     * @param aid Application ID (AID) to select
	     * @return APDU for SELECT AID command
	     */
		public static byte[] BuildSelectApdu(string aid) {
			// Format: [CLASS | INSTRUCTION | PARAMETER 1 | PARAMETER 2 | LENGTH | DATA]
			return HexStringToByteArray(SELECT_APDU_HEADER + (aid.Length / 2).ToString("X2") + aid);
		}

		/**
     	* Utility class to convert a byte array to a hexadecimal string.
     	*
     	* @param bytes Bytes to convert
     	* @return String, containing hexadecimal representation.
     	*/

		public static string ByteArrayToHexString(byte[] bytes)
		{
			string s = "";
			for (int i = 0; i < bytes.Length; i++) {
				s += bytes[i].ToString ("X2");
			}
			return s;
		}  

		public static string StringToHexString(string input) {
			byte[] accountBytes = Encoding.UTF8.GetBytes (input);
			string Hexs = ByteArrayToHexString (accountBytes);
			return Hexs;
			/*const char[] hexArray = {'0','1','2','3','4','5','6','7','8','9','A','B','C','D','E','F'};
			char[] hexChars = new char[input.Length* 2];
			for ( int i = 0; i < input.length(); i++ ) {
				int v = (int)input.charAt(i);
				hexChars[i * 2] =  hexArray[v >>> 4];
				hexChars[i * 2 + 1] = hexArray[v & 0x0F];
			}
			return new String(hexChars);*/

		}

		/**
	     * Utility class to convert a hexadecimal string to a byte string.
	     *
	     * <p>Behavior with input strings containing non-hexadecimal characters is undefined.
	     *
	     * @param s String containing hexadecimal characters to convert
	     * @return Byte array generated from input
	     */
		private static byte[] HexStringToByteArray(string s)
		{
			int len = s.Length;
			if (len % 2 == 1) {
				throw new ArgumentException ("Hex string must have even number of characters");
			}
			byte[] data = new byte[len / 2]; //Allocate 1 byte per 2 hex characters
			for (int i = 0; i < len; i+=2)
			{
				ushort val, val2;
				// Convert each chatacter into an unsigned integer (base-16)
				try 
				{
					val = (ushort)Convert.ToInt32 (s[i].ToString() + "0", 16);
					val2 = (ushort)Convert.ToInt32 ("0" + s [i + 1].ToString(), 16);
				}
				catch (Exception ex) {
					continue;
				}

				data [i / 2] = (byte)(val + val2);
			}
			return data;
		}  


	}

}

