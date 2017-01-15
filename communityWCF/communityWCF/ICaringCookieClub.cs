using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace CaringCookieClubWCF
{
	// NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
	[ServiceContract]
	public interface ICaringCookieClub
	{
		[OperationContract]
		activityWCF GetActivity(int activity_id);

		[OperationContract]
		profileWCF GetProfile(string user_id);

		[OperationContract]
		bool DeleteActivity(int activity_id);

		[OperationContract]
		List<activityWCF> GetActivitys();

		[OperationContract]
		bool UpdateOrCreateProfile(profileWCF profile);

		[OperationContract]
		bool UpdateOrCreatePost(activityWCF activity);

		[OperationContract]
		List<activityWCF> GetActivitysAssociatedWithProfile(string id);

		[OperationContract]
		int GetActivityCount(string id);

		[OperationContract]
		bool GetIsFollowing(string myId,string id);

		[OperationContract]
		bool Follow(string myID, string id);

		[OperationContract]
		bool UnFollow(string myID, string id);

		[OperationContract]
		List<activityWCF> GetActivitysFromFollowers(string id);

		[OperationContract]
		List<activityWCF> GetMostPopularActivities(int limit);

		[OperationContract]
		List<profileWCF> GetSearchedProfiles(string term);
		
		[OperationContract]
		List<string> GetAdmins();

		[OperationContract]
		List<commentWCF> GetComments(int act_id);

		[OperationContract]
		bool AddComment(commentWCF comment);

		[OperationContract]
		List<activityWCF> GetRecieviedActivitys(string prof_id);

		[OperationContract]
		bool SetLastActive(string prof_id);
	}



}

