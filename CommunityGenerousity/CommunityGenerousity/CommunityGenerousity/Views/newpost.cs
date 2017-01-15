using CaringCookieClub.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

using Xamarin.Forms;

namespace CaringCookieClub.GUI
{
	public class newpost : BasePage
	{
		Entry name;
		Editor desc;

		Label recipientLabel;

		myDataTypes.profile recipientprof;

		StackLayout baseStack;

		public newpost()
		{
			Title = "New Cookie";

			Button postBtn = new Button
			{
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Text = "Send cookie!",
			};
			postBtn.Clicked += postBtnClicked;

			name = new Entry
			{
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Placeholder = "Cookie Name"
			};

			desc = new Editor
			{
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand

			};

			recipientLabel = new Label {
				Text = "Cookie Recipient: None "
			};

			baseStack = new StackLayout
			{
				Padding = new Thickness(25),
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Children = {
					name,

					new Label {
						HorizontalOptions = LayoutOptions.FillAndExpand,
						Text = "Cookie Description"
					},

					desc,
					recipientLabel,

					new Button
					{

						HorizontalOptions = LayoutOptions.FillAndExpand,
						Text = "Add recipient",
						Command = new Command(()=> {
							Navigation.PushAsync(new selectUser(this));						
						}),
					},

					postBtn

				}
			};

			Content = baseStack;

		}

		bool posted = false;
		private void postBtnClicked(object sender, EventArgs e)
		{

			//App.ORM.GetProfileInfo(App.CredManager.GetCredentials());
			if (!posted && !string.IsNullOrWhiteSpace(name.Text) && !string.IsNullOrWhiteSpace(desc.Text))
			{
				myDataTypes.activity act = new myDataTypes.activity(new myDataTypes.activityInfo(-1, name.Text), new myDataTypes.userInfo(App.CredManager.GetAccountValue("G_id"), App.CredManager.GetAccountValue("G_name")), desc.Text, 0, App.CredManager.GetAccountValue("G_picture"), DateTime.Now, recipientprof == null ? new myDataTypes.userInfo("","") : new myDataTypes.userInfo(recipientprof.userInfo.user_id, recipientprof.userInfo.userName));
				App.MANAGER.UpdateOrCreatePostAsync(act);
				posted = true;
				Navigation.PopAsync();
			}
			else
			{
				DisplayAlert("Empty Fields", "Please add a name and/or description to your cookie", "OK");
			}

		}

		public void doRecipientView(myDataTypes.profile recipientprof)
		{
			this.recipientprof = recipientprof;
			recipientLabel.Text = "Cookie Recipient: " + recipientprof.userInfo.userName;
		}

		StackLayout cell(myDataTypes.profile p)
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
			return new StackLayout
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
			};
		}
	}
}
