using CaringCookieClub;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

using Xamarin.Forms;


public class BasePage : ContentPage
{
	public BasePage()
	{
			Style = (Style)Application.Current.Resources["contentPageStyle"];	
	}
}

