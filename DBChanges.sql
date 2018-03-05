EXEC sys.sp_cdc_enable_db  
GO ;



select * from RoomType;

Insert Into Roomtype
values('201', 'Premium',0);

USE Hotel  
GO  
EXEC sys.sp_cdc_enable_table  
@source_schema = N'dbo',  
@source_name   = N'Roomtype',  
@role_name     = NULL  
  
GO 

Select is_cdc_enabled,* from sys.databases

select is_tracked_by_cdc,name from sys.tables;

SELECT *   FROM [Hotel].[cdc].[dbo_roomtype_CT]
select * from [cdc].[lsn_time_mapping];

select * from msdb.dbo.cdc_jobs;

Insert Into Roomtype
values('206', 'Premium',0);

execute sp_MScdc_capture_job;

SELECT *   FROM [Hotel].[cdc].[dbo_roomtype_CT];

select * from [cdc].[lsn_time_mapping];

SELECT * FROM RoomType where Roomname='201'

select roomType.RoomName, roomType.RoomType from cdc.dbo_roomtype_CT roomType,cdc.lsn_time_mapping timing
where roomType.Isreplicated = 0
and roomType.__$operation = 2
and roomType.__$start_lsn = timing.start_lsn
order by timing.tran_end_time, timing.tran_begin_time





