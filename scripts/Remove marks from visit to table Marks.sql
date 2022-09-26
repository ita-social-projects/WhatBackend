
alter table marks add flag int;

insert into marks (Value, Comment, EvaluationDate , Type, EvaluatedBy, flag)
select visits.StudentMark ," ",LessonDate,'0', 1,  visits.ID
from visits
JOIN lessons ON lessons.ID = lessons.ThemeID;

ALTER TABLE visits MODIFY StudentMark BIGINT;
ALTER TABLE visits DROP Check CH_MarkVisits ;

update visits 
JOIN marks ON marks.flag = visits.ID
set visits.StudentMark =  marks.ID ;

ALTER TABLE visits RENAME COLUMN StudentMark TO StudentMarkID;
ALTER TABLE marks DROP COLUMN flag ;
