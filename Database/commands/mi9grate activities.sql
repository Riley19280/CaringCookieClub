INSERT INTO cccdb.dbo.Activities
		(user_id,name,description,date,upvotes,picture_URL) 
		SELECT user_id,name,description,date,upvotes,picture_URL FROM cccdb.dbo.backupA


--SELECT * INTO cccdb.dbo.User_ProfileB FROM cccdb.dbo.User_Profile

--DRop table User_Profile

--  CREATE TABLE User_Profile (
--  user_id VARCHAR(255) NOT NULL,
--  name VARCHAR(255) NULL,
--  bio NVARCHAR(255) NULL,
--  date_joined DATETIME NOT NULL,
--  picture_URL VARCHAR(255) NULL,
--  lastUpvote DateTime NULL
--  PRIMARY KEY (user_id),
--  INDEX user_id_UNIQUE  (user_id ASC))

--INSERT INTO cccdb.dbo.User_Profile
--		([user_id],[name],[bio],[date_joined],[picture_URL],[lastUpvote]) 
--		SELECT [user_id],[name],[bio],[date_joined],[picture_URL],[lastUpvote] FROM cccdb.dbo.User_ProfileB
