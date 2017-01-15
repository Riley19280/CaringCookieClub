using CaringCookieClub.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

using Xamarin.Forms;

namespace CaringCookieClub.GUI
{
	public class activityView : BasePage
	{
		Button visitProf;
		Button upvote;

		public activityView(myDataTypes.activity act)
		{
			Title = act.actInfo.actName;

			Label description = new Label
			{
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
				Text = act.description,
			};


			visitProf = new Button
			{
				Text = "Visit Profile",
				HorizontalOptions = LayoutOptions.EndAndExpand,
				VerticalOptions = LayoutOptions.CenterAndExpand,

				Command = new Command(() =>
				{
					Navigation.PushAsync(new othersProfile(act.userInfo.user_id));

				})
			};

			upvote = new Button
			{
				Text = "Upvote",
				HorizontalOptions = LayoutOptions.EndAndExpand,
				VerticalOptions = LayoutOptions.CenterAndExpand,
				Command = new Command(() =>
				{
					upvote.IsEnabled = false;
					myDataTypes.activity upvotea = new myDataTypes.activity(act.actInfo);
					upvotea.upvotes += act.upvotes + 1;
					act.upvotes++;//updatign client side
					App.MANAGER.UpdateOrCreatePostAsync(upvotea);

					myDataTypes.profile p = new myDataTypes.profile(new myDataTypes.userInfo(App.CredManager.GetAccountValue("G_id"), App.CredManager.GetAccountValue("G_name")));
					p.lastUpvote = DateTime.Now;
					App.CredManager.UpdateAccountValue("lastUpvote", p.lastUpvote);
					App.MANAGER.UpdateOrCreateProfileAsync(p);
					DisplayAlert("Upvoted!", "You can upvote every " + Constants.UpvoteInterval + " hours", "OK");
				})
			};

			#region Edit Delete stack
			StackLayout editDeleteStack = new StackLayout
			{
				Orientation = StackOrientation.Horizontal,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.End,
				Children = {
							new Button
							{
								HorizontalOptions = LayoutOptions.FillAndExpand,
								Text = "Delete Cookie",
								Command = new Command (async()=>{
									if(!await DisplayAlert("Are you sure?","Are you sure you want to delete this post?","No","Yes")) {
										App.MANAGER.DeleteActivityAsync(act.actInfo.activity_id);
										Navigation.PopAsync();
									}
								})

							},
							new Button
							{
								HorizontalOptions = LayoutOptions.FillAndExpand,
								Text = "Edit Cookie",
								Command = new Command(()=> {
									Navigation.PushAsync(new editPost(act));
								})
							}
						}
			};

			#endregion

			//checking upvote time 


			if (!App.CredManager.IsLoggedIn())
			{
				upvote.IsEnabled = false;
			}
			else if (!(App.CredManager.GetAccountDate("lastUpvote").AddHours(Constants.UpvoteInterval) < DateTime.Now))
			{
				upvote.IsEnabled = false;
			}

			StackLayout mainStack = new StackLayout
			{
				Padding = new Thickness(10),
				Children = {
					new StackLayout {
						Orientation = StackOrientation.Horizontal,
						VerticalOptions = LayoutOptions.Start,
						Children = {
							new Label {
								Text = "By: " + act.userInfo.userName,
								HorizontalOptions = LayoutOptions.StartAndExpand,
									VerticalOptions = LayoutOptions.CenterAndExpand,
							},
							visitProf
						}
					},
					new StackLayout {
						Orientation = StackOrientation.Horizontal,
						VerticalOptions = LayoutOptions.Start,
						Children = {
							new Label {
								Text = "Posted: " + act.datePosted,
								HorizontalOptions = LayoutOptions.StartAndExpand,
								VerticalOptions = LayoutOptions.CenterAndExpand,
							},
							upvote
						}
					},
					new Label {
						Text = string.IsNullOrWhiteSpace(act.recipientInfo.userName) ? "Recipient: None" : "Recipient: "+ act.recipientInfo.userName
					},
					description,
				}
			};


			#region handling the edit/delete stack
			if (App.CredManager.IsLoggedIn())
				if (act.userInfo.user_id == App.CredManager.GetAccountValue("G_id") || Constants.AdminUsers.Contains(App.CredManager.GetAccountValue("G_id")))
				{
					bool added = false;
					if (act.userInfo.user_id == App.CredManager.GetAccountValue("G_id"))
					{
						visitProf.IsEnabled = false;
						upvote.IsEnabled = false;
						mainStack.Children.Add(editDeleteStack);
						added = true;
					}

					if (Constants.AdminUsers.Contains(App.CredManager.GetAccountValue("G_id")))
					{
						if (act.userInfo.user_id == App.CredManager.GetAccountValue("G_id"))
						{
							visitProf.IsEnabled = false;
							upvote.IsEnabled = false;
						}
						if (!added)
							mainStack.Children.Add(editDeleteStack);
					}

					mainStack.Children.Add(editDeleteStack);
				}
			#endregion

			//adding comments button

			mainStack.Children.Add(new Button
			{
				Text = "View Comments",
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.End,
				Command = new Command(() =>
				{
					Navigation.PushAsync(new commentView(act.actInfo.activity_id));
				})
			});

			Content = mainStack;
		}


		public void disableVisitButton()
		{
			visitProf.IsEnabled = false;
		}
	}
}
