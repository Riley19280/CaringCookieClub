using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using CaringCookieClub.GUI;
using CommunityGenerousity.Droid;
using Xamarin.Forms.Platform.Android;
using Xamarin.Auth;
using CaringCookieClub;

/*
			 http://www.appliedcodelog.com/2015/08/login-by-google-account-integration-for.html
		 http://stackoverflow.com/questions/30774510/offline-authentication-best-practices-in-xamarin
		 https://developer.xamarin.com/recipes/cross-platform/xamarin-forms/general/store-credentials/
		 https://lostechies.com/jimmybogard/2014/11/13/mobile-authentication-with-xamarin-auth-and-refresh-tokens/
		 https://components.xamarin.com/gettingstarted/xamarin.auth
		 */

[assembly: ExportRenderer(typeof(LoginPage), typeof(LoginPageRenderer))]

namespace CommunityGenerousity.Droid
{
	public class LoginPageRenderer : PageRenderer
	{
		protected override void OnElementChanged(ElementChangedEventArgs<Page> e)
		{
			base.OnElementChanged(e);

			// this is a ViewGroup - so should be able to load an AXML file and FindView<>
			var activity = this.Context as Activity;

			try
			{

				var auth = new OAuth2Authenticator(
					Constants.ClientId, // your OAuth2 client id
					Constants.Scope, // the scopes for the particular API you're accessing, delimited by "+" symbols
					new Uri(Constants.AuthorizeUrl), // the auth URL for the service
					new Uri(Constants.RedirectUrl)); // the redirect URL for the service


				AccountStore store = AccountStore.Create(this.Context);
				Account savedAccount = store.FindAccountsForService(Constants.AppName).FirstOrDefault();
				if (savedAccount != null)
				{
					//TODO: check tokens expired? may not be needed bc this is the login page so they couldnt be. should check for login at login button click

				}
				else
				{
					auth.Completed += (sender, eventArgs) =>
					{
						if (eventArgs.IsAuthenticated)
						{
							// Use eventArgs.Account to do wonderful things

							//string access_token;
							//eventArgs.Account.Properties.TryGetValue("access_token", out access_token);

							store.Save(eventArgs.Account, Constants.AppName);

							App.SuccessfulLoginAction.Invoke();

							
						}
						else // Authentication failed
						{
							Toast.MakeText(this.Context, "Error logging in", ToastLength.Long).Show();
						}
						};
					activity.StartActivity(auth.GetUI(activity));
				};

				
			}
			catch (Exception ex)
			{
				Toast.MakeText(this.Context, "Error logging in", ToastLength.Long).Show();
				App.CredManager.DeleteCredentials();
				
				App.mainPage.Children.RemoveAt(0);
				App.mainPage.Children.Insert(0, new login());
				App.mainPage.SelectedItem = App.mainPage.Children[0];
			}



		}
	}
}