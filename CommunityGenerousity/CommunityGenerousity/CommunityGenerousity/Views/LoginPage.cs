using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

using Xamarin.Forms;

namespace CaringCookieClub.GUI
{
	public class LoginPage : BasePage
	{
		Button BackButton = new Button
		{
			Text = "Back",
			VerticalOptions = LayoutOptions.CenterAndExpand,
			HorizontalOptions = LayoutOptions.CenterAndExpand
		};

		public LoginPage() {
			Title = "Login";
			BackButton.Clicked += BackButton_Clicked;

			Content = new StackLayout
			{

				Children = {
					BackButton
				}
			};
		}

		private void BackButton_Clicked(object sender, EventArgs e)
		{
			App.navigation.PopModalAsync();
		}
	}
}
