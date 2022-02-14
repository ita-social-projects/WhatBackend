USE `Soft`;
insert into eventoccurrences
select null as ID, StudentGroup, EventStart, EventFinish from scheduledevents as t1
where t1.EventFinish < CURTIME();