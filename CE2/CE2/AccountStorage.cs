using System;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Text;
using Android.Preferences;

namespace CE2
{
	public static class AccountStorage
	{
		private const string PREF_ACCOUNT_NUMBER = "account_number";
		private const string DEFAULT_ACCOUNT_NUMBER = "00000000";
		private const string TAG = "AccountStorage";
		private static string sAccount = null;
		private static readonly object sAccountLock = new Object();

		public static void SetAccount(Context c, String s) {
			lock(sAccountLock) {
				//Log.i(TAG, "Setting  \taccount number: " + s);
				ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(c);
				prefs.Edit().PutString(PREF_ACCOUNT_NUMBER, s).Commit();
				sAccount = s;
				Console.WriteLine ("SetAccount:{0}",sAccount);
			}
		}

		public static string GetAccount(Context c) {
			lock(sAccountLock) {
				if (sAccount == null) {
					ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(c);
					string account = prefs.GetString(PREF_ACCOUNT_NUMBER, DEFAULT_ACCOUNT_NUMBER);
					sAccount = account;
				}
				return sAccount;
			}
		}
	}
}

