using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CaringCookieClub.CCCServiceReference;
using System.ServiceModel;
using System.Diagnostics;
using System.Collections.ObjectModel;

namespace CaringCookieClub
{
	public interface ICaringCookieClubService
	{
		Task<myDataTypes.activity> GetActivityAsync(int id);
		Task<myDataTypes.profile> GetProfileAsync(string id);
		Task DeleteActivityAsync(int id);
		Task UpdateOrCreatePost(myDataTypes.activity a);
		Task UpdateOrCreateProfile(myDataTypes.profile p);
		Task<List<myDataTypes.activity>> GetActivitiesAsync();
		Task<List<myDataTypes.activity>> GetActivitiesAssociatedWithProfileAsync(string id);
		Task<int> GetActivityCountAsync(string id);
		Task<bool> GetIsFollowingAsync(string myId, string id);
		Task<bool> FollowAsync(string myId, string id);
		Task<bool> UnFollowAsync(string myId, string id);
		Task<List<myDataTypes.activity>> GetActivitysFromFollowersAsync(string id);
		Task<List<myDataTypes.activity>> GetMostPopularActivitiesAsync(int limit);
		Task<List<myDataTypes.profile>> GetSearchedProfilesAsync(string term);
		Task<List<string>> GetAdminUsers();
		Task<List<myDataTypes.Comment>> GetComments(int act_id);
		Task<bool> AddComment(myDataTypes.Comment comment);
		Task<List<myDataTypes.activity>> GetRecieviedActivitys(string prof_id);
		Task<bool> SetLastActive(string prof_id);
	}



	public class CaringCookieClubService : ICaringCookieClubService
	{
		ICaringCookieClub service;

		public CaringCookieClubService()
		{
			try
			{
				service = new CaringCookieClubClient(
				new BasicHttpBinding(),
				new EndpointAddress(Constants.WCFURL));
			}
			catch (Exception e)
			{
				Debug.WriteLine(e.Message);
				Debug.WriteLine(e.StackTrace);

			}
		}

		#region To and From
		static myDataTypes.activity FromWCFService(activityWCF item)
		{
			return new myDataTypes.activity(new myDataTypes.activityInfo(item.activity_id, item.actName), new myDataTypes.userInfo(item.user_id, item.user_name), item.description, item.upvotes, item.picture_URL, item.datePosted,new myDataTypes.userInfo(item.recipient_id,item.recipient_name));
		}

		static myDataTypes.profile FromWCFService(profileWCF item)
		{
			return new myDataTypes.profile(new myDataTypes.userInfo(item.user_id, item.user_name), item.bio, item.date_joined, item.picture_URL, item.lastUpvote);
		}

		static activityWCF ToWCFService(myDataTypes.activity item)
		{
			activityWCF ac = new activityWCF();


			ac.activity_id = item.actInfo.activity_id;
			ac.actName = item.actInfo.actName ?? "";
			if (item.userInfo != null)
			{
				ac.user_id = item.userInfo.user_id ?? "";
				ac.user_name = item.userInfo.userName ?? "";
			}
			ac.description = item.description ?? "";
			ac.upvotes = item.upvotes;
			ac.picture_URL = item.picture_URL ?? "";
			ac.recipient_id = item.recipientInfo.user_id ?? "";
			ac.recipient_name = item.recipientInfo.userName ?? "";

			return ac;
		}

		static profileWCF ToWCFService(myDataTypes.profile item)
		{
			return new profileWCF
			{
				user_id = item.userInfo.user_id,
				user_name = item.userInfo.userName,
				bio = item.bio,
				date_joined = item.date_joined,
				picture_URL = item.picture_URL,
				lastUpvote = item.lastUpvote
			};
		}

		static myDataTypes.Comment FromWCFService(commentWCF item)
		{
			return new myDataTypes.Comment(item.act_id,item.user_id,item.user_name,item.comment,item.date_posted);
		}

		static commentWCF ToWCFService(myDataTypes.Comment item)
		{
			commentWCF c = new commentWCF();
			c.act_id = item.act_id;
			c.user_id = item.user_id;
			c.user_name = item.user_name;
			c.comment = item.comment ?? "";
			c.date_posted = item.date_posted;

			return c;
		}


		#endregion

		public async Task UpdateOrCreatePost(myDataTypes.activity a)
		{
			try
			{
				activityWCF activity = ToWCFService(a);

				await Task.Factory.FromAsync(service.BeginUpdateOrCreatePost, service.EndUpdateOrCreatePost, activity, TaskCreationOptions.None);

			}
			catch (FaultException fe)
			{
				Debug.WriteLine(@"			{0}", fe.Message);
			}
			catch (Exception ex)
			{
				Debug.WriteLine(@"				ERROR {0} \n {1}", ex.Message, ex.StackTrace);
			}
		}

		public async Task UpdateOrCreateProfile(myDataTypes.profile p)
		{
			try
			{
				profileWCF prof = ToWCFService(p);

				await Task.Factory.FromAsync(service.BeginUpdateOrCreateProfile, service.EndUpdateOrCreateProfile, prof, TaskCreationOptions.None);

			}
			catch (FaultException fe)
			{
				Debug.WriteLine(@"			{0}", fe.Message);
			}
			catch (Exception ex)
			{
				Debug.WriteLine(@"				ERROR {0} \n {1}", ex.Message, ex.StackTrace);
			}
		}

		public async Task DeleteActivityAsync(int id)
		{
			myDataTypes.profile prof = null;
			try
			{

				var item = await Task.Factory.FromAsync(service.BeginDeleteActivity, service.EndDeleteActivity, id, TaskCreationOptions.None);

			}
			catch (FaultException fe)
			{
				Debug.WriteLine(@"			{0}", fe.Message);
			}
			catch (Exception ex)
			{
				Debug.WriteLine(@"				ERROR {0} \n {1}", ex.Message, ex.StackTrace);
			}
		}

		public async Task<List<myDataTypes.activity>> GetActivitiesAsync()
		{
			List<myDataTypes.activity> Items = new List<myDataTypes.activity>();

			try
			{
				var todoItems = await Task.Factory.FromAsync(service.BeginGetActivitys, service.EndGetActivitys, null, TaskCreationOptions.None);

				foreach (var item in todoItems)
				{
					if (item != null)
						Items.Add(FromWCFService(item));
					else
					{
						Debug.WriteLine("NULL ITEM");
					}
				}
			}
			catch (FaultException fe)
			{
				Debug.WriteLine(@"			{0} \n {1} \n {2}", fe.Message, fe.Reason, fe.StackTrace);
			}
			catch (Exception ex)
			{
				Debug.WriteLine(@"				ERROR {0} \n {1}", ex.Message, ex.StackTrace);
			}

			return Items;
		}

		public async Task<myDataTypes.activity> GetActivityAsync(int id)
		{
			myDataTypes.activity act = null;
			try
			{

				var item = await Task.Factory.FromAsync(service.BeginGetActivity, service.EndGetActivity, id, TaskCreationOptions.None);


				act = FromWCFService(item);

			}
			catch (FaultException fe)
			{
				Debug.WriteLine(@"			{0}", fe.Message);
			}
			catch (Exception ex)
			{
				Debug.WriteLine(@"				ERROR {0} \n {1}", ex.Message, ex.StackTrace);
			}
			return act;
		}

		public async Task<myDataTypes.profile> GetProfileAsync(string id)
		{
			//FIXME: this returns null if the server thros an error
			myDataTypes.profile prof = null;
			try
			{

				var item = await Task.Factory.FromAsync(service.BeginGetProfile, service.EndGetProfile, id, TaskCreationOptions.None);

				prof = FromWCFService(item);

			}
			catch (FaultException fe)
			{
				Debug.WriteLine(@"			{0}", fe.Message);
			}
			catch (Exception ex)
			{
				Debug.WriteLine(@"				ERROR {0} \n {1}", ex.Message, ex.StackTrace);
			}
			return prof;
		}

		public async Task<List<myDataTypes.activity>> GetActivitiesAssociatedWithProfileAsync(string id)
		{
			List<myDataTypes.activity> Items = new List<myDataTypes.activity>();

			try
			{
				var todoItems = await Task.Factory.FromAsync(service.BeginGetActivitysAssociatedWithProfile, service.EndGetActivitysAssociatedWithProfile, id, TaskCreationOptions.None);

				foreach (var item in todoItems)
				{
					if (item != null)
						Items.Add(FromWCFService(item));
					else
					{
						Debug.WriteLine("NULL ITEM");
					}
				}
			}
			catch (FaultException fe)
			{
				Debug.WriteLine(@"			{0} \n {1} \n {2}", fe.Message, fe.Reason, fe.StackTrace);
			}
			catch (Exception ex)
			{
				Debug.WriteLine(@"				ERROR {0} \n {1}", ex.Message, ex.StackTrace);
			}

			return Items;
		}

