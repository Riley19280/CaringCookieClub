using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace CaringCookieClubWCF
{
	public interface IDatabaseAccess
	{
		bool UpdateOrCreateActivity(activityWCF act);
		bool UpdateOrCreateProfile(profileWCF prof);
		List<activityWCF> GetActivities(int limit);
		List<activityWCF> GetActivitysAssociatedWithProfile(string id);
		profileWCF GetProfile(string id);
		activityWCF GetActivity(int id);
		bool DeleteActivity(int id);
		int GetActivityCount(string id);
		bool GetIsFollowing(string myID, string id);
		bool Follow(string myID, string id);
		bool UnFollow(string myID, string id);
		List<activityWCF> GetActivitysFromFollowers(string id);
		List<activityWCF> GetMostPopularActivities(int limit);
		List<profileWCF> GetSearchedProfiles(string term);
		List<commentWCF> GetComments(int act_id);
		bool AddComment(commentWCF comment);
		List<activityWCF> GetRecieviedActivitys(string prof_id);
		bool SetLastActive(string prof_id);
		List<string> GetAdminUsers();
	}

	public class DatabaseAccess : IDatabaseAccess
	{
		public bool UpdateOrCreateActivity(activityWCF act)
		{
			int activity_id = act.activity_id;
			string user_id = act.user_id;
			string name = act.actName;
			string description = act.description;
			int upvotes = act.upvotes;
			string picture_URL = act.picture_URL;
			string recipient = act.recipient_id;
			int affected = 0;

			using (SqlConnection connection = new SqlConnection(Constants.SQLConnectionString))

				if (activity_id == -1 || activity_id == 0)
				{
					using (SqlCommand cmd = new SqlCommand("INSERT INTO dbo.activities (user_id,name,description,upvotes,picture_URL,date,recipient) VALUES(@user_id, @name, @description,@upvotes, @picture_URL,@date,@recipient)", connection))
					{
						cmd.Parameters.AddWithValue("@user_id", user_id);
						cmd.Parameters.AddWithValue("@name", name);
						cmd.Parameters.AddWithValue("@description", description);
						cmd.Parameters.AddWithValue("@picture_URL", picture_URL);
						cmd.Parameters.AddWithValue("@upvotes", 0);
						cmd.Parameters.AddWithValue("@date", DateTime.Now);
						cmd.Parameters.AddWithValue("@recipient", recipient);
						connection.Open();
						affected = cmd.ExecuteNonQuery();
					}
				}
				else
				{
					//TODO: values included in this sql statment
					//the update part
					using (SqlCommand cmd = new SqlCommand("UPDATE dbo.activities SET name = COALESCE(@name, name), description = COALESCE(@description, description), upvotes = COALESCE(@upvotes, upvotes), picture_URL = COALESCE(@picture_URL, picture_URL), recipient = COALESCE(@recipient, recipient) WHERE activity_id = @activity_id", connection))
					{

						//cmd.Parameters.AddWithValue("@user_id", user_id);
						cmd.Parameters.AddWithValue("@name", string.IsNullOrWhiteSpace(name) ? (object)DBNull.Value : name);
						cmd.Parameters.AddWithValue("@description", string.IsNullOrWhiteSpace(description) ? (object)DBNull.Value : description);
						cmd.Parameters.AddWithValue("@picture_URL", string.IsNullOrWhiteSpace(picture_URL) ? (object)DBNull.Value : picture_URL);
						cmd.Parameters.AddWithValue("@upvotes", upvotes == 0 ? (object)DBNull.Value : upvotes);
						cmd.Parameters.AddWithValue("@recipient", string.IsNullOrWhiteSpace(recipient) ? (object)DBNull.Value : recipient);
						cmd.Parameters.AddWithValue("@activity_id", activity_id);


						connection.Open();
						affected = cmd.ExecuteNonQuery();

					}
				}
			if (affected > 0)
				return true;
			else
				return false;

		}

		public bool UpdateOrCreateProfile(profileWCF prof)
		{

			string user_id = prof.user_id;
			string name = prof.user_name;
			string bio = prof.bio;
			DateTime date_joined = prof.date_joined;
			string picture_URL = prof.picture_URL;
			DateTime lastUpvote = prof.lastUpvote;

			int affected = 0;

			using (SqlConnection connection = new SqlConnection(Constants.SQLConnectionString))
			{
				if (!string.IsNullOrWhiteSpace(user_id))
				{
					using (SqlCommand cmd = new SqlCommand("if(not exists(select * from cccdb.dbo.User_Profile where user_id = @user_id)) INSERT INTO dbo.User_Profile (user_id,name,bio,date_joined,picture_URL,lastUpvote) VALUES (@user_id, @name, 'No Bio',@date_joined, @picture_URL,'2000-01-01 00:00:00.000') else UPDATE dbo.User_Profile SET name = IsNull(@name, name), bio = IsNull(@bio, bio), picture_URL = IsNull(@picture_URL, picture_URL), lastUpvote = IsNull(@lastUpvote, lastUpvote) WHERE user_id = @user_id", connection))
					{
						cmd.Parameters.AddWithValue("@user_id", string.IsNullOrWhiteSpace(user_id) ? (object)DBNull.Value : user_id);
						cmd.Parameters.AddWithValue("@name", string.IsNullOrWhiteSpace(name) ? (object)DBNull.Value : name);
						cmd.Parameters.AddWithValue("@bio", string.IsNullOrWhiteSpace(bio) ? (object)DBNull.Value : bio);
						cmd.Parameters.AddWithValue("@date_joined", date_joined == null ? (object)DBNull.Value : date_joined);
						cmd.Parameters.AddWithValue("@picture_URL", string.IsNullOrWhiteSpace(picture_URL) ? (object)DBNull.Value : picture_URL);
						cmd.Parameters.AddWithValue("@lastUpvote", lastUpvote == new DateTime(2000, 1, 1) ? (object)DBNull.Value : lastUpvote);
						connection.Open();
						affected = cmd.ExecuteNonQuery();

					}
				}
			}

			return affected > 0 ? true : false;

		}

		public List<activityWCF> GetActivities(int limit)
		{
			//TODO:Check all values for nulls
			List<activityWCF> activities = new List<activityWCF>();

			using (SqlConnection connection = new SqlConnection(Constants.SQLConnectionString))
			{

				using (SqlCommand cmd = new SqlCommand("SELECT TOP (@limit) * from dbo.Activities ORDER BY date DESC", connection))
				{
					cmd.Parameters.AddWithValue("limit", limit);
					connection.Open();
					using (SqlDataReader reader = cmd.ExecuteReader())
					{
						// Check is the reader has any rows at all before starting to read.
						if (reader.HasRows)
						{
							// Read advances to the next row.
							//TODO: chekc fvalues for null
							while (reader.Read())
							{
								activityWCF a = new activityWCF(
									 reader.GetInt32(reader.GetOrdinal("activity_id")),
									 reader.GetString(reader.GetOrdinal("name")),
									 reader.GetString(reader.GetOrdinal("user_id")),
									 ResolveUserName(reader.GetString(reader.GetOrdinal("user_id"))),
									 reader.GetString(reader.GetOrdinal("description")),
									 reader.GetInt32(reader.GetOrdinal("upvotes")),
							  		 reader.GetString(reader.GetOrdinal("picture_URL")),
									 reader.GetDateTime(reader.GetOrdinal("date")),
									 reader.GetString(reader.GetOrdinal("recipient")) ?? "",
									 ResolveUserName(reader.GetString(reader.GetOrdinal("recipient")))
									);

								activities.Add(a);
							}
						}
					}
				}
			}
			return activities;
		}

		public profileWCF GetProfile(string id)
		{
			using (SqlConnection connection = new SqlConnection(Constants.SQLConnectionString))
			{

				using (SqlCommand cmd = new SqlCommand("SELECT * from dbo.User_Profile WHERE user_id = @user_id", connection))
				{
					cmd.Parameters.AddWithValue("user_id", id);
					connection.Open();
					using (SqlDataReader reader = cmd.ExecuteReader())
					{
						// Check is the reader has any rows at all before starting to read.
						if (reader.HasRows)
						{
							// Read advances to the next row.
							//TODO: chekc fvalues for null
							while (reader.Read())
							{
								profileWCF p = new profileWCF(
									 reader.GetString(reader.GetOrdinal("user_id")),
									 reader.GetString(reader.GetOrdinal("name")),
									 reader.GetString(reader.GetOrdinal("bio")),
									 reader.GetDateTime(reader.GetOrdinal("date_joined")),
							  		 reader.GetString(reader.GetOrdinal("picture_URL")),
									 reader.GetDateTime(reader.GetOrdinal("lastUpvote"))
									);

								return p;
							}
						}
					}
				}
			}
			return null;
		}

		public activityWCF GetActivity(int id)
		{
			using (SqlConnection connection = new SqlConnection(Constants.SQLConnectionString))
			{

				using (SqlCommand cmd = new SqlCommand("SELECT * from dbo.Activities WHERE activity_id = @activity_id", connection))
				{
					cmd.Parameters.AddWithValue("activity_id", id);
					connection.Open();
					using (SqlDataReader reader = cmd.ExecuteReader())
					{
						// Check is the reader has any rows at all before starting to read.
						if (reader.HasRows)
						{
							// Read advances to the next row.
							//TODO: chekc fvalues for null
							while (reader.Read())
							{
								activityWCF a = new activityWCF(
									 reader.GetInt32(reader.GetOrdinal("activity_id")),
									 reader.GetString(reader.GetOrdinal("name")),
									 reader.GetString(reader.GetOrdinal("user_id")),
									 ResolveUserName(reader.GetString(reader.GetOrdinal("user_id"))),
									 reader.GetString(reader.GetOrdinal("description")),
									 reader.GetInt32(reader.GetOrdinal("upvotes")),
							  		 reader.GetString(reader.GetOrdinal("picture_URL")),
									 reader.GetDateTime(reader.GetOrdinal("date")),
									 reader.GetString(reader.GetOrdinal("recipient")) ?? "",
									 ResolveUserName(reader.GetString(reader.GetOrdinal("recipient")))
									);

								return a;
							}
						}
					}
				}
			}
			return null;
		}

		public List<activityWCF> GetActivitysAssociatedWithProfile(string id)
		{
			List<activityWCF> activities = new List<activityWCF>();

			using (SqlConnection connection = new SqlConnection(Constants.SQLConnectionString))
			{

				using (SqlCommand cmd = new SqlCommand("SELECT * from dbo.Activities WHERE user_id = @user_id ORDER BY date DESC", connection))
				{
					cmd.Parameters.AddWithValue("user_id", id);
					connection.Open();
					using (SqlDataReader reader = cmd.ExecuteReader())
					{
						// Check is the reader has any rows at all before starting to read.
						if (reader.HasRows)
						{
							// Read advances to the next row.
							//TODO: chekc fvalues for null
							while (reader.Read())
							{
								activityWCF a = new activityWCF(
									 reader.GetInt32(reader.GetOrdinal("activity_id")),
									 reader.GetString(reader.GetOrdinal("name")),
									 reader.GetString(reader.GetOrdinal("user_id")),
									 ResolveUserName(reader.GetString(reader.GetOrdinal("user_id"))),
									 reader.GetString(reader.GetOrdinal("description")),
									 reader.GetInt32(reader.GetOrdinal("upvotes")),
							  		 reader.GetString(reader.GetOrdinal("picture_URL")),
									 reader.GetDateTime(reader.GetOrdinal("date")),
									 reader.GetString(reader.GetOrdinal("recipient")) ?? "",
									 ResolveUserName(reader.GetString(reader.GetOrdinal("recipient")))
									);

								activities.Add(a);
							}
						}
					}
				}
			}
			return activities;
		}

		public bool DeleteActivity(int id)
		{

			using (SqlConnection connection = new SqlConnection(Constants.SQLConnectionString))
			{

				using (SqlCommand cmd = new SqlCommand("DELETE from dbo.Activities WHERE activity_id = @activity_id", connection))
				{
					cmd.Parameters.AddWithValue("activity_id", id);
					connection.Open();
					int affected = cmd.ExecuteNonQuery();
					return affected > 0 ? true : false;
				}
			}

		}

		public int GetActivityCount(string id)
		{
			using (SqlConnection connection = new SqlConnection(Constants.SQLConnectionString))
			{

				using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM dbo.Activities WHERE user_id = @user_id", connection))
				{
					cmd.Parameters.AddWithValue("user_id", id);
					connection.Open();
					return (Int32)cmd.ExecuteScalar();

				}
			}

		}

		public bool Follow(string myID, string id)
		{
			using (SqlConnection connection = new SqlConnection(Constants.SQLConnectionString))
			{

				using (SqlCommand cmd = new SqlCommand("INSERT INTO dbo.User_Realtionships (user_1,user_2) VALUES (@user_1, @user_2)", connection))
				{
					cmd.Parameters.AddWithValue("user_1", myID);
					cmd.Parameters.AddWithValue("user_2", id);
					connection.Open();
					return (int)cmd.ExecuteNonQuery() > 0 ? true : false;
				}
			}
		}

		public bool UnFollow(string myID, string id)
		{
			using (SqlConnection connection = new SqlConnection(Constants.SQLConnectionString))
			{

				using (SqlCommand cmd = new SqlCommand("DELETE from dbo.User_Realtionships WHERE user_1 = @user_1 AND user_2 = @user_2", connection))
				{
					cmd.Parameters.AddWithValue("user_1", myID);
					cmd.Parameters.AddWithValue("user_2", id);
					connection.Open();
					return (int)cmd.ExecuteNonQuery() > 0 ? true : false;
				}
			}
		}

		public bool GetIsFollowing(string myID, string id)
		{
			using (SqlConnection connection = new SqlConnection(Constants.SQLConnectionString))
			{

				using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM dbo.User_Realtionships WHERE user_1 = @user_1 AND user_2 = @user_2", connection))
				{
					cmd.Parameters.AddWithValue("user_1", myID);
					cmd.Parameters.AddWithValue("user_2", id);
					connection.Open();
					return (int)cmd.ExecuteScalar() > 0 ? true : false;
				}
			}
		}

		internal string ResolveUserName(string id)
		{
			using (SqlConnection connection = new SqlConnection(Constants.SQLConnectionString))
			{

				using (SqlCommand cmd = new SqlCommand("select name from dbo.User_Profile where user_id = @user_id", connection))
				{
					cmd.Parameters.AddWithValue("user_id", id);
					connection.Open();
					string s = (string)cmd.ExecuteScalar();
					return s;

				}
			}
		}

		public List<activityWCF> GetActivitysFromFollowers(string id)
		{
			List<activityWCF> activities = new List<activityWCF>();

			using (SqlConnection connection = new SqlConnection(Constants.SQLConnectionString))
			{

				using (SqlCommand cmd = new SqlCommand("SELECT * FROM cccdb.dbo.Activities WHERE user_id IN (SELECT user_2 FROM cccdb.dbo.User_Realtionships WHERE user_1 = @user_1) ORDER BY date DESC", connection))
				{
					cmd.Parameters.AddWithValue("user_1", id);
					connection.Open();
					using (SqlDataReader reader = cmd.ExecuteReader())
					{
						// Check is the reader has any rows at all before starting to read.
						if (reader.HasRows)
						{
							// Read advances to the next row.
							//TODO: chekc fvalues for null
							while (reader.Read())
							{
								activityWCF a = new activityWCF(
									 reader.GetInt32(reader.GetOrdinal("activity_id")),
									 reader.GetString(reader.GetOrdinal("name")),
									 reader.GetString(reader.GetOrdinal("user_id")),
									 ResolveUserName(reader.GetString(reader.GetOrdinal("user_id"))),
									 reader.GetString(reader.GetOrdinal("description")),
									 reader.GetInt32(reader.GetOrdinal("upvotes")),
							  		 reader.GetString(reader.GetOrdinal("picture_URL")),
									 reader.GetDateTime(reader.GetOrdinal("date")),
									 reader.GetString(reader.GetOrdinal("recipient")) ?? "",
									 ResolveUserName(reader.GetString(reader.GetOrdinal("recipient")))
									);

								activities.Add(a);
							}
						}
					}
				}
			}
			return activities;
		}

		public List<activityWCF> GetMostPopularActivities(int limit)
		{
			//TODO:Check all values for nulls
			List<activityWCF> activities = new List<activityWCF>();

			using (SqlConnection connection = new SqlConnection(Constants.SQLConnectionString))
			{

				using (SqlCommand cmd = new SqlCommand("SELECT TOP (@limit) * from dbo.Activities ORDER BY upvotes DESC, date DESC", connection))
				{
					cmd.Parameters.AddWithValue("limit", limit);
					connection.Open();
					using (SqlDataReader reader = cmd.ExecuteReader())
					{
						// Check is the reader has any rows at all before starting to read.
						if (reader.HasRows)
						{
							// Read advances to the next row.
							//TODO: chekc fvalues for null
							while (reader.Read())
							{
								activityWCF a = new activityWCF(
									 reader.GetInt32(reader.GetOrdinal("activity_id")),
									 reader.GetString(reader.GetOrdinal("name")),
									 reader.GetString(reader.GetOrdinal("user_id")),
									 ResolveUserName(reader.GetString(reader.GetOrdinal("user_id"))),
									 reader.GetString(reader.GetOrdinal("description")),
									 reader.GetInt32(reader.GetOrdinal("upvotes")),
							  		 reader.GetString(reader.GetOrdinal("picture_URL")),
									 reader.GetDateTime(reader.GetOrdinal("date")),
									 reader.GetString(reader.GetOrdinal("recipient")) ?? "",
									 ResolveUserName(reader.GetString(reader.GetOrdinal("recipient")))
									);

								activities.Add(a);
							}
						}
					}
				}
			}
			return activities;
		}

		public List<profileWCF> GetSearchedProfiles(string term)
		{
			List<profileWCF> profs = new List<profileWCF>();

			using (SqlConnection connection = new SqlConnection(Constants.SQLConnectionString))
			{

				using (SqlCommand cmd = new SqlCommand("SELECT TOP (@limit) * FROM dbo.User_Profile WHERE name LIKE @term ORDER BY name, date_Joined DESC", connection))
				{
					cmd.Parameters.AddWithValue("limit", 25);
					cmd.Parameters.AddWithValue("term", "%" + term + "%");
					connection.Open();
					using (SqlDataReader reader = cmd.ExecuteReader())
					{
						// Check is the reader has any rows at all before starting to read.
						if (reader.HasRows)
						{
							// Read advances to the next row.
							//TODO: chekc fvalues for null
							while (reader.Read())
							{
								profileWCF a = new profileWCF(
									 reader.GetString(reader.GetOrdinal("user_id")),
									 reader.GetString(reader.GetOrdinal("name")),
									 reader.GetString(reader.GetOrdinal("bio")),
									 reader.GetDateTime(reader.GetOrdinal("date_joined")),
									 reader.GetString(reader.GetOrdinal("picture_URL")),
									 reader.GetDateTime(reader.GetOrdinal("lastUpvote"))
									);

								profs.Add(a);
							}
						}
					}
				}
			}
			return profs;
		}

		public List<commentWCF> GetComments(int act_id)
		{
			//TODO:Check all values for nulls
			List<commentWCF> comments = new List<commentWCF>();

			using (SqlConnection connection = new SqlConnection(Constants.SQLConnectionString))
			{

				using (SqlCommand cmd = new SqlCommand("SELECT * from dbo.Comments where activity_id = @activity_id ORDER BY date_posted DESC", connection))
				{
					cmd.Parameters.AddWithValue("activity_id", act_id);
					connection.Open();
					using (SqlDataReader reader = cmd.ExecuteReader())
					{
						// Check is the reader has any rows at all before starting to read.
						if (reader.HasRows)
						{
							// Read advances to the next row.
							//TODO: chekc fvalues for null
							while (reader.Read())
							{
								commentWCF a = new commentWCF(
									 reader.GetInt32(reader.GetOrdinal("activity_id")),
									 reader.GetString(reader.GetOrdinal("user_id")),
									 ResolveUserName(reader.GetString(reader.GetOrdinal("user_id"))),
									 reader.GetString(reader.GetOrdinal("comment")),
									 reader.GetDateTime(reader.GetOrdinal("date_posted"))
									);

								comments.Add(a);
							}
						}
					}
				}
			}
			if (comments.Count == 0)
			{
				comments.Add(new commentWCF(0, "", "", "No Comments to display.", DateTime.Now));
			}

			return comments;
		}

		public bool AddComment(commentWCF comment)
		{
			int activity_id = comment.act_id;
			string user_id = comment.user_id;
			string comments = comment.comment;
			DateTime date_posted = comment.date_posted;

			int affected = 0;

			using (SqlConnection connection = new SqlConnection(Constants.SQLConnectionString))
			{
				using (SqlCommand cmd = new SqlCommand("INSERT INTO dbo.Comments (activity_id,user_id,comment,date_posted) VALUES(@activity_id, @user_id, @comment,@date_posted)", connection))
				{
					cmd.Parameters.AddWithValue("@activity_id", activity_id);
					cmd.Parameters.AddWithValue("@user_id", user_id);
					cmd.Parameters.AddWithValue("@comment", comments);
					cmd.Parameters.AddWithValue("@date_posted", DateTime.Now);

					connection.Open();
					affected = cmd.ExecuteNonQuery();
				}
			}

			if (affected > 0)
				return true;
			else
				return false;

		}

		public List<activityWCF> GetRecieviedActivitys(string prof_id)
		{
			//TODO:Check all values for nulls
			List<activityWCF> activities = new List<activityWCF>();

			using (SqlConnection connection = new SqlConnection(Constants.SQLConnectionString))
			{

				using (SqlCommand cmd = new SqlCommand("SELECT * from dbo.Activities WHERE recipient = @prof_id ORDER BY date DESC", connection))
				{
					cmd.Parameters.AddWithValue("prof_id", prof_id);
					connection.Open();
					using (SqlDataReader reader = cmd.ExecuteReader())
					{
						// Check is the reader has any rows at all before starting to read.
						if (reader.HasRows)
						{
							// Read advances to the next row.
							//TODO: chekc fvalues for null
							while (reader.Read())
							{
								activityWCF a = new activityWCF(
									 reader.GetInt32(reader.GetOrdinal("activity_id")),
									 reader.GetString(reader.GetOrdinal("name")),
									 reader.GetString(reader.GetOrdinal("user_id")),
									 ResolveUserName(reader.GetString(reader.GetOrdinal("user_id"))),
									 reader.GetString(reader.GetOrdinal("description")),
									 reader.GetInt32(reader.GetOrdinal("upvotes")),
							  		 reader.GetString(reader.GetOrdinal("picture_URL")),
									 reader.GetDateTime(reader.GetOrdinal("date")),
									 reader.GetString(reader.GetOrdinal("recipient")) ?? "",
									 ResolveUserName(reader.GetString(reader.GetOrdinal("recipient")))
									);

								activities.Add(a);
							}
						}
					}
				}
			}

			//if (activities.Count == 0)
			//{
			//	activities.Add(new activityWCF(0, "No recieved Cookies", "SERVER", "", "", 0, "http://masterriley.com/images/ccclogo.png", DateTime.Now, "",""));
			//}
			return activities;
		}

		public bool SetLastActive(string prof_id)
		{
			using (SqlConnection connection = new SqlConnection(Constants.SQLConnectionString))
			{

				using (SqlCommand cmd = new SqlCommand("UPDATE dbo.User_Profile SET last_active = @last_active WHERE user_id = @prof_id", connection))
				{
					cmd.Parameters.AddWithValue("prof_id", prof_id);
					cmd.Parameters.AddWithValue("last_active", DateTime.Now);
					connection.Open();
					return (int)cmd.ExecuteNonQuery() > 0 ? true : false;
				}
			}
		}

		public List<string> GetAdminUsers()
		{
			List<string> users = new List<string>();

			using (SqlConnection connection = new SqlConnection(Constants.SQLConnectionString))
			{
				using (SqlCommand cmd = new SqlCommand("SELECT * FROM cccdb.dbo.Admin_Users", connection))
				{
					connection.Open();
					using (SqlDataReader reader = cmd.ExecuteReader())
					{
						// Check is the reader has any rows at all before starting to read.
						if (reader.HasRows)
						{
							while (reader.Read())
							{
								string a = reader.GetString(reader.GetOrdinal("user_id"));
								users.Add(a);
							}
						}
					}
				}
			}
			return users;
		}
	}

}