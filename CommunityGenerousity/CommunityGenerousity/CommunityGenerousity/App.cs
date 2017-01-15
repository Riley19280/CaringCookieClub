using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Auth;
using Xamarin.Forms;

namespace CaringCookieClub
{
	public class App : Application
	{
		public static manager MANAGER;
		public static ICredentialManager CredManager;
		public static OAuthReqManager ORM;

		public static INavigation navigation;
		public static main mainPage;
		public App()
		{
			#region Style
			Resources = new ResourceDictionary();
			var contentPageStyle = new Style(typeof(ContentPage))
			{
				Setters = {
				new Setter { Property = ContentPage.BackgroundColorProperty, Value = Constants.palette.primary },
				}
			};
			Resources.Add("contentPageStyle",contentPageStyle);

			var labelStyle = new Style(typeof(Label))
			{
				Setters = {
				new Setter { Property = Label.TextColorProperty, Value = Constants.palette.primary_text },
				}
			};
			Resources.Add(labelStyle);

			var editorStyle = new Style(typeof(Editor))
			{
				Setters = {
				new Setter { Property = Editor.TextColorProperty, Value = Constants.palette.primary_text },
				new Setter { Property = Editor.BackgroundColorProperty, Value = Constants.palette.primary_variant },
				}
			};
			Resources.Add(editorStyle);

			var buttonStyle = new Style(typeof(Button))
			{
				Setters = {
				new Setter { Property = Button.TextColorProperty, Value = Constants.palette.primary_text },
				new Setter { Property = Button.BackgroundColorProperty, Value = Constants.palette.primary_variant },
				}
			};
			Resources.Add(buttonStyle);

			var activityIndicatorStyle = new Style(typeof(ActivityIndicator))
			{
				Setters = {
				new Setter { Property = ActivityIndicator.ColorProperty, Value = Constants.palette.primary_dark_variant },
				}
			};
			Resources.Add(activityIndicatorStyle);

			var listViewStyle = new Style(typeof(ListView))
			{
				Setters = {
				new Setter { Property = ListView.SeparatorColorProperty, Value = Constants.palette.primary_text },
				}
			};
			Resources.Add(listViewStyle);


			#endregion

			MANAGER = new manager(new CaringCookieClubService());
			CredManager = DependencyService.Get<ICredentialManager>();
			ORM = new OAuthReqManager();

			//CredManager.DeleteCredentials();
			mainPage = new main() {
				//BarBackgroundColor = Constants.palette.primary_dark,
			};
			MainPage = new NavigationPage(mainPage) {
				BarBackgroundColor = Constants.palette.primary_dark,
			};
			navigation = MainPage.Navigation;
			MainPage.Title = "Caring Cookie Club";

		}



		public static Action SuccessfulLoginAction
		{
			get
			{
				return new Action(async () =>
				{
					try
					{
						navigation.PopModalAsync();		
			
						if (!await ORM.GetProfileInfo())
						{
							await mainPage.DisplayAlert("Error logging in","Error logging in, Please try again","Dismiss");

							CredManager.DeleteCredentials();
							mainPage.SelectedItem = mainPage.Children[0];

							return;
						}
					
						
						await App.MANAGER.UpdateOrCreateProfileAsync(new myDataTypes.profile(new myDataTypes.userInfo(App.CredManager.GetAccountValue("G_id"), App.CredManager.GetAccountValue("G_name")), "", DateTime.Now, App.CredManager.GetAccountValue("G_picture"), new DateTime(2000, 1, 1)));
						
						myProfile prof = new myProfile(false);
						mainPage.Children.RemoveAt(0);
						mainPage.Children.Insert(0, prof);
						mainPage.Children.Insert(1, new mycookies());
						mainPage.SelectedItem = mainPage.Children[0];

						//wait till /|\ is done
						prof.isReady = true;//might not be necessary, keeping it to be safe
						prof.populateProfileFields();
					}
					catch (Exception e)
					{
												
					}

				});
			}
		}
	}
}
