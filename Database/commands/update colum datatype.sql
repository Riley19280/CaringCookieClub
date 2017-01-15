alter table cccdb.dbo.User_Profile
add bio2 NVARCHAR(255)

UPDATE cccdb.dbo.User_Profile
SET bio2 = bio

alter table cccdb.dbo.User_Profile
drop column bio 
alter table cccdb.dbo.User_Profile
add bio NVARCHAR(255)
UPDATE cccdb.dbo.User_Profile
set bio = bio2
