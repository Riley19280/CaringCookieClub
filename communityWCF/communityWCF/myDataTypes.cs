using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CaringCookieClubWCF
{
	[DataContract]
	public class activityWCF
	{
		public activityWCF(int activity_id, string actName, string user_id, string user_name, string description,int upvotes, string picture_URL,DateTime datePosted,string recipient_id, string recipient_name)
		{
			this.activity_id = activity_id;
			this.actName = actName;
			this.user_id = user_id;
			this.user_name = user_name;
			this.description = description;
			this.upvotes = upvotes;
			this.picture_URL = picture_URL;
			this.datePosted = datePosted;
			this.recipient_id = recipient_id;
			this.recipient_name = recipient_name;

		}
		[DataMember]
		public int activity_id { get; protected set; } = -1;
		[DataMember]
		public string actName { get; protected set; } = "";
		[DataMember]
		public string user_id { get; protected set; } = "";
		[DataMember]
		public string user_name { get; protected set; } = "";
		[DataMember]
		public string description { get; protected set; } = "";
		[DataMember]
		public int upvotes { get; protected set; } = 0;
		[DataMember]
		public string picture_URL { get; protected set; } = "";
		[DataMember]
		public DateTime datePosted { get; protected set; }
		[DataMember]
		public string recipient_id { get; protected set; }
		[DataMember]
		public string recipient_name { get; protected set; }

	}

	[DataContract]
	public class profileWCF
	{
		public profileWCF(string user_id, string user_name, string bio, DateTime date_joined, string picture_URL,DateTime lastUpvote)
		{
			this.user_id = user_id;
			this.user_name = user_name;
			this.bio = bio;
			this.date_joined = date_joined;
			this.picture_URL = picture_URL;
			this.lastUpvote = lastUpvote;
		}
		[DataMember]
		public string user_id { get; protected set; } = "";
		[DataMember]
		public string user_name { get; protected set; } = "";
		[DataMember]
		public string bio { get; protected set; } = "";
		[DataMember]
		public DateTime date_joined { get; protected set; } = new DateTime();
		[DataMember]
		public string picture_URL { get; protected set; } = "";
		[DataMember]
		public DateTime lastUpvote { get; protected set; } = DateTime.MinValue;
	}

	[DataContract]
	public class commentWCF
	{
		public commentWCF(int act_id, string user_id,string user_name, string comment, DateTime date_posted)
		{
			this.act_id = act_id;
			this.user_id = user_id;
			this.user_name = user_name;
			this.comment = comment;
			this.date_posted = date_posted;

		}
		[DataMember]
		public int act_id { get; protected set; } = 0;
		[DataMember]
		public string user_id { get; protected set; } = "";
		[DataMember]
		public string user_name { get; protected set; } = "";
		[DataMember]
		public string comment { get; protected set; } = "";
		[DataMember]
		public DateTime date_posted { get; protected set; } = new DateTime();
	}

}

