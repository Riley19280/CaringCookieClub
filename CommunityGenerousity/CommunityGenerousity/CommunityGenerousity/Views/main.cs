using CaringCookieClub.GUI;
using CaringCookieClub.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

using Xamarin.Forms;

namespace CaringCookieClub
{
	public class main : TabbedPage
	{
		public main()
		{
			this.Title = "Main Menu";
			
			NavigationPage.SetHasNavigationBar(this, false);

			if (!App.CredManager.IsLoggedIn())
			{
				this.Children.Add(new login());
			}
			else {
				this.Children.Add(new myProfile(true));
				this.Children.Add(new mycookies());
			}

			//this.Children.Add(new othersProfile(App.CredManager.GetAccountValue("G_id")));


			this.Children.Add(new peopleifollow());

			this.Children.Add(new mostpopular());

			this.Children.Add(new searchUsers());
		}
	}
}

