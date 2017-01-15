--drop table Activities
--drop table User_Relationships
--drop table User_Profile



  --CREATE TABLE User_Profile (
  --user_id VARCHAR(255) NOT NULL,
  --name VARCHAR(255) NULL,
  --bio NVARCHAR(255) NULL,
  --last_active DateTime NULL,
  --ADD CONSTRAINT bio  
--DEFAULT 'No Bio' FOR bio ;  
  --date_joined DATETIME NOT NULL,
  --picture_URL VARCHAR(255) NULL,
  --lastUpvote DateTime NULL
  --PRIMARY KEY (user_id),
  --INDEX user_id_UNIQUE  (user_id ASC))

  CREATE TABLE Activities (
  activity_id INT NOT NULL identity(1,1) primary key,
  user_id VARCHAR(255) NOT NULL,
  name NVARCHAR(255) NOT NULL,
  description NVARCHAR(255) NULL,
  date DATETIME NOT NULL,
  upvotes INT NOT NULL,
  picture_URL VARCHAR(255) NULL,
  recipient varchar(255) NULL,
  INDEX user_id_idx (user_id ASC),
  CONSTRAINT user_id
    FOREIGN KEY (user_id)
    REFERENCES cccdb.dbo.User_Profile(user_id)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)

	CREATE TABLE User_Realtionships (
  user_1 VARCHAR(255) NOT NULL,
  user_2 VARCHAR(255) NOT NULL,
  PRIMARY KEY (user_1, user_2),
  INDEX user_2_idx (user_2 ASC),
  CONSTRAINT user_1
    FOREIGN KEY (user_1)
    REFERENCES cccdb.dbo.User_Profile (user_id)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT user_2
    FOREIGN KEY (user_2)
    REFERENCES cccdb.dbo.User_Profile(user_id)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)

	CREATE TABLE cccdb.dbo.Comments (
  activity_id INT NOT NULL,
  user_id VARCHAR(255) NOT NULL,
  comment NVARCHAR(255) NOT NULL,
  date_posted DATETIME NOT NULL,
  PRIMARY KEY (activity_id, user_id),
 -- CONSTRAINT activity_id
    FOREIGN KEY (activity_id)
    REFERENCES cccdb.dbo.Activities (activity_id),
  --CONSTRAINT user_id
    FOREIGN KEY (user_id)
   REFERENCES cccdb.dbo.User_Profile (user_id))


   CREATE TABLE cccdb.dbo.Admin_Users (
  user_id VARCHAR(255) NOT NULL,
  PRIMARY KEY (user_id),
  --CONSTRAINT user_id
    FOREIGN KEY (user_id)
    REFERENCES cccdb.dbo.User_Profile (user_id)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
