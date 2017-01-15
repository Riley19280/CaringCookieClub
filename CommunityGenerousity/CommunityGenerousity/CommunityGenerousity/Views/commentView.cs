using CaringCookieClub.GUI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

using Xamarin.Forms;

namespace CaringCookieClub.Views
{
	public class commentView : BasePage
	{
		ListView listView;
		StackLayout baseStack;
		int act_id;

		Label titleLabel;

		bool isCommenting = false;

		Editor commentEditor;

		public commentView(int act_id)
		{
			this.act_id = act_id;
			Title = "View Comments";

			createListView();

			titleLabel = new Label
			{
				Text = "Comments",
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),

			};

			commentEditor = new Editor
			{
				//BackgroundColor = Color.FromRgb(64, 64, 64),
				VerticalOptions = LayoutOptions.FillAndExpand,
				HorizontalOptions = LayoutOptions.FillAndExpand
			};


			baseStack = new StackLayout
			{
				Padding = new Thickness(25),
				Children = {
					titleLabel,
					listView
				}
			};

			if (App.CredManager.IsLoggedIn())
			{

				Button btn = null;
				btn = new Button
				{
					Text = "Add a comment",
					Command = new Command(async () =>
					{


						isCommenting = !isCommenting;
						if (isCommenting)
						{
							baseStack.Children.Remove(listView);
							baseStack.Children.Insert(1, commentEditor);
							btn.Text = "Post Comment";
							titleLabel.Text = "Add a Comment";
						}
						else
						{
							//posting comment					
							if (commentEditor.Text.Length > 0)
							{
								await App.MANAGER.AddComment(new myDataTypes.Comment(act_id, App.CredManager.GetAccountValue("G_id"), App.CredManager.GetAccountValue("G_name"), commentEditor.Text, DateTime.Now));
								baseStack.Children.Remove(commentEditor);
								baseStack.Children.Insert(1, listView);
								titleLabel.Text = "View Comments";
								commentEditor.Text = "";
								listView.ItemsSource = await App.MANAGER.GetComments(act_id);
							}
							else
							{
								await DisplayAlert("Enter Comment", "Enter a comment before posting", "OK");
							}

						}


					})
				};

				baseStack.Children.Add(btn);
			}

			Content = baseStack;

		}


		private void createListView()
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
					Label nameLabel = new Label(), commentLabel = new Label();
					nameLabel.SetBinding(Label.TextProperty, "user_name");
					commentLabel.SetBinding(Label.TextProperty, "comment");

					// Return an assembled ViewCell.
					return new ViewCell
					{
						View = new StackLayout
						{
							Padding = new Thickness(5, 5, 5, 0),
							Orientation = StackOrientation.Horizontal,
							Children =
								{
									new StackLayout
									{
										VerticalOptions = LayoutOptions.Center,
										Spacing = 0,
										Children =
										{
											nameLabel,
											commentLabel,
										}
									}
								}
						}
					};
				})
			};

			listView.ItemSelected += OnItemSelected;

		}

		async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
		{
			listView.SelectedItem = null;
		}


		protected async override void OnAppearing()
		{
			base.OnAppearing();
			{
				try
				{
					listView.ItemsSource = await App.MANAGER.GetComments(act_id);
				}
				catch (Exception e)
				{
					Debug.WriteLine(e.Message);
					Debug.WriteLine(e.StackTrace);
				}
			}
		}
	}
}
