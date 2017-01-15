using CaringCookieClub.GUI;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CaringCookieClub
{
	class mycookies : BasePage
	{
		ListView listView;
		StackLayout baseStack;

		public mycookies()
		{
			Title = "Cookies I've Recieved";

			createListView();

			baseStack = new StackLayout
			{
				Children = {
					new Label {
						Text = "Cookies I've Recieved",
						HorizontalOptions = LayoutOptions.CenterAndExpand,
						FontSize = Device.GetNamedSize (NamedSize.Large, typeof(Label)),
					},
					listView
				}
			};



			ActivityIndicator i = new ActivityIndicator();
			i.IsRunning = true;
			Content = i;
		}

		protected async override void OnAppearing()
		{
			base.OnAppearing();
			if (listView.ItemsSource == null && App.CredManager.IsLoggedIn())
			{
				await Task.Delay(1000);

				listView.ItemsSource = await App.MANAGER.GetRecieviedActivitys(App.CredManager.GetAccountValue("G_id"));
				Content = baseStack;
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
					Label nameLabel = new Label(), userLabel = new Label(), descriptionLabel = new Label();
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
				listView.ItemsSource = await App.MANAGER.GetRecieviedActivitys(App.CredManager.GetAccountValue("G_id"));
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