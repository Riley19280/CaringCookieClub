using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using CaringCookieClub;
using Xamarin.Auth;
using Xamarin.Forms;
using System.Threading.Tasks;

[assembly: Xamarin.Forms.Dependency(typeof(CaringCookieClub.Droid.CredentialManager))]
namespace CaringCookieClub.Droid
{

	public class CredentialManager : ICredentialManager
	{
		//https://developer.xamarin.com/recipes/cross-platform/xamarin-forms/general/store-credentials/
		public void DeleteCredentials()
		{
			Constants.AdminUsers = null;
			var account = AccountStore.Create(Forms.Context).FindAccountsForService(Constants.AppName).FirstOrDefault();
			if (account != null)
			{
				AccountStore.Create(Forms.Context).Delete(account, Constants.AppName);
			}
		}

		public Account GetCredentials()
		{
			Account a = AccountStore.Create().FindAccountsForService(Constants.AppName).FirstOrDefault();
			return a;
		}

		public bool IsLoggedIn()
		{
			//TODO: check if expired tokens (refresh)
			if (GetCredentials() != null)
				return true;
			else
				return false;

		}

		public void RequestRefreshTokenAsync(string refreshToken)
		{
			var queryValues = new Dictionary<string, string>{
				{"refresh_token", refreshToken},
				{"client_id", Constants.ClientId},
				{"grant_type", "refresh_token"}};

			if (!string.IsNullOrEmpty(Constants.ClientSecret))
			{
				queryValues["client_secret"] = Constants.ClientSecret;
			}

			var c = GetCredentials().Properties["access_token"];



		}

		public void SetAccountValue(string key, string value)
		{
			Account acct = GetCredentials();
		//	if (!acct.Properties.ContainsKey(key))
				acct.Properties.Add(key, value);
			//else
			//	acct.Properties[key] = value;
			AccountStore.Create(Forms.Context).Save(acct, Constants.AppName);
		}

		public string GetAccountValue(string key)
		{
			Account acct = GetCredentials();
			string s = (acct != null) ? acct.Properties[key] : null;
			return s;
		}

		public void UpdateAccountValue(string key, DateTime value)
		{
			Account acct = GetCredentials();
			if (!acct.Properties.ContainsKey(key))
				acct.Properties.Add(key, value.ToString());
			else
				acct.Properties[key] = value.ToString();
			AccountStore.Create(Forms.Context).Save(acct, Constants.AppName);
		}

		public DateTime GetAccountDate(string key)
		{
			try
			{
				Account acct = GetCredentials();
				DateTime s = (acct != null) ? DateTime.Parse(acct.Properties[key]) : new DateTime(2000,1,1);
				return s;
			}
			catch (KeyNotFoundException e)
			{
				UpdateAccountValue(key, new DateTime(2000, 1, 1));
				return new DateTime(2000, 1, 1) ;
			}
			
		}
	}
}