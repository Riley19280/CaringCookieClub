using CaringCookieClub.GUI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CaringCookieClub
{
	public class peopleifollow : BasePage
	{
		ListView listView;
		StackLayout baseStack;
		ActivityIndicator i;
		Label titleText;

		public peopleifollow()
		{
			Title = "People I Follow";

			createListView();

			titleText = new Label
			{
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),

			};

			baseStack = new StackLayout
			{
				Children = {
					titleText,
					 listView

				}
			};



			i = new ActivityIndicator();
			i.IsRunning = true;

			Content = i;
		}

		protected async override void OnAppearing()
		{
			base.OnAppearing();
			try
			{
				if (listView.ItemsSource == null && App.CredManager.IsLoggedIn())
				{
					await Task.Delay(1000);
					listView.ItemsSource = await App.MANAGER.GetActivitysFromFollowersAsync(App.CredManager.GetAccountValue("G_id"));
					Content = baseStack;
				}


				if (App.CredManager.IsLoggedIn())
				{
					titleText.Text = "People I Follow";
				}
				else
				{
					titleText.Text = "Log in to see people you follow";
					listView.ItemsSource = null;
					Content = baseStack;
				}
			}
			catch (Exception e)
			{
				Debug.WriteLine(e.Message);
				Debug.WriteLine(e.StackTrace);
			}

		}

		void createListView()
		{
			listView = new ListView
			{
				// Source of data items.


				RowHeight = 80,
				// Define template for displaying each item.
				// (Argument of DataTemplate constructor is called for 
				//      each item; it must return a Cell derivative.)
				ItemTemplate = new DataTemplate(() =>
				{
					// Create views with bindings for displaying each property.
					Label nameLabel = new Label(), userLabel = new Label(), descriptionLabel = new Label(), picture_URL = new Label();
					nameLabel.SetBinding(Label.TextProperty, "actInfo.actName");
					userLabel.SetBinding(Label.TextProperty, "userInfo.userName");
					descriptionLabel.SetBinding(Label.TextProperty, "description");

					Image imageView = new Image
					{
						HeightRequest = 75,
						WidthRequest = 75,

					};
					imageView.SetBinding(Image.SourceProperty, "picture_URL");


					// Return an assembled ViewCell.
					return new ViewCell
					{
						View = new StackLayout
						{
							Padding = new Thickness(5, 5, 5, 0),
							Orientation = StackOrientation.Horizontal,
							Children =
								{
									imageView,
									new StackLayout
									{
										VerticalOptions = LayoutOptions.Center,
										Spacing = 0,
										Children =
										{
											nameLabel,
											userLabel,
											descriptionLabel
										}
									}
								}
						}
					};
				})
			};
			listView.IsPullToRefreshEnabled = true;

			listView.Refreshing += (async (sender, eventArgs) =>
			{
				listView.ItemsSource = await App.MANAGER.GetActivitysFromFollowersAsync(App.CredManager.GetAccountValue("G_id"));
				listView.IsRefreshing = false;
			});

			listView.ItemSelected += ((sender, eventArgs) =>
			{
				if (listView.SelectedItem != null)
				{
					var actView = new activityView(listView.SelectedItem as myDataTypes.activity);

					listView.SelectedItem = null;
					Navigation.PushAsync(actView);
				}
			});
		}
	}
}
