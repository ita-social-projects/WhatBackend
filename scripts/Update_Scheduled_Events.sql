UPDATE 
     scheduledevents t1
SET 
	t1.LessonID = (Select ID from lessons t3 WHERE t1.ThemeID = t3.ThemeID and t1.LessonID != t3.ID and t1.EventFinish < CURTIME() limit 1),
    t1.MentorID = (Select MentorID from lessons t3 WHERE t1.ThemeID = t3.ThemeID and t1.EventFinish < CURTIME() limit 1),
    t1.StudentGroup = (Select StudentGroup from lessons t3 WHERE t1.ThemeID = t3.ThemeID and t1.EventFinish < CURTIME() limit 1),
    t1.EventStart = (Select LessonDate from lessons t3 WHERE t1.ThemeID = t3.ThemeID and t1.EventFinish < CURTIME() limit 1),
    t1.EventFinish = (Select ID LessonDate from lessons t3 WHERE t1.ThemeID = t3.ThemeID and t1.EventFinish < CURTIME() limit 1) + INTERVAL 1 HOUR;