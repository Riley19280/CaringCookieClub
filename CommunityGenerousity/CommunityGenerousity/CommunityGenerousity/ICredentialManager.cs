using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Auth;

namespace CaringCookieClub
{
	public interface ICredentialManager
	{

		void DeleteCredentials();
		Account GetCredentials();
		void RequestRefreshTokenAsync(string refreshToken);

		void SetAccountValue( string key, string value);
		void UpdateAccountValue(string key, DateTime value);
		string GetAccountValue( string key);
		DateTime GetAccountDate(string key);

		bool IsLoggedIn();
	}
}
