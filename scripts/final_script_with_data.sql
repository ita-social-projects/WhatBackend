use soft;
insert  account(role, first_name, last_name, email, password, salt, is_active)
VALUES 
(4,'admin','admin','admin.@gmail.com','8977ac989dc476f2c02431358834c00dda1128923339659f143fbc758dba47d2','ruPUCpyh1mDvutrQoiUk',true),
(3,'Жанна','Дарк','Janna.@gmail.com','eacbdc5e75850e7d07ac8504883c3de9a7225e93310417d079d53681d8a75eaa','AP4zYvi5JyR1t6NWbk1a',true),
(3,'Клара','Цеткин','Tsetkina.@gmail.com','59ab6d0386bf6d4c41cb54561abf1b3b35a512e4f749d714fc11eb33613fd00b','AP4zYvi5JyR1t6NWbk1b',true),
(2,'Фродо','Бэггинс','Frodo.@gmail.com','a7fb1ad7840034a97564c1a834a0f95cbf376912e9e5ee48c1142ce8ef62676e','AP4zYvi5JyR1t6NWbk1R',true),
(2,'Леголас','Зелёный Лист','Lego.@gmail.com','19ba92491c216c84693a0a0f3309ec2785e8cb010c45a1c513b7608332a11a97','xgqjh8yPx6QvGjRRdhyQ',true),
(2,'Торин','Дубощит','Thorin.@gmail.com','265c8d045e4debf7dc52586d8dcf33609825693bdffa7362102d49d7828b28cf','N0CLFg7Ts8CUFqN3wkvd',true),

(1,'Сергей','Мордор','serg.Mor@gmail.com','de6462d2bfc578327152278c298fcec9598265d15d423ebed6d7981a71f92d4d','wqfKvbJaBsYqRh7jVtQt',true),
(1,'Егор','Чародей','Egor.@gmail.com','b97ad424aa93766ab97a7b9d5ee7e25b74c052139e6f7434c7486bf36d529712','5t5mFqxaLzpcacJ7x8on',true),
(1,'Дмитрий','Devil','Devil.@gmail.com','6a0adca70bae18b92a55fd663483c268990ab86ff94012472948f7be8b13768e','FsDexnYG6uJp78Et4sXd',true),
(1,'Liza','Klymenko','Klymenko.@gmail.com','a244b8814de3250ecc406e39cdc7ed2e21f52600d45ed098d368aad608409c65','m3VM8tTUzPMd3LcUKRdn',true),
(1,'Голлум','Смéагол ','Goll.@gmail.com','903a9ceb5d34245eefd388b490d18e9f2cb4dad7337600ae9ca6f99d5ae230f0','LjvYkDTqWNQ4vP0qqpud',true),

(1,'Гюнтер','о’Дим','God.@gmail.com','5dcafa351b64375e41f63f3efbcb19b553380837796427c1f3599af0b1d32f6c','bhk4KfVdXJM28A2TGeGd',true),
(1,'Кейра','Мец','Keira.@gmail.com','1817ab380941708ed5b5e153685f147a37f36026127134c710f7f66bc2ff1876','Z45GC6Rn8EDcsXXtCiVa',true),
(1,'Трисс','Меригольд','Triss.@gmail.com','b5f2143ce4a27109711ba8d65920281b47dbceb811c98b974a15a27443046fff','YMvGWFQ0q38L8P7ZXCpJ',true),
(1,'Вернон','Роше','Vernon.@gmail.com','f18d409f970a766f242ba0e789d6e9e51e5c629794cce6411ec6c96456c907c4','YQTyrEKEE28WCktWdMPQ',true),
(1,'Эредин','Бреакк Глас','Eredin.@gmail.com','d5d88a32b44cb1a47f9cf823a198b78100ad3ad90775b36bb338eca1ad2590c7','V5XeFuFBXDoQss1pn3oT',true),

(1,'Хагрид','Рубеус','Hagrid.@gmail.com','2136d4f71fbd73c9eda40ee9262c6a9b4efc7895d7c297e8d62b05bad3ca0a47','LcJmgsMD886diEM99w7v',true),
(1,'Полумна','Лавгуд','Luna.@gmail.com','a329a64ab7c623089d1947a5a0ebce04463cfaaf131874b4c4ede03f3aab3002','eZA4zvb4EBQgMY2rW6rX',true),
(1,'Минерва','Макгонагалл','McGonagall.@gmail.com','5298ba2960215a4aeecbbaeae3949a2b0a840410a20006c52f27edf6a94bca42','wmYwW02uijgPe2FzR2Xs',true),
(1,'Чжоу','Чанг','Chang.@gmail.com','20b31894a220f9e9c2bee58f9628fffcfb32ee3ad480fc6da0032bc952642e53','9DHgFtVENMLeGAdg7hqU',true),
(1,'Северус','Снегг','Snape.@gmail.com','17dea7e51732eaa28cc487b62454e329aa94cc853caec1b850128774e2bf767d','P9cj0dAyeX2o4xFGMUzL',true);

insert into student(account_id)
(select account.id from account where  account.role = 1 );

insert into mentor(account_id)
(select account.id from account where  account.role = 2 );

insert into course(name)
value ('Веб дизайн'), ('разработка на Java'),('создание игр на Unity'), ('создание веб приложений');

