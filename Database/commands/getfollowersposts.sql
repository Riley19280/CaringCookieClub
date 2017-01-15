


select * from cccdb.dbo.Activities where user_id in (
select user_2 from cccdb.dbo.User_Realtionships where user_1 = '104743196180865273164'
)
order by date desc