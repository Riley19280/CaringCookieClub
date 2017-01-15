using CaringCookieClub.GUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

using Xamarin.Forms;

namespace CaringCookieClub.Views
{
	public class searchUsers : BasePage
	{
		ListView listView;
		Entry search;

		public searchUsers()
		{
			Title = "Search";

			search = new Entry
			{
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Placeholder = "Search"
			};

			Button btn = new Button
			{
				HorizontalOptions = LayoutOptions.End,
				Text = "GO",
				Command = new Command(async () =>
				{
					if (!string.IsNullOrWhiteSpace(search.Text))
					{
						List<myDataTypes.profile> profs = await App.MANAGER.GetSearchedProfilesAsync(search.Text);
						string id = App.CredManager.GetAccountValue("G_id");
						foreach (var item in profs)
						{
							if (item.userInfo.user_id == id)
							{
								profs.Remove(item);
								break;
							}
						}
						listView.ItemsSource = profs;

					}
					else
						await DisplayAlert("Enter name", "Enter a name to search for", "OK");
				})
			};

			CreateListView();

			Content = new StackLayout
			{
				Children = {
					new StackLayout {
						Padding = new Thickness(10),
						Orientation= StackOrientation.Horizontal,
						HorizontalOptions = LayoutOptions.FillAndExpand,
						Children = {
							search,
							btn
						}
					},
					listView
				}
			};
		}

		private void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
		{
			myDataTypes.profile p = e.SelectedItem as myDataTypes.profile;

			//FIXME: change this to not retrieve the profile again

			Navigation.PushAsync(new othersProfile(p.userInfo.user_id));
		}

		private void CreateListView()
		{

			listView = new ListView
			{
				// Source of data items.
				VerticalOptions = LayoutOptions.FillAndExpand,
				HorizontalOptions = LayoutOptions.FillAndExpand,

				RowHeight = 75,
				// Define template for displaying each item.
				// (Argument of DataTemplate constructor is called for 
				//      each item; it must return a Cell derivative.)
				ItemTemplate = new DataTemplate(() =>
				{
					// Create views with bindings for displaying each property.
					Label nameLabel = new Label { FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)) }, bioLabel = new Label();
					nameLabel.SetBinding(Label.TextProperty, "userInfo.userName");
					bioLabel.SetBinding(Label.TextProperty, "bio");

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
										//VerticalOptions = LayoutOptions.Center,
										Spacing = 5,
										Children =
										{
											nameLabel,
											bioLabel,
										}
									}
								}
						}
					};
				})
			};

			listView.ItemSelected += OnItemSelected;
		}

	}
}