insert into theme(name)/*Ok*/
value ('Создание макета'), ('ООП в Java'),('создание простых скриптов на Unity'), ('разработка API');

insert student_group (course_id, name, start_date, finish_date)
values
((select id from course where course.name = 'Веб дизайн'),'121_18_1', '2018-09-01',  '2022-09-01'),
((select id from course where course.name = 'разработка на Java'),'122_17_1', '2019-09-01',  '2022-09-01'),
((select id from course where course.name = 'создание игр на Unity'),'122_18_ck1', '2018-09-01',  '2021-09-01'),
((select id from course where course.name = 'создание веб приложений'),'121_18_ck1', '2018-09-01',  '2021-09-01');

insert student_of_student_group (student_group_id, student_id)
values
((SELECT student_group.id FROM student_group where student_group.name = "121_18_1" ),
(SELECT student.id FROM student where account_id =  (select id from account where first_name = "Сергей"))),
((SELECT student_group.id FROM student_group where student_group.name = "121_18_1" ),
(SELECT student.id FROM student where account_id  =  (select id from account where first_name = "Егор"))),
((SELECT student_group.id FROM student_group where student_group.name = "121_18_1" ),
(SELECT student.id FROM student where account_id  =  (select id from account where first_name = "Дмитрий"))),
((SELECT student_group.id FROM student_group where student_group.name = "121_18_1" ),
(SELECT student.id FROM student where account_id  =  (select id from account where first_name = "Liza"))),
((SELECT student_group.id FROM student_group where student_group.name = "121_18_1" ),
(SELECT student.id FROM student where account_id  =  (select id from account where first_name = "Голлум"))),
((SELECT student_group.id FROM student_group where student_group.name = "122_17_1" ),
(SELECT student.id FROM student where account_id  =  (select id from account where first_name = "Гюнтер"))),
((SELECT student_group.id FROM student_group where student_group.name = "122_17_1" ),
(SELECT student.id FROM student where account_id  =  (select id from account where first_name = "Кейра"))),
((SELECT student_group.id FROM student_group where student_group.name = "122_17_1" ),
(SELECT student.id FROM student where account_id  =  (select id from account where first_name = "Эредин"))),
((SELECT student_group.id FROM student_group where student_group.name = "122_17_1" ),
(SELECT student.id FROM student where account_id  =  (select id from account where first_name = "Трисс"))),
((SELECT student_group.id FROM student_group where student_group.name = "122_17_1" ),
(SELECT student.id FROM student where account_id  =  (select id from account where first_name = "Вернон"))),
((SELECT student_group.id FROM student_group where student_group.name = "122_18_ск1" ),
(SELECT student.id FROM student where account_id  =  (select id from account where first_name = "Хагрид"))),
((SELECT student_group.id FROM student_group where student_group.name = "122_18_ск1" ),
(SELECT student.id FROM student where account_id  =  (select id from account where first_name = "Чжоу"))),
((SELECT student_group.id FROM student_group where student_group.name = "122_18_ск1" ),
(SELECT student.id FROM student where account_id  =  (select id from account where first_name = "Полумна"))),
((SELECT student_group.id FROM student_group where student_group.name = "122_18_ск1" ),
(SELECT student.id FROM student where account_id  =  (select id from account where first_name = "Минерва"))),
((SELECT student_group.id FROM student_group where student_group.name = "122_18_ск1" ),
(SELECT student.id FROM student where account_id  =  (select id from account where first_name = "Северус")));

insert mentor_of_course (course_id, mentor_id)
value
((select id from course where course.name='Веб дизайн'), (SELECT mentor.id FROM mentor where mentor.account_id = (select id from account where first_name = "Фродо"))),
((select id from course where course.name='создание веб приложений'),(SELECT mentor.id FROM mentor where mentor.account_id = (select id from account where first_name = "Торин"))),
((select id from course where course.name='разработка на Java'),(SELECT mentor.id FROM mentor where mentor.account_id = (select id from account where first_name = "Леголас")));

insert mentor_of_student_group (mentor_id, student_group_id)
values
(
(SELECT mentor.id FROM mentor where mentor.account_id = (select id from account where first_name = "Фродо")),
(SELECT student_group.id FROM student_group where student_group.name='121_18_1')
),
(
(SELECT mentor.id FROM mentor where mentor.account_id = (select id from account where first_name = "Торин")),
(SELECT student_group.id FROM student_group where student_group.name='121_18_1')
),
(
(SELECT mentor.id FROM mentor where mentor.account_id = (select id from account where first_name = "Леголас")),
(SELECT student_group.id FROM student_group where student_group.name='121_18_1')
);


insert lesson (mentor_id, student_group_id, theme_id, lesson_date)
value(
(SELECT mentor.id FROM mentor where mentor.account_id = (select id from account where first_name = "Леголас")),
(SELECT student_group.id FROM student_group where student_group.Name='121_18_1'),
(SELECT theme.id FROM theme where theme.name='Создание макета'),
'2020-02-12'
);

insert visit (student_id, lesson_id,presence)
value(
(SELECT student.id FROM student where account_id =  (select id from account where first_name = "Егор")),
(SELECT lesson.id FROM lesson where theme_id = (SELECT theme.id FROM theme where theme.name='Создание макета')),
true
);