		public async Task<int> GetActivityCountAsync(string id)
		{
			try
			{
				return await Task.Factory.FromAsync(service.BeginGetActivityCount, service.EndGetActivityCount, id, TaskCreationOptions.None);
			}
			catch (FaultException fe)
			{
				Debug.WriteLine(@"			{0}", fe.Message);
			}
			catch (Exception ex)
			{
				Debug.WriteLine(@"				ERROR {0} \n {1}", ex.Message, ex.StackTrace);
			}
			return 0;
		}

		public async Task<bool> GetIsFollowingAsync(string myId, string id)
		{
			try
			{
				return await Task.Factory.FromAsync(service.BeginGetIsFollowing, service.EndGetIsFollowing, myId, id, TaskCreationOptions.None);
			}
			catch (FaultException fe)
			{
				Debug.WriteLine(@"			{0}", fe.Message);
			}
			catch (Exception ex)
			{
				Debug.WriteLine(@"				ERROR {0} \n {1}", ex.Message, ex.StackTrace);
			}
			return false;
		}

		public async Task<bool> FollowAsync(string myId, string id)
		{
			try
			{
				return await Task.Factory.FromAsync(service.BeginFollow, service.EndFollow, myId, id, TaskCreationOptions.None);
			}
			catch (FaultException fe)
			{
				Debug.WriteLine(@"			{0}", fe.Message);
			}
			catch (Exception ex)
			{
				Debug.WriteLine(@"				ERROR {0} \n {1}", ex.Message, ex.StackTrace);
			}
			return false;
		}

		public async Task<bool> UnFollowAsync(string myId, string id)
		{
			try
			{
				return await Task.Factory.FromAsync(service.BeginUnFollow, service.EndUnFollow, myId, id, TaskCreationOptions.None);
			}
			catch (FaultException fe)
			{
				Debug.WriteLine(@"			{0}", fe.Message);
			}
			catch (Exception ex)
			{
				Debug.WriteLine(@"				ERROR {0} \n {1}", ex.Message, ex.StackTrace);
			}
			return false;
		}

		public async Task<List<myDataTypes.activity>> GetActivitysFromFollowersAsync(string id)
		{
			List<myDataTypes.activity> Items = new List<myDataTypes.activity>();

			try
			{
				var todoItems = await Task.Factory.FromAsync(service.BeginGetActivitysFromFollowers, service.EndGetActivitysFromFollowers, id, TaskCreationOptions.None);

				foreach (var item in todoItems)
				{
					if (item != null)
						Items.Add(FromWCFService(item));
					else
					{
						Debug.WriteLine("NULL ITEM");
					}
				}
			}
			catch (FaultException fe)
			{
				Debug.WriteLine(@"			{0} \n {1} \n {2}", fe.Message, fe.Reason, fe.StackTrace);
			}
			catch (Exception ex)
			{
				Debug.WriteLine(@"				ERROR {0} \n {1}", ex.Message, ex.StackTrace);
			}

			return Items;
		}

		public async Task<List<myDataTypes.activity>> GetMostPopularActivitiesAsync(int limit)
		{
			List<myDataTypes.activity> Items = new List<myDataTypes.activity>();

			try
			{
				var todoItems = await Task.Factory.FromAsync(service.BeginGetMostPopularActivities, service.EndGetMostPopularActivities, limit, TaskCreationOptions.None);

				foreach (var item in todoItems)
				{
					if (item != null)
						Items.Add(FromWCFService(item));
					else
					{
						Debug.WriteLine("NULL ITEM");
					}
				}
			}
			catch (FaultException fe)
			{
				Debug.WriteLine(@"			{0} \n {1} \n {2}", fe.Message, fe.Reason, fe.StackTrace);
			}
			catch (Exception ex)
			{
				Debug.WriteLine(@"				ERROR {0} \n {1}", ex.Message, ex.StackTrace);
			}

			return Items;
		}

