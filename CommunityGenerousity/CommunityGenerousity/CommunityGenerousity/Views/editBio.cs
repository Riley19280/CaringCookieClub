using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CaringCookieClub.Views
{
	public class editBio : BasePage
	{
		public delegate void EditBioHandler(object myObject, EditBioArgs myArgs);
		public event EditBioHandler OnEditBio;

		Editor edit;
		public editBio()
		{
			Title = "Edit Bio";
			Button finish = new Button
			{
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Text = "Update Bio",
			};
			finish.Clicked += postBtnClicked;

			edit = new Editor
			{
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand

			};

			Content = new StackLayout
			{
				Padding = new Thickness(5, 5, 5, 5),
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Children = {
					new Label {
						HorizontalOptions = LayoutOptions.CenterAndExpand,
						Text = "New Bio:",
						FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label))
					},

					edit,

					finish

				}
			};
		}

		bool finished = false;
		private void postBtnClicked(object sender, EventArgs e)
		{
			if (!finished)
			{
				myDataTypes.profile act = new myDataTypes.profile(new myDataTypes.userInfo(App.CredManager.GetAccountValue("G_id"), App.CredManager.GetAccountValue("G_name")));
				act.bio = edit.Text;
				App.MANAGER.UpdateOrCreateProfileAsync(act);
				finished = true;
				OnEditBio(this, new EditBioArgs(act.bio));
				Navigation.PopAsync();

			}
		}


	}


	public class EditBioArgs : EventArgs
	{
		private string newBio;

		public EditBioArgs(string newBio)
		{
			this.newBio = newBio;
		}

		// This is a straightforward implementation for 
		// declaring a public field
		public string Bio
		{
			get
			{
				return newBio;
			}
		}
	}

}
