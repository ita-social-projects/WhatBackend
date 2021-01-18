SELECT `s`.`course_id` AS `CourseId`, `l`.`student_group_id` AS `StudentGroupId`, `v`.`student_id` AS `StudentId`, cast(AVG(`v`.`student_mark` ) AS decimal(12,2)) AS `StudentAverageMark`
FROM `visit` AS `v`
LEFT JOIN `lesson` AS `l` ON `v`.`lesson_id` = `l`.`id`
LEFT JOIN `student_group` AS `s` ON `l`.`student_group_id` = `s`.`id`
WHERE (`l`.`student_group_id` IN (2) AND `v`.`student_id` IN (8, 12, 13, 14, 17)) AND `v`.`student_mark` IS NOT NULL
GROUP BY `l`.`student_group_id`, `s`.`course_id`, `v`.`student_id`

SELECT `s`.`course_id` AS `CourseId`, `l`.`student_group_id` AS `StudentGroupId`, `v`.`student_id` AS `StudentId`, avg(`v`.`student_mark`) AS `StudentAverageMark`
FROM `visit` AS `v`
LEFT JOIN `lesson` AS `l` ON `v`.`lesson_id` = `l`.`id`
LEFT JOIN `student_group` AS `s` ON `l`.`student_group_id` = `s`.`id`
-- WHERE (`l`.`student_group_id` IN (2) AND `v`.`student_id` IN (8, 12, 13, 14, 17)) AND `v`.`student_mark` IS NOT NULL
GROUP BY `l`.`student_group_id`, `s`.`course_id`, `v`.`student_id`

SELECT `s`.`course_id` AS `CourseId`, `l`.`student_group_id` AS `StudentGroupId`, `v`.`student_id` AS `StudentId`,SUM(`v`.`student_mark`) /  COUNT(`v`.`student_mark`) AS `StudentAverageMark`
FROM `visit` AS `v`
LEFT JOIN `lesson` AS `l` ON `v`.`lesson_id` = `l`.`id`
LEFT JOIN `student_group` AS `s` ON `l`.`student_group_id` = `s`.`id`
-- WHERE (`l`.`student_group_id` IN (2) AND `v`.`student_id` IN (8, 12, 13, 14, 17)) AND `v`.`student_mark` IS NOT NULL
GROUP BY `l`.`student_group_id`, `s`.`course_id`, `v`.`student_id`

SELECT `s`.`course_id` AS `CourseId`, `l`.`student_group_id` AS `StudentGroupId`, `v`.`student_id` AS `StudentId`, CAST(AVG(CAST(CAST(`v`.`student_mark` AS signed) AS double)) AS double) AS `StudentAverageMark`
FROM `visit` AS `v`
LEFT JOIN `lesson` AS `l` ON `v`.`lesson_id` = `l`.`id`
LEFT JOIN `student_group` AS `s` ON `l`.`student_group_id` = `s`.`id`
WHERE (`l`.`student_group_id` IN (2) AND `v`.`student_id` IN (8, 12, 13, 14, 17)) AND `v`.`student_mark` IS NOT NULLcourse

CAST(SUM(CAST(`v`.`student_mark` AS signed)) AS double) /

SELECT `s`.`course_id` AS `CourseId`, `l`.`student_group_id` AS `StudentGroupId`, `v`.`student_id` AS `StudentId`, AVG(CAST(CAST(`v`.`student_mark` AS signed) AS double)) AS `StudentAverageMark`
FROM `visit` AS `v`
LEFT JOIN `lesson` AS `l` ON `v`.`lesson_id` = `l`.`id`
LEFT JOIN `student_group` AS `s` ON `l`.`student_group_id` = `s`.`id`
WHERE (`l`.`student_group_id` IN (9, 10) AND `v`.`student_id` IN (1, 2, 3)) AND `v`.`student_mark` IS NOT NULL
GROUP BY `l`.`student_group_id`, `s`.`course_id`, `v`.`student_id`