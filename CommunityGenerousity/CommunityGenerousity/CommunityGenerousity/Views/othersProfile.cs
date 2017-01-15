using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using CaringCookieClub.GUI;

using Xamarin.Forms;
using System.Diagnostics;

namespace CaringCookieClub.GUI
{
	public class othersProfile : BasePage
	{
		ListView listView;
		myDataTypes.profile prof;

		Label lblName = new Label
		{
			Text = "",
			VerticalOptions = LayoutOptions.StartAndExpand,
			HorizontalOptions = LayoutOptions.CenterAndExpand,

		};

		Label lblBio = new Label
		{
			Text = "",
			VerticalOptions = LayoutOptions.StartAndExpand,
			HorizontalOptions = LayoutOptions.CenterAndExpand,
		};

		Label lblStats = new Label
		{
			HorizontalOptions = LayoutOptions.CenterAndExpand,
			Text = ""
		};

		Image profImg = new Image
		{
			WidthRequest = 150,
			HeightRequest = 150,
		};

		Button followButton = new Button
		{
			HorizontalOptions = LayoutOptions.FillAndExpand,
		};

		Button sentCookies;

		Button recievedCookies;


		public othersProfile(string id)
		{
			sentCookies = new Button
			{
				Text = "  Sent Cookies  ",
				BorderRadius = 0,
				Margin = 0,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Command = new Command(async () =>
				{
					recievedCookies.IsEnabled = true;
					sentCookies.IsEnabled = false;
					listView.ItemsSource = null;
					listView.IsRefreshing = true;
					listView.ItemsSource = await App.MANAGER.GetActivitiesAssociatedWithProfile(id);
					listView.IsRefreshing = false;
				})
			};
			sentCookies.IsEnabled = false;

			recievedCookies = new Button
			{
				Text = "Received Cookies",
				BorderRadius = 0,
				Margin = 0,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Command = new Command(async () =>
				{
					sentCookies.IsEnabled = true;
					recievedCookies.IsEnabled = false;
					listView.ItemsSource = null;
					listView.IsRefreshing = true;
					listView.ItemsSource = await App.MANAGER.GetRecieviedActivitys(id);
					listView.IsRefreshing = false;
				})
			};

			if (!App.CredManager.IsLoggedIn())
			{
				followButton.IsEnabled = false;
			}

			CreateListView();
			try
			{
				populateProfileFields(id);
			}
			catch (Exception e)
			{
				Debug.WriteLine(e.Message);
				Debug.WriteLine(e.StackTrace);
			}

			Content = new StackLayout
			{
				Children = {
					new StackLayout {

						Orientation = StackOrientation.Horizontal,

						Children = {

							profImg,

							new StackLayout {
								VerticalOptions = LayoutOptions.FillAndExpand,
								HorizontalOptions = LayoutOptions.FillAndExpand,
								Children = {
									lblName,
									lblBio,
								}
							}
						}
					},
					followButton,

					new StackLayout {
						Orientation = StackOrientation.Horizontal,
						Padding = 0,
						Margin = 0,
						Spacing = 0,
						Children = {
							sentCookies,
							recievedCookies
						}

					},

					listView

				}
			};
		}

		public async void populateProfileFields(string id)
		{
			prof = await App.MANAGER.GetProfileAsync(id);

			listView.ItemsSource = await App.MANAGER.GetActivitiesAssociatedWithProfile(id);

			if (prof != null)
			{
				lblName.Text = prof.userInfo.userName;

				Title = prof.userInfo.userName;

				profImg.Source = ImageSource.FromUri(new Uri(prof.picture_URL));

				lblBio.Text = prof.bio;

				if (App.CredManager.IsLoggedIn())
				{
					if (!await App.MANAGER.GetIsFollowingAsync(App.CredManager.GetAccountValue("G_id"), id))
					{
						doFollow();
					}
					else
					{
						doUnFollow();
					}
				}
				else
				{
					followButton.Text = "Log in to follow!";
				}
			}

			//int i = await App.MANAGER.GetActivityCountAsync(id);
			//lblStats.Text = "Number of cookies: " + i.ToString();



		}

		private void doFollow()
		{
			followButton.IsEnabled = true;
			followButton.Text = "Follow this person";
			followButton.Command = new Command(() =>
			{
				//follow
				App.MANAGER.FollowAsync(App.CredManager.GetAccountValue("G_id"), prof.userInfo.user_id);
				doUnFollow();
			});
		}

		private void doUnFollow()
		{
			followButton.IsEnabled = true;
			followButton.Text = "Unfollow this person";
			followButton.Command = new Command(() =>
			{
				//unfollow
				App.MANAGER.UnFollowAsync(App.CredManager.GetAccountValue("G_id"), prof.userInfo.user_id);
				doFollow();
			});
		}

		private void CreateListView()
		{

			listView = new ListView
			{
				// Source of data items.


				RowHeight = 75,
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
					picture_URL.SetBinding(Label.TextProperty, "picture_URL");


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

			listView.ItemSelected += (async (s, e) =>
			{
				if (listView.SelectedItem != null)
				{
					var actView = new activityView(listView.SelectedItem as myDataTypes.activity);
					actView.disableVisitButton();
					listView.SelectedItem = null;
					await Navigation.PushAsync(actView);
				}
			});
		}
	}
}
