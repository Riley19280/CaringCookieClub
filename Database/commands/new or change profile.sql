if(not exists(select * from cccdb.dbo.User_Profile where user_id = @user_id))
INSERT INTO dbo.User_Profile (user_id,name,bio,date_joined,picture_URL) VALUES (@user_id, @name, @bio, @date_joined, @picture_URL)
else
UPDATE dbo.User_Profile SET name = IsNull(@name, name), bio = IsNull(@bio, bio), picture_URL = IsNull(@picture_URL, picture_URL) WHERE user_id = @user_id