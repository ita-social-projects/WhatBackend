using System;
using System.IO;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CharlieBackend.Api.Migrations
{
	public partial class InitialCreate : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
				name: "Attachments",
				columns: table => new
				{
					ID = table.Column<long>(nullable: false)
						.Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
					CreatedOn = table.Column<DateTime>(type: "DATETIME", nullable: false, comment: "Use UTC time"),
					CreatedByAccountID = table.Column<long>(nullable: false),
					ContainerName = table.Column<string>(type: "VARCHAR(36)", nullable: false, comment: "GUID length is 36 characters"),
					FileName = table.Column<string>(type: "VARCHAR(100)", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PRIMARY", x => x.ID);
				});

			migrationBuilder.CreateTable(
				name: "Courses",
				columns: table => new
				{
					ID = table.Column<long>(nullable: false)
						.Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
					Name = table.Column<string>(type: "VARCHAR(100)", nullable: false),
					IsActive = table.Column<ulong>(type: "BIT", nullable: false, defaultValueSql: "1")
				},
				constraints: table =>
				{
					table.PrimaryKey("PRIMARY", x => x.ID);
				});

			migrationBuilder.CreateTable(
				name: "Themes",
				columns: table => new
				{
					ID = table.Column<long>(nullable: false)
						.Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
					Name = table.Column<string>(type: "VARCHAR(100)", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PRIMARY", x => x.ID);
				});

			migrationBuilder.CreateTable(
				name: "Accounts",
				columns: table => new
				{
					ID = table.Column<long>(nullable: false)
						.Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
					Role = table.Column<byte>(nullable: false, comment: "Roles:0 - NotAssigned; 1 - Student, 2 - Mentor, 4 - Admin, 8 - Secretary"),
					FirstName = table.Column<string>(type: "VARCHAR(30)", nullable: false),
					LastName = table.Column<string>(type: "VARCHAR(30)", nullable: false),
					Email = table.Column<string>(type: "VARCHAR(50)", nullable: false),
					PasswordHash = table.Column<string>(type: "VARCHAR(64)", nullable: false, comment: "SHA265 output size is 256 bits or 64 characters"),
					Salt = table.Column<string>(type: "VARCHAR(32)", nullable: false, comment: "Standard salt size is 128 bits or 32 characters"),
					IsActive = table.Column<ulong>(type: "BIT", nullable: false, defaultValueSql: "1"),
					ForgotPasswordToken = table.Column<string>(type: "VARCHAR(36)", nullable: true, comment: "GUID length is 36 characters"),
					ForgotTokenGenDate = table.Column<DateTime>(type: "DATETIME", nullable: true, comment: "Use UTC time"),
					AvatarID = table.Column<long>(nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PRIMARY", x => x.ID);
					table.ForeignKey(
						name: "FK_AvatarAccounts",
						column: x => x.AvatarID,
						principalTable: "Attachments",
						principalColumn: "ID",
						onDelete: ReferentialAction.Restrict);
				});

			migrationBuilder.CreateTable(
				name: "StudentGroups",
				columns: table => new
				{
					ID = table.Column<long>(nullable: false)
						.Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
					CourseID = table.Column<long>(nullable: false),
					Name = table.Column<string>(type: "VARCHAR(100)", nullable: false),
					StartDate = table.Column<DateTime>(type: "DATE", nullable: false),
					FinishDate = table.Column<DateTime>(type: "DATE", nullable: false),
					IsActive = table.Column<ulong>(type: "BIT", nullable: false, defaultValueSql: "1")
				},
				constraints: table =>
				{
					table.PrimaryKey("PRIMARY", x => x.ID);
					table.ForeignKey(
						name: "FK_CourseStudentGroups",
						column: x => x.CourseID,
						principalTable: "Courses",
						principalColumn: "ID",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "Marks",
				columns: table => new
				{
					ID = table.Column<long>(nullable: false)
						.Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
					Value = table.Column<sbyte>(type: "TINYINT", nullable: false),
					Comment = table.Column<string>(type: "VARCHAR(1024)", nullable: true),
					EvaluationDate = table.Column<DateTime>(type: "DATETIME", nullable: false),
					Type = table.Column<byte>(type: "TINYINT UNSIGNED", nullable: false, comment: "Types: 0 - Homework, 1 - Visit"),
					EvaluatedBy = table.Column<long>(nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PRIMARY", x => x.ID);
					table.ForeignKey(
						name: "FK_AccountOfMark",
						column: x => x.EvaluatedBy,
						principalTable: "Accounts",
						principalColumn: "ID",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "Mentors",
				columns: table => new
				{
					ID = table.Column<long>(nullable: false)
						.Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
					AccountID = table.Column<long>(nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PRIMARY", x => x.ID);
					table.ForeignKey(
						name: "FK_AccountMentors",
						column: x => x.AccountID,
						principalTable: "Accounts",
						principalColumn: "ID",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "Secretaries",
				columns: table => new
				{
					ID = table.Column<long>(nullable: false)
						.Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
					AccountID = table.Column<long>(nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PRIMARY", x => x.ID);
					table.ForeignKey(
						name: "FK_AccountSecretaries",
						column: x => x.AccountID,
						principalTable: "Accounts",
						principalColumn: "ID",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "Students",
				columns: table => new
				{
					ID = table.Column<long>(nullable: false)
						.Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
					AccountID = table.Column<long>(nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PRIMARY", x => x.ID);
					table.ForeignKey(
						name: "FK_AccountStudents",
						column: x => x.AccountID,
						principalTable: "Accounts",
						principalColumn: "ID",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "EventOccurrences",
				columns: table => new
				{
					ID = table.Column<long>(nullable: false)
						.Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
					StudentGroupID = table.Column<long>(nullable: false),
					EventStart = table.Column<DateTime>(type: "DATETIME", nullable: false, comment: "Use UTC time"),
					EventFinish = table.Column<DateTime>(type: "DATETIME", nullable: false, comment: "Use UTC time"),
					Pattern = table.Column<int>(nullable: false, comment: "Patterns: 0 - Daily, 1 - Weekly, 2 - AbsoluteMonthly, 3 - RelativeMonthly"),
					Storage = table.Column<long>(nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PRIMARY", x => x.ID);
					table.ForeignKey(
						name: "FK_StudentGroupEventOccurrences",
						column: x => x.StudentGroupID,
						principalTable: "StudentGroups",
						principalColumn: "ID",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "Lessons",
				columns: table => new
				{
					ID = table.Column<long>(nullable: false)
						.Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
					MentorID = table.Column<long>(nullable: false),
					StudentGroupID = table.Column<long>(nullable: false),
					ThemeID = table.Column<long>(nullable: false),
					LessonDate = table.Column<DateTime>(type: "DATETIME", nullable: false, comment: "Use UTC time")
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Lessons", x => x.ID);
					table.ForeignKey(
						name: "FK_MentorLessons",
						column: x => x.MentorID,
						principalTable: "Mentors",
						principalColumn: "ID",
						onDelete: ReferentialAction.Cascade);
					table.ForeignKey(
						name: "FK_StudentGroupLessons",
						column: x => x.StudentGroupID,
						principalTable: "StudentGroups",
						principalColumn: "ID",
						onDelete: ReferentialAction.Cascade);
					table.ForeignKey(
						name: "FK_ThemeLessons",
						column: x => x.ThemeID,
						principalTable: "Themes",
						principalColumn: "ID",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "MentorsOfCourses",
				columns: table => new
				{
					ID = table.Column<long>(nullable: false)
						.Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
					CourseID = table.Column<long>(nullable: false),
					MentorID = table.Column<long>(nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PRIMARY", x => x.ID);
					table.ForeignKey(
						name: "FK_CourseOfMentors",
						column: x => x.CourseID,
						principalTable: "Courses",
						principalColumn: "ID",
						onDelete: ReferentialAction.Cascade);
					table.ForeignKey(
						name: "FK_MentorOfCourses",
						column: x => x.MentorID,
						principalTable: "Mentors",
						principalColumn: "ID",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "MentorsOfStudentGroups",
				columns: table => new
				{
					ID = table.Column<long>(nullable: false)
						.Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
					MentorID = table.Column<long>(nullable: false),
					StudentGroupID = table.Column<long>(nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PRIMARY", x => x.ID);
					table.ForeignKey(
						name: "FK_MentorOfStudentGroups",
						column: x => x.MentorID,
						principalTable: "Mentors",
						principalColumn: "ID",
						onDelete: ReferentialAction.Cascade);
					table.ForeignKey(
						name: "FK_StudentGroupOfMentors",
						column: x => x.StudentGroupID,
						principalTable: "StudentGroups",
						principalColumn: "ID",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "StudentsOfStudentGroups",
				columns: table => new
				{
					ID = table.Column<long>(nullable: false)
						.Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
					StudentGroupID = table.Column<long>(nullable: false),
					StudentID = table.Column<long>(nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PRIMARY", x => x.ID);
					table.ForeignKey(
						name: "FK_StudentGroupOfStudents",
						column: x => x.StudentGroupID,
						principalTable: "StudentGroups",
						principalColumn: "ID",
						onDelete: ReferentialAction.Cascade);
					table.ForeignKey(
						name: "FK_StudentOfStudentGroups",
						column: x => x.StudentID,
						principalTable: "Students",
						principalColumn: "ID",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "Homeworks",
				columns: table => new
				{
					ID = table.Column<long>(nullable: false)
						.Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
					DueDate = table.Column<DateTime>(type: "DATETIME", nullable: false, comment: "Use UTC time"),
					TaskText = table.Column<string>(type: "VARCHAR(8000)", nullable: false),
					LessonID = table.Column<long>(nullable: false),
					PublishingDate = table.Column<DateTime>(type: "DATETIME", nullable: false, comment: "Use UTC time"),
					CreatedBy = table.Column<long>(nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PRIMARY", x => x.ID);
					table.ForeignKey(
						name: "FK_AccountOfHomework",
						column: x => x.CreatedBy,
						principalTable: "Accounts",
						principalColumn: "ID",
						onDelete: ReferentialAction.Cascade);
					table.ForeignKey(
						name: "FK_LessonHomeworks",
						column: x => x.LessonID,
						principalTable: "Lessons",
						principalColumn: "ID",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "ScheduledEvents",
				columns: table => new
				{
					ID = table.Column<long>(nullable: false)
						.Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
					EventOccurrenceID = table.Column<long>(nullable: false),
					StudentGroupID = table.Column<long>(nullable: false),
					ThemeID = table.Column<long>(nullable: false),
					MentorID = table.Column<long>(nullable: false),
					LessonID = table.Column<long>(nullable: true),
					EventStart = table.Column<DateTime>(type: "DATETIME", nullable: false, comment: "Use UTC time"),
					EventFinish = table.Column<DateTime>(type: "DATETIME", nullable: false, comment: "Use UTC time")
				},
				constraints: table =>
				{
					table.PrimaryKey("PRIMARY", x => x.ID);
					table.ForeignKey(
						name: "FK_EventOccurrenceScheduledEvents",
						column: x => x.EventOccurrenceID,
						principalTable: "EventOccurrences",
						principalColumn: "ID",
						onDelete: ReferentialAction.Cascade);
					table.ForeignKey(
						name: "FK_LessonScheduledEvents",
						column: x => x.LessonID,
						principalTable: "Lessons",
						principalColumn: "ID",
						onDelete: ReferentialAction.Restrict);
					table.ForeignKey(
						name: "FK_MentorScheduledEvents",
						column: x => x.MentorID,
						principalTable: "Mentors",
						principalColumn: "ID",
						onDelete: ReferentialAction.Cascade);
					table.ForeignKey(
						name: "FK_StudentGroupScheduledEvents",
						column: x => x.StudentGroupID,
						principalTable: "StudentGroups",
						principalColumn: "ID",
						onDelete: ReferentialAction.Cascade);
					table.ForeignKey(
						name: "FK_ThemeScheduledEvents",
						column: x => x.ThemeID,
						principalTable: "Themes",
						principalColumn: "ID",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "Visits",
				columns: table => new
				{
					ID = table.Column<long>(nullable: false)
						.Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
					StudentID = table.Column<long>(nullable: false),
					LessonID = table.Column<long>(nullable: false),
					StudentMark = table.Column<sbyte>(nullable: true),
					Presence = table.Column<ulong>(type: "BIT", nullable: false),
					Comment = table.Column<string>(type: "VARCHAR(1024)", nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PRIMARY", x => x.ID);
					table.CheckConstraint("CH_MarkVisits", "StudentMark >= 0 AND StudentMark <= 100");
					table.ForeignKey(
						name: "FK_LessonVisits",
						column: x => x.LessonID,
						principalTable: "Lessons",
						principalColumn: "ID",
						onDelete: ReferentialAction.Cascade);
					table.ForeignKey(
						name: "FK_StudentVisits",
						column: x => x.StudentID,
						principalTable: "Students",
						principalColumn: "ID",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "AttachmentsOfHomeworks",
				columns: table => new
				{
					ID = table.Column<long>(nullable: false)
						.Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
					HomeworkID = table.Column<long>(nullable: false),
					AttachmentID = table.Column<long>(nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PRIMARY", x => x.ID);
					table.ForeignKey(
						name: "FK_AttachmentOfHomeworks",
						column: x => x.AttachmentID,
						principalTable: "Attachments",
						principalColumn: "ID",
						onDelete: ReferentialAction.Cascade);
					table.ForeignKey(
						name: "FK_HomeworkOfAttachments",
						column: x => x.HomeworkID,
						principalTable: "Homeworks",
						principalColumn: "ID",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "HomeworksFromStudents",
				columns: table => new
				{
					ID = table.Column<long>(nullable: false)
						.Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
					StudentID = table.Column<long>(nullable: false),
					HomeworkID = table.Column<long>(nullable: false),
					HomeworkText = table.Column<string>(type: "VARCHAR(8000)", nullable: true),
					MarkId = table.Column<long>(nullable: true),
					PublishingDate = table.Column<DateTime>(type: "DATETIME", nullable: false),
					IsSent = table.Column<bool>(type: "TINYINT(1)", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PRIMARY", x => x.ID);
					table.ForeignKey(
						name: "FK_HomeworkOfStudents",
						column: x => x.HomeworkID,
						principalTable: "Homeworks",
						principalColumn: "ID",
						onDelete: ReferentialAction.Cascade);
					table.ForeignKey(
						name: "FK_MarkOfHomeworkFromStudent",
						column: x => x.MarkId,
						principalTable: "Marks",
						principalColumn: "ID",
						onDelete: ReferentialAction.Restrict);
					table.ForeignKey(
						name: "FK_StudentOfHomeworks",
						column: x => x.StudentID,
						principalTable: "Students",
						principalColumn: "ID",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "AttachmentsOfHomeworksFromStudents",
				columns: table => new
				{
					ID = table.Column<long>(nullable: false)
						.Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
					HomeworkFromStudentID = table.Column<long>(nullable: false),
					AttachmentID = table.Column<long>(nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PRIMARY", x => x.ID);
					table.ForeignKey(
						name: "FK_AttachmentOfHomeworksFromStudents",
						column: x => x.AttachmentID,
						principalTable: "Attachments",
						principalColumn: "ID",
						onDelete: ReferentialAction.Cascade);
					table.ForeignKey(
						name: "FK_HomeworkFromStudentOfAttachments",
						column: x => x.HomeworkFromStudentID,
						principalTable: "HomeworksFromStudents",
						principalColumn: "ID",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "HomeworksFromStudentsHistory",
				columns: table => new
				{
					ID = table.Column<long>(nullable: false)
						.Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
					HomeworkText = table.Column<string>(type: "VARCHAR(8000)", nullable: true),
					HomeworkFromStudentID = table.Column<long>(nullable: false),
					MarkID = table.Column<long>(nullable: false),
					PublishingDate = table.Column<DateTime>(type: "DATETIME", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PRIMARY", x => x.ID);
					table.ForeignKey(
						name: "FK_HomeworkStudentOfHistory",
						column: x => x.HomeworkFromStudentID,
						principalTable: "HomeworksFromStudents",
						principalColumn: "ID",
						onDelete: ReferentialAction.Cascade);
					table.ForeignKey(
						name: "FK_MarkOfHitory",
						column: x => x.MarkID,
						principalTable: "Marks",
						principalColumn: "ID",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "AttachmentsOfHomeworksFromStudentsHistory",
				columns: table => new
				{
					ID = table.Column<long>(nullable: false)
						.Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
					HomeworkFromStudentHistoryID = table.Column<long>(nullable: false),
					AttachmentID = table.Column<long>(nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PRIMARY", x => x.ID);
					table.ForeignKey(
						name: "FK_AttachmentOfHomeworksFromStudentsHistory",
						column: x => x.AttachmentID,
						principalTable: "Attachments",
						principalColumn: "ID",
						onDelete: ReferentialAction.Cascade);
					table.ForeignKey(
						name: "FK_HomeworkFromStudentHistoryOfAttachments",
						column: x => x.HomeworkFromStudentHistoryID,
						principalTable: "HomeworksFromStudentsHistory",
						principalColumn: "ID",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateIndex(
				name: "UQ_AvatarAccounts",
				table: "Accounts",
				column: "AvatarID",
				unique: true);

			migrationBuilder.CreateIndex(
				name: "UQ_EmailAccounts",
				table: "Accounts",
				column: "Email",
				unique: true);

			migrationBuilder.CreateIndex(
				name: "UQ_ContainerNameAttachments",
				table: "Attachments",
				column: "ContainerName",
				unique: true);

			migrationBuilder.CreateIndex(
				name: "IX_AttachmentsOfHomeworks_AttachmentID",
				table: "AttachmentsOfHomeworks",
				column: "AttachmentID");

			migrationBuilder.CreateIndex(
				name: "UQ_AttachmentAndHomework",
				table: "AttachmentsOfHomeworks",
				columns: new[] { "HomeworkID", "AttachmentID" },
				unique: true);

			migrationBuilder.CreateIndex(
				name: "IX_Attachment",
				table: "AttachmentsOfHomeworksFromStudents",
				column: "AttachmentID");

			migrationBuilder.CreateIndex(
				name: "IX_HomeworkFromStudent",
				table: "AttachmentsOfHomeworksFromStudents",
				column: "HomeworkFromStudentID");

			migrationBuilder.CreateIndex(
				name: "UQ_HomeworkFromStudentAndAttachment",
				table: "AttachmentsOfHomeworksFromStudents",
				columns: new[] { "AttachmentID", "HomeworkFromStudentID" },
				unique: true);

			migrationBuilder.CreateIndex(
				name: "IX_AttachmentHistory",
				table: "AttachmentsOfHomeworksFromStudentsHistory",
				column: "AttachmentID");

			migrationBuilder.CreateIndex(
				name: "IX_HomeworkFromStudentHistory",
				table: "AttachmentsOfHomeworksFromStudentsHistory",
				column: "HomeworkFromStudentHistoryID");

			migrationBuilder.CreateIndex(
				name: "UQ_HomeworkFromStudentAndAttachment",
				table: "AttachmentsOfHomeworksFromStudentsHistory",
				columns: new[] { "AttachmentID", "HomeworkFromStudentHistoryID" },
				unique: true);

			migrationBuilder.CreateIndex(
				name: "UQ_NameCourses",
				table: "Courses",
				column: "Name",
				unique: true);

			migrationBuilder.CreateIndex(
				name: "IX_EventOccurrences_StudentGroupID",
				table: "EventOccurrences",
				column: "StudentGroupID");

			migrationBuilder.CreateIndex(
				name: "IX_Homeworks_CreatedBy",
				table: "Homeworks",
				column: "CreatedBy");

			migrationBuilder.CreateIndex(
				name: "IX_Lesson",
				table: "Homeworks",
				column: "LessonID");

			migrationBuilder.CreateIndex(
				name: "IX_Homework",
				table: "HomeworksFromStudents",
				column: "HomeworkID");

			migrationBuilder.CreateIndex(
				name: "IX_HomeworksFromStudents_MarkId",
				table: "HomeworksFromStudents",
				column: "MarkId",
				unique: true);

			migrationBuilder.CreateIndex(
				name: "UQ_HomeworkAndStudent",
				table: "HomeworksFromStudents",
				columns: new[] { "StudentID", "HomeworkID" },
				unique: true);

			migrationBuilder.CreateIndex(
				name: "IX_HomeworksFromStudentsHistory_HomeworkFromStudentID",
				table: "HomeworksFromStudentsHistory",
				column: "HomeworkFromStudentID");

			migrationBuilder.CreateIndex(
				name: "IX_HomeworksFromStudentsHistory_MarkID",
				table: "HomeworksFromStudentsHistory",
				column: "MarkID",
				unique: true);

			migrationBuilder.CreateIndex(
				name: "IX_Lessons_MentorID",
				table: "Lessons",
				column: "MentorID");

			migrationBuilder.CreateIndex(
				name: "IX_Lessons_StudentGroupID",
				table: "Lessons",
				column: "StudentGroupID");

			migrationBuilder.CreateIndex(
				name: "IX_Lessons_ThemeID",
				table: "Lessons",
				column: "ThemeID");

			migrationBuilder.CreateIndex(
				name: "IX_Marks_EvaluatedBy",
				table: "Marks",
				column: "EvaluatedBy");

			migrationBuilder.CreateIndex(
				name: "UQ_AccountMentors",
				table: "Mentors",
				column: "AccountID",
				unique: true);

			migrationBuilder.CreateIndex(
				name: "IX_MentorsOfCourses_MentorID",
				table: "MentorsOfCourses",
				column: "MentorID");

			migrationBuilder.CreateIndex(
				name: "UQ_MentorAndCourse",
				table: "MentorsOfCourses",
				columns: new[] { "CourseID", "MentorID" },
				unique: true);

			migrationBuilder.CreateIndex(
				name: "IX_MentorsOfStudentGroups_StudentGroupID",
				table: "MentorsOfStudentGroups",
				column: "StudentGroupID");

			migrationBuilder.CreateIndex(
				name: "UQ_MentorAndStudentGroup",
				table: "MentorsOfStudentGroups",
				columns: new[] { "MentorID", "StudentGroupID" },
				unique: true);

			migrationBuilder.CreateIndex(
				name: "IX_ScheduledEvents_EventOccurrenceID",
				table: "ScheduledEvents",
				column: "EventOccurrenceID");

			migrationBuilder.CreateIndex(
				name: "UQ_LessonScheduledEvents",
				table: "ScheduledEvents",
				column: "LessonID",
				unique: true);

			migrationBuilder.CreateIndex(
				name: "IX_ScheduledEvents_MentorID",
				table: "ScheduledEvents",
				column: "MentorID");

			migrationBuilder.CreateIndex(
				name: "IX_ScheduledEvents_StudentGroupID",
				table: "ScheduledEvents",
				column: "StudentGroupID");

			migrationBuilder.CreateIndex(
				name: "IX_ScheduledEvents_ThemeID",
				table: "ScheduledEvents",
				column: "ThemeID");

			migrationBuilder.CreateIndex(
				name: "UQ_AccountSecretaries",
				table: "Secretaries",
				column: "AccountID",
				unique: true);

			migrationBuilder.CreateIndex(
				name: "IX_StudentGroups_CourseID",
				table: "StudentGroups",
				column: "CourseID");

			migrationBuilder.CreateIndex(
				name: "UQ_NameStudentGroups",
				table: "StudentGroups",
				column: "Name",
				unique: true);

			migrationBuilder.CreateIndex(
				name: "UQ_AccountStudents",
				table: "Students",
				column: "AccountID",
				unique: true);

			migrationBuilder.CreateIndex(
				name: "IX_StudentsOfStudentGroups_StudentGroupID",
				table: "StudentsOfStudentGroups",
				column: "StudentGroupID");

			migrationBuilder.CreateIndex(
				name: "UQ_StudentAndStudentGroup",
				table: "StudentsOfStudentGroups",
				columns: new[] { "StudentID", "StudentGroupID" },
				unique: true);

			migrationBuilder.CreateIndex(
				name: "UQ_NameThemes",
				table: "Themes",
				column: "Name",
				unique: true);

			migrationBuilder.CreateIndex(
				name: "IX_Visits_LessonID",
				table: "Visits",
				column: "LessonID");

			migrationBuilder.CreateIndex(
				name: "IX_Visits_StudentID",
				table: "Visits",
				column: "StudentID");

			migrationBuilder.Sql(File.ReadAllText("../scripts/DataScript.sql"));

		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "AttachmentsOfHomeworks");

			migrationBuilder.DropTable(
				name: "AttachmentsOfHomeworksFromStudents");

			migrationBuilder.DropTable(
				name: "AttachmentsOfHomeworksFromStudentsHistory");

			migrationBuilder.DropTable(
				name: "MentorsOfCourses");

			migrationBuilder.DropTable(
				name: "MentorsOfStudentGroups");

			migrationBuilder.DropTable(
				name: "ScheduledEvents");

			migrationBuilder.DropTable(
				name: "Secretaries");

			migrationBuilder.DropTable(
				name: "StudentsOfStudentGroups");

			migrationBuilder.DropTable(
				name: "Visits");

			migrationBuilder.DropTable(
				name: "HomeworksFromStudentsHistory");

			migrationBuilder.DropTable(
				name: "EventOccurrences");

			migrationBuilder.DropTable(
				name: "HomeworksFromStudents");

			migrationBuilder.DropTable(
				name: "Homeworks");

			migrationBuilder.DropTable(
				name: "Marks");

			migrationBuilder.DropTable(
				name: "Students");

			migrationBuilder.DropTable(
				name: "Lessons");

			migrationBuilder.DropTable(
				name: "Mentors");

			migrationBuilder.DropTable(
				name: "StudentGroups");

			migrationBuilder.DropTable(
				name: "Themes");

			migrationBuilder.DropTable(
				name: "Accounts");

			migrationBuilder.DropTable(
				name: "Courses");

			migrationBuilder.DropTable(
				name: "Attachments");
		}
	}
}
