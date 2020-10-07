/*==============================================================*/
/* DBMS name:      MySQL 5.0                                    */
/* Created on:     26.06.2020 10:02:06                          */
/*==============================================================*/


drop table if exists Accounts;

drop table if exists Cources;

drop table if exists CourcesOfMentors;

drop table if exists Groups;

drop table if exists Lessons;

drop table if exists Mentors;

drop table if exists MentorsOfGroups;

drop table if exists Themes;

drop table if exists Trainees;

drop table if exists TraineesOfGroups;

drop table if exists Visits;

/*==============================================================*/
/* Table: Accounts                                              */
/*==============================================================*/
create table Accounts
(
   Id                   bigint not null auto_increment,
   Role                 tinyint comment 'from enum of roles:
            1 - trainee
            2 - mentor
            4 - admin',
   FirstName            varchar(30),
   LasttName            varchar(30),
   Email                varchar(50),
   Password             varchar(65),
   Solt                 varchar(65),
   primary key (Id)
);

/*==============================================================*/
/* Table: Cources                                               */
/*==============================================================*/
create table Cources
(
   Id                   bigint not null auto_increment,
   Name                 varchar(100),
   primary key (Id)
);

/*==============================================================*/
/* Table: CourcesOfMentors                                      */
/*==============================================================*/
create table CourcesOfMentors
(
   IdCource             bigint,
   IdMentor             bigint,
   MentorComment        varchar(2048)
);

/*==============================================================*/
/* Table: Groups                                                */
/*==============================================================*/
create table Groups
(
   Id                   bigint not null auto_increment,
   IdCource             bigint,
   Name                 varchar(100),
   StartDate            date,
   FinishDate           date,
   primary key (Id)
);

/*==============================================================*/
/* Table: Lessons                                               */
/*==============================================================*/
create table Lessons
(
   Id                   bigint not null auto_increment,
   IdMentor             bigint,
   IdGroup              bigint,
   IdTheme              bigint,
   LessonDate           datetime,
   primary key (Id)
);

/*==============================================================*/
/* Table: Mentors                                               */
/*==============================================================*/
create table Mentors
(
   Id                   bigint not null auto_increment,
   IdAccount            bigint,
   primary key (Id)
);

/*==============================================================*/
/* Table: MentorsOfGroups                                       */
/*==============================================================*/
create table MentorsOfGroups
(
   IdMentor             bigint,
   IdGroup              bigint,
   Comments             varchar(1024)
);

/*==============================================================*/
/* Table: Themes                                                */
/*==============================================================*/
create table Themes
(
   Id                   bigint not null auto_increment,
   Name                 varchar(100),
   primary key (Id)
);

/*==============================================================*/
/* Table: Trainees                                              */
/*==============================================================*/
create table Trainees
(
   Id                   bigint not null,
   IdAccount            bigint,
   primary key (Id)
);

/*==============================================================*/
/* Table: TraineesOfGroups                                      */
/*==============================================================*/
create table TraineesOfGroups
(
   IdGroup              bigint not null,
   IdTrainee            bigint not null,
   primary key (IdGroup, IdTrainee)
);

/*==============================================================*/
/* Table: Visits                                                */
/*==============================================================*/
create table Visits
(
   IdTrainee            bigint,
   IdLesson             bigint,
   StudentMark          tinyint,
   Presence             bool not null,
   Comments             varchar(1024)
);

alter table CourcesOfMentors add constraint FK_OfCources foreign key (IdCource)
      references Cources (Id) on delete restrict on update restrict;

alter table CourcesOfMentors add constraint FK_OfMentors foreign key (IdMentor)
      references Mentors (Id) on delete restrict on update restrict;

alter table Groups add constraint FK_LessonsCouces foreign key (IdCource)
      references Cources (Id) on delete restrict on update restrict;

alter table Lessons add constraint FK_LessonsMentors foreign key (IdMentor)
      references Mentors (Id) on delete restrict on update restrict;

alter table Lessons add constraint FK_LessonsOfGroup foreign key (IdGroup)
      references Groups (Id) on delete restrict on update restrict;

alter table Lessons add constraint FK_ThemesOfCources foreign key (IdTheme)
      references Themes (Id) on delete restrict on update restrict;

alter table Mentors add constraint FK_MentorAccount foreign key (IdAccount)
      references Accounts (Id) on delete restrict on update restrict;

alter table MentorsOfGroups add constraint FK_GroupsOf foreign key (IdGroup)
      references Groups (Id) on delete restrict on update restrict;

alter table MentorsOfGroups add constraint FK_MentorsOf foreign key (IdMentor)
      references Mentors (Id) on delete restrict on update restrict;

alter table Trainees add constraint FK_TraineeAccout foreign key (IdAccount)
      references Accounts (Id) on delete restrict on update restrict;

alter table TraineesOfGroups add constraint FK_TraineesOfGroups foreign key (IdGroup)
      references Groups (Id) on delete restrict on update restrict;

alter table TraineesOfGroups add constraint FK_TraineesOfGroups2 foreign key (IdTrainee)
      references Trainees (Id) on delete restrict on update restrict;

alter table Visits add constraint FK_VisitsLessons foreign key (IdLesson)
      references Lessons (Id) on delete restrict on update restrict;

alter table Visits add constraint FK_VisitsTrainees foreign key (IdTrainee)
      references Trainees (Id) on delete restrict on update restrict;
