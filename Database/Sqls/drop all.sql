use vv_airline;

EXEC sp_MSForEachTable 'DISABLE TRIGGER ALL ON ?'
GO
EXEC sp_MSForEachTable 'ALTER TABLE ? NOCHECK CONSTRAINT ALL'
GO
EXEC sp_MSForEachTable 'SET QUOTED_IDENTIFIER ON; DELETE FROM ?'
GO
EXEC sp_MSForEachTable 'ALTER TABLE ? WITH CHECK CHECK CONSTRAINT ALL'
GO
EXEC sp_MSForEachTable 'ENABLE TRIGGER ALL ON ?'
GO

use mvcapp;
drop database vv_airline;

use vv_airline;
select * from Flights;
select * from Schedules;
select * from FlightSchedule;


select * from Flights;
select * from Schedules;

insert into FlightSchedules
values(6,6);

select * from airports;
select * from Routes;

select * from provinces;