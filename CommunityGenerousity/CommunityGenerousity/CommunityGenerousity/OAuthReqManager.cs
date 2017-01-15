using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Auth;

using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Json;

namespace CaringCookieClub
{
	public class OAuthReqManager
	{
		public async Task<bool> GetProfileInfo()
		{
			try
			{

				var request = new OAuth2Request("GET", new Uri(Constants.UserInfoURL), null, App.CredManager.GetCredentials());

				var response = await request.GetResponseAsync();
				if (response != null)
				{
					string userJson = response.GetResponseText();

					using (Stream s = GenerateStreamFromString(userJson))
					{
						DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(GoogleInfo));
						GoogleInfo gi = (GoogleInfo)ser.ReadObject(s);
						if (gi != null)
						{
							App.CredManager.SetAccountValue("G_id", gi.id);
							App.CredManager.SetAccountValue("G_name", gi.name);
							App.CredManager.SetAccountValue("G_picture", gi.picture);
						}
						else {
							return false;
						}
						return true;
					}

				}
				else
				{

					return false;
				}

			}
			catch (Exception ex)
			{
				throw new Exception("Failed saving google profile info\n\n\n" + ex.Message + "\n\n\n" + ex.StackTrace);
			}

		}

		Stream GenerateStreamFromString(string s)
		{
			MemoryStream stream = new MemoryStream();
			StreamWriter writer = new StreamWriter(stream);
			writer.Write(s);
			writer.Flush();
			stream.Position = 0;
			return stream;
		}
	}
}
