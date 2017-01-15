using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace CaringCookieClubWCF
{
	// needs to change
	// NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "communityGenerosityService" in code, svc and config file together.
	// NOTE: In order to launch WCF Test Client for testing this service, please select communityGenerosityService.svc or communityGenerosityService.svc.cs at the Solution Explorer and start debugging.
	public class CaringCookieClubService : ICaringCookieClub
	{
		IDatabaseAccess DBACC = new DatabaseAccess();
		
		public bool UpdateOrCreatePost(activityWCF activity)
		{
			if (activity == null)
				return false;

			return DBACC.UpdateOrCreateActivity(activity);
		}

		public bool DeleteActivity(int activity_id)
		{
			DBACC.DeleteActivity(activity_id);
			//TODO: return?
			return true;
		}

		public activityWCF GetActivity(int activity_id)
		{
			return DBACC.GetActivity(activity_id);
		}

		public List<activityWCF> GetActivitys()
		{
			return DBACC.GetActivities(10);
			//return activities;
		}

		public profileWCF GetProfile(string user_id)
		{
			return DBACC.GetProfile(user_id);
		}

		public bool UpdateOrCreateProfile(profileWCF profile)
		{
			return DBACC.UpdateOrCreateProfile(profile);
		}

		public List<activityWCF> GetActivitysAssociatedWithProfile(string id) {
			return DBACC.GetActivitysAssociatedWithProfile(id);
		}

		public int GetActivityCount(string id)
		{
			return DBACC.GetActivityCount(id);
		}

		public bool GetIsFollowing(string myId, string id)
		{
			return DBACC.GetIsFollowing(myId, id);
		}

		public bool Follow(string myID, string id)
		{
			return DBACC.Follow(myID, id);
		}

		public bool UnFollow(string myID, string id)
		{
			return DBACC.UnFollow(myID, id);
		}

		public List<activityWCF> GetActivitysFromFollowers(string id)
		{
			return DBACC.GetActivitysFromFollowers(id);
		}

		public List<activityWCF> GetMostPopularActivities(int limit)
		{
			return DBACC.GetMostPopularActivities(limit);
		}

		public List<profileWCF> GetSearchedProfiles(string term)
		{
			return DBACC.GetSearchedProfiles(term);
		}

		public List<string> GetAdmins()
		{
			return DBACC.GetAdminUsers();
		}

		public List<commentWCF> GetComments(int act_id)
		{
			return DBACC.GetComments(act_id);
		}

		public bool AddComment(commentWCF comment) {
			return DBACC.AddComment(comment);
		}

		public List<activityWCF> GetRecieviedActivitys(string prof_id)
		{
			return DBACC.GetRecieviedActivitys(prof_id);
		}

		public bool SetLastActive(string prof_id)
		{
			return DBACC.SetLastActive(prof_id);
		}
	}
}
