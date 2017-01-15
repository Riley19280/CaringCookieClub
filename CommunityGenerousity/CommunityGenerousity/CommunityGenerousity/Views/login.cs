using CaringCookieClub.GUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using Xamarin.Auth;
using Xamarin.Forms;

namespace CaringCookieClub
{
	public class login : BasePage
	{
		Button LoginButton = new Button
		{
			Text = "Login",
			VerticalOptions = LayoutOptions.CenterAndExpand,
			HorizontalOptions = LayoutOptions.CenterAndExpand
		};

		Label WelcomeLabel = new Label
		{
			Text = "Please log in to see your profile",
			VerticalOptions = LayoutOptions.CenterAndExpand,
			HorizontalOptions = LayoutOptions.CenterAndExpand
		};
		


		public login()
		{
			Title = "Login";
			LoginButton.Clicked += LoginButton_Clicked;

			Content = new StackLayout
			{

				Children = {
					WelcomeLabel,
					LoginButton


				}
			};

		}

		private void LoginButton_Clicked(object sender, EventArgs e)
		{
			Navigation.PushModalAsync(new LoginPage());
		}
	}
}