		public async Task<List<myDataTypes.profile>> GetSearchedProfilesAsync(string term)
		{
			List<myDataTypes.profile> Items = new List<myDataTypes.profile>();

			try
			{
				var todoItems = await Task.Factory.FromAsync(service.BeginGetSearchedProfiles, service.EndGetSearchedProfiles, term, TaskCreationOptions.None);

				foreach (var item in todoItems)
				{
					if (item != null)
						Items.Add(FromWCFService(item));
					else
					{
						Debug.WriteLine("NULL ITEM");
					}
				}
			}
			catch (FaultException fe)
			{
				Debug.WriteLine(@"			{0} \n {1} \n {2}", fe.Message, fe.Reason, fe.StackTrace);
			}
			catch (Exception ex)
			{
				Debug.WriteLine(@"				ERROR {0} \n {1}", ex.Message, ex.StackTrace);
			}

			return Items;
		}

		public async Task<List<string>> GetAdminUsers()
		{
			List<string> Items = new List<string>();

			try
			{
				var todoItems = await Task.Factory.FromAsync(service.BeginGetAdmins, service.EndGetAdmins, null, TaskCreationOptions.None);

				foreach (var item in todoItems)
				{
					if (item != null)
						Items.Add(item);
				}
			}
			catch (FaultException fe)
			{
				Debug.WriteLine(@"			{0} \n {1} \n {2}", fe.Message, fe.Reason, fe.StackTrace);
			}
			catch (Exception ex)
			{
				Debug.WriteLine(@"				ERROR {0} \n {1}", ex.Message, ex.StackTrace);
			}

			return Items;
		}

		public async Task<List<myDataTypes.Comment>> GetComments(int act_id)
		{
			List<myDataTypes.Comment> Items = new List<myDataTypes.Comment>();

			try
			{
				var todoItems = await Task.Factory.FromAsync(service.BeginGetComments, service.EndGetComments, act_id, TaskCreationOptions.None);

				foreach (var item in todoItems)
				{
					if (item != null)
						Items.Add(FromWCFService(item));
					else
					{
						Debug.WriteLine("NULL ITEM");
					}
				}
			}
			catch (FaultException fe)
			{
				Debug.WriteLine(@"			{0} \n {1} \n {2}", fe.Message, fe.Reason, fe.StackTrace);
			}
			catch (Exception ex)
			{
				Debug.WriteLine(@"				ERROR {0} \n {1}", ex.Message, ex.StackTrace);
			}

			return Items;
		}

		public async Task<bool> AddComment(myDataTypes.Comment comment)
		{
			try
			{
				return await Task.Factory.FromAsync(service.BeginAddComment, service.EndAddComment,ToWCFService(comment), TaskCreationOptions.None);
			}
			catch (FaultException fe)
			{
				Debug.WriteLine(@"			{0}", fe.Message);
			}
			catch (Exception ex)
			{
				Debug.WriteLine(@"				ERROR {0} \n {1}", ex.Message, ex.StackTrace);
			}
			return false;
		}

		public async Task<List<myDataTypes.activity>> GetRecieviedActivitys(string prof_id)
		{
			List<myDataTypes.activity> Items = new List<myDataTypes.activity>();

			try
			{
				var todoItems = await Task.Factory.FromAsync(service.BeginGetRecieviedActivitys, service.EndGetRecieviedActivitys, prof_id, TaskCreationOptions.None);

				foreach (var item in todoItems)
				{
					if (item != null)
						Items.Add(FromWCFService(item));
					else
					{
						Debug.WriteLine("NULL ITEM");
					}
				}
			}
			catch (FaultException fe)
			{
				Debug.WriteLine(@"			{0} \n {1} \n {2}", fe.Message, fe.Reason, fe.StackTrace);
			}
			catch (Exception ex)
			{
				Debug.WriteLine(@"				ERROR {0} \n {1}", ex.Message, ex.StackTrace);
			}

			return Items;
		}

		public async Task<bool> SetLastActive(string prof_id)
		{
			try
			{
				return await Task.Factory.FromAsync(service.BeginSetLastActive, service.EndSetLastActive, prof_id, TaskCreationOptions.None);
			}
			catch (FaultException fe)
			{
				Debug.WriteLine(@"			{0}", fe.Message);
			}
			catch (Exception ex)
			{
				Debug.WriteLine(@"				ERROR {0} \n {1}", ex.Message, ex.StackTrace);
			}
			return false;
		}
	}
}

