using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

using Xamarin.Forms;

namespace CaringCookieClub.Views
{
	public class editPost : BasePage
	{
		Entry name;
		Editor desc;

		myDataTypes.activity act;

		public editPost(myDataTypes.activity act)
		{
			this.act = act;

			Title = "Edit Post";
			Button updateBtn = new Button
			{
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Text = "Update",
			};
			updateBtn.Clicked += updateBtnClicked;

			name = new Entry
			{
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Placeholder = "Name",
				Text = act.actInfo.actName
			};

			desc = new Editor
			{
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
				Text = act.description
			};

			Content = new StackLayout
			{
				Padding = new Thickness(5, 5, 5, 5),
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Children = {
					new Label {
						HorizontalOptions = LayoutOptions.CenterAndExpand,
						Text = "Edit Post",
						FontSize = Device.GetNamedSize (NamedSize.Large, typeof(Label)),
					},
					name,

					new Label {
						HorizontalOptions = LayoutOptions.FillAndExpand,
						Text = "Description"
					},

					desc,

					updateBtn

				}
			};
		}

		bool posted = false;
		private void updateBtnClicked(object sender, EventArgs e)
		{
			
			if (!posted && name.Text != "" && desc.Text != "")
			{
				myDataTypes.activity acts = new myDataTypes.activity(new myDataTypes.activityInfo(act.actInfo.activity_id, name.Text), act.userInfo, desc.Text, act.upvotes, act.picture_URL, act.datePosted,new myDataTypes.userInfo("",""));
				App.MANAGER.UpdateOrCreatePostAsync(acts);
				posted = true; 
				Navigation.PopAsync();
			}
			else
			{
				DisplayAlert("Empty Fields", "Please add a name and/or description to your post", "OK");
			}



		}
	}
}
