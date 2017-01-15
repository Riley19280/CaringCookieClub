using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaringCookieClub
{
	public class manager
	{
		private ICaringCookieClubService CCCService;

		public manager(ICaringCookieClubService service)
		{
			this.CCCService = service;
		}

		public Task<List<myDataTypes.activity>> GetActivitiesAsync()
		{
			return CCCService.GetActivitiesAsync();
		}

		public Task DeleteActivityAsync(int id)
		{
			return CCCService.DeleteActivityAsync(id);
		}

		public Task UpdateOrCreatePostAsync(myDataTypes.activity activity)
		{
			return CCCService.UpdateOrCreatePost(activity);
		}
		public Task UpdateOrCreateProfileAsync(myDataTypes.profile profile)
		{
			return CCCService.UpdateOrCreateProfile(profile);
		}
		public Task<myDataTypes.activity> GetActivityAsync(int id)
		{
			return CCCService.GetActivityAsync(id);
		}
		public Task<myDataTypes.profile> GetProfileAsync(string id)
		{
			return CCCService.GetProfileAsync(id);
		}
		public Task<List<myDataTypes.activity>> GetActivitiesAssociatedWithProfile(string id) {
			return CCCService.GetActivitiesAssociatedWithProfileAsync(id);
		}
		public Task<int> GetActivityCountAsync(string id)
		{
			return CCCService.GetActivityCountAsync(id);
		}
		public Task<bool> GetIsFollowingAsync(string myID, string id)
		{
			return CCCService.GetIsFollowingAsync(myID, id);
		}
		public Task<bool> FollowAsync(string myId, string id) {
			return CCCService.FollowAsync(myId, id);
		}
		public Task<bool> UnFollowAsync(string myId, string id)
		{
			return CCCService.UnFollowAsync(myId, id);
		}
		public Task<List<myDataTypes.activity>> GetActivitysFromFollowersAsync(string id)
		{
			return CCCService.GetActivitysFromFollowersAsync( id);
		}
		public Task<List<myDataTypes.activity>> GetMostPopularActivitiesAsync(int limit)
		{
			return CCCService.GetMostPopularActivitiesAsync(limit);
		}
		public Task<List<myDataTypes.profile>> GetSearchedProfilesAsync(string term)
		{
			return CCCService.GetSearchedProfilesAsync( term);
		}
		public Task<List<string>> GetAdminUsers()
		{
			return CCCService.GetAdminUsers();
		}
		public Task<List<myDataTypes.Comment>> GetComments(int act_id) {
			return CCCService.GetComments(act_id);
		}
		public Task<bool> AddComment(myDataTypes.Comment c)
		{
			return CCCService.AddComment(c);
		}
		public Task<List<myDataTypes.activity>> GetRecieviedActivitys(string prof_id)
		{
			return CCCService.GetRecieviedActivitys(prof_id);
		}
		public Task<bool> SetLastActive(string prof_id)
		{
			return CCCService.SetLastActive(prof_id);
		}
	}
}

