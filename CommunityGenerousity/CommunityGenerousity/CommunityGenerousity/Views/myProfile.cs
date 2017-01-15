using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using CaringCookieClub.GUI;

using Xamarin.Forms;
using System.Diagnostics;
using CaringCookieClub.Views;
using System.Threading.Tasks;

namespace CaringCookieClub
{
	public class myProfile : BasePage
	{

		ListView listView;
		myDataTypes.profile prof;

		StackLayout baseStack;

		#region Views
		Label lblName = new Label
		{
			Text = "",
			VerticalOptions = LayoutOptions.StartAndExpand,
			HorizontalOptions = LayoutOptions.CenterAndExpand

		};

		Label lblBio = new Label
		{
			Text = "",

			HorizontalOptions = LayoutOptions.CenterAndExpand
		};

		Label lblStats = new Label
		{
			HorizontalOptions = LayoutOptions.CenterAndExpand,
			Text = ""
		};

		RelativeLayout relLayout = new RelativeLayout
		{
			Margin = 0,
			Padding = 0,
			HorizontalOptions = LayoutOptions.Start,
		};

		Image profImg = new Image
		{
			WidthRequest = 150,
			HeightRequest = 150,
		};

		Label profImgLabel = new Label
		{
			Text = "Click to change"
		};

		#endregion

		public bool isReady = false;

		public myProfile(bool ready)
		{
			Title = "Profile";
			isReady = ready;

			#region prof image
			profImg.GestureRecognizers.Add(new TapGestureRecognizer
			{
				Command = new Command(async () =>
				{
					var answer = await DisplayAlert("Change profile picture", "Would you like to change your profile picture?", "Yes", "No");
					if (answer)
					{
						//FIXME: wont work on windows platform
						Device.OpenUri(new Uri("https://plus.google.com/u/0/me?tab=XX"));
					}
				}),
				NumberOfTapsRequired = 1
			});

			relLayout.Children.Add(profImg,
				Constraint.RelativeToParent((parent) =>
				{
					return parent.X;
				}), Constraint.RelativeToParent((parent) =>
				{
					return parent.Y;
				}));

			relLayout.Children.Add(profImgLabel, Constraint.RelativeToView(profImg, (parent, sibling) =>
			{
				return (sibling.Width / 2) - (profImgLabel.Width / 2);
			}), Constraint.RelativeToView(profImg, (parent, sibling) =>
			{
				return sibling.Height - 5 - profImgLabel.Height;
			}));

			#endregion


			if (App.CredManager.IsLoggedIn())
			{
				CreateListView();

				#region basestack
				baseStack = new StackLayout
				{
					Children = {
							new StackLayout {
								//Padding = 0,
								//Margin = 0,
								//Spacing = 0,
								
								Orientation = StackOrientation.Horizontal,
								Children = {
									relLayout,

									new StackLayout {
										VerticalOptions = LayoutOptions.FillAndExpand,
										HorizontalOptions = LayoutOptions.FillAndExpand,
										Children = {
											lblName,
												lblBio,
												new Button {
													Text = "Edit bio",
													HorizontalOptions = LayoutOptions.FillAndExpand,
													VerticalOptions = LayoutOptions.End,

													Command = new Command(()=>{

														editBio eb = new editBio();
														eb.OnEditBio += (sender,editBioArgs)=> {
															lblBio.Text = editBioArgs.Bio;
														};
														Navigation.PushAsync(eb);
													})
												}
										}
						}

					}
					},
					new Button {
						Text = "Send new cookie",
						HorizontalOptions = LayoutOptions.FillAndExpand,
						Command = new Command(() => Navigation.PushAsync(new newpost()))
					},
							new Button {
						Text = "Logout",
						HorizontalOptions = LayoutOptions.FillAndExpand,
						Command = new Command(() => {
							App.CredManager.DeleteCredentials();
							App.mainPage.Children.Remove(this);
							App.mainPage.Children.RemoveAt(0);
							App.mainPage.Children.Insert(0,new login());
							App.mainPage.SelectedItem = App.mainPage.Children[0];
						})
					},

					lblStats,

					listView

				}
				};

				#endregion

				try
				{
					if (isReady)
						populateProfileFields();


				}
				catch (Exception e)
				{
					Debug.WriteLine(e.Message);
					Debug.WriteLine(e.StackTrace);
				}

				ActivityIndicator i = new ActivityIndicator();
				i.IsRunning = true;

				Content = i;

				i = new ActivityIndicator();
				i.IsRunning = true;

				Content = i;
				//Content = baseStack;
			}
		}

		bool adminAlert = false;

		public async void populateProfileFields()
		{
			await Task.Delay(500);
			prof = await App.MANAGER.GetProfileAsync(App.CredManager.GetAccountValue("G_id"));

			if (prof != null)
			{
				App.CredManager.UpdateAccountValue("lastUpvote", prof.lastUpvote);

				listView.ItemsSource = await App.MANAGER.GetActivitiesAssociatedWithProfile(App.CredManager.GetAccountValue("G_id"));

				lblName.Text = prof.userInfo.userName;

				profImg.Source = ImageSource.FromUri(new Uri(prof.picture_URL));

				lblBio.Text = prof.bio;
				int i = await App.MANAGER.GetActivityCountAsync(App.CredManager.GetAccountValue("G_id"));
				lblStats.Text = "Number of Cookies: " + i.ToString();

				if (Constants.AdminUsers == null)
					Constants.AdminUsers = await App.MANAGER.GetAdminUsers();


				Content = baseStack;

				if (Constants.AdminUsers.Contains(App.CredManager.GetAccountValue("G_id")) && !adminAlert)
				{
					await Task.Delay(5000);
					DisplayAlert("Admin Notice:", "You are logged in as an administrative user!", "Dismiss");
					adminAlert = true;
				}

			}
			else
			{
				await DisplayAlert("Someting went wrong!", "Someting went wrong, please try again!", "Dismiss");
				App.mainPage.Children.RemoveAt(0);
				App.mainPage.Children.Insert(0, new login());
				App.CredManager.DeleteCredentials();
				App.mainPage.SelectedItem = App.mainPage.Children[0];

			}
			App.MANAGER.SetLastActive(App.CredManager.GetAccountValue("G_id"));
		}

		void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
		{
			if (listView.SelectedItem != null)
			{
				var actView = new activityView(listView.SelectedItem as myDataTypes.activity);

				listView.SelectedItem = null;
				Navigation.PushAsync(actView);
			}
		}

		private void CreateListView()
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

			listView.ItemSelected += OnItemSelected;
			listView.IsPullToRefreshEnabled = true;
			listView.Refreshing += (async (sender, eventArgs) =>
			{
				listView.ItemsSource = await App.MANAGER.GetActivitiesAssociatedWithProfile(App.CredManager.GetAccountValue("G_id"));

				int i = await App.MANAGER.GetActivityCountAsync(App.CredManager.GetAccountValue("G_id"));
				lblStats.Text = "Number of cookies: " + i.ToString();

				listView.IsRefreshing = false;
			});
		}
	}
}
