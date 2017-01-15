using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CaringCookieClub
{
	public class myDataTypes
	{
		public class activity
		{
			public activity(activityInfo actInfo, userInfo userInfo, string description, int upvotes, string picture_URL, DateTime datePosted,userInfo recipientInfo)
			{
				this.actInfo = actInfo;
				this.userInfo = userInfo;
				this.description = description;
				this.upvotes = upvotes;
				this.picture_URL = picture_URL;
				this.datePosted = datePosted;
				this.recipientInfo = recipientInfo;
			}
			public activity(activityInfo actInfo)
			{
				this.actInfo = actInfo;
			}

			public activityInfo actInfo { get; set; }
			public userInfo userInfo { get; set; }
			public string description { get; protected set; }
			public int upvotes { get; set; }
			public string picture_URL { get; protected set; }
			public DateTime datePosted { get; protected set; }
			public userInfo recipientInfo { get; protected set; }
		}

		public class activityInfo
		{
			public activityInfo(int activity_id, string actName)
			{
				this.activity_id = activity_id;
				this.actName = actName;
			}

			public string actName { get; protected set; }
			public int activity_id { get; protected set; }
		}
		public class userInfo
		{
			public userInfo(string user_id, string userName)
			{
				this.user_id = user_id;
				this.userName = userName;
			}

			public string userName { get; protected set; }
			public string user_id { get; protected set; }
		}

		public class profile
		{
			public profile(userInfo userInfo, string bio, DateTime date_joined, string picture_URL, DateTime lastUpvote)
			{
				this.userInfo = userInfo;
				this.bio = bio;
				this.date_joined = date_joined;
				this.picture_URL = picture_URL;
				this.lastUpvote = lastUpvote;
			}

			public profile(userInfo userInfo)
			{
				this.userInfo = userInfo;
			}

			public userInfo userInfo { get; protected set; }
			public string bio { get; set; }
			public DateTime date_joined { get; protected set; } = new DateTime(2000, 1, 1);
			public string picture_URL { get; protected set; }
			public DateTime lastUpvote { get; set; } = new DateTime(2000, 1, 1);
		}

		public class Comment
		{
			public Comment(int act_id, string user_id, string user_name, string comment, DateTime date_posted)
			{
				this.act_id = act_id;
				this.user_id = user_id;
				this.user_name = user_name;
				this.comment = comment;
				this.date_posted = date_posted;
			}

			public int act_id { get; protected set; } = 0;

			public string user_id { get; protected set; } = "";
			public string user_name { get; protected set; } = "";
			public string comment { get; protected set; } = "";

			public DateTime date_posted { get; protected set; } = new DateTime();

		}

	}
}
