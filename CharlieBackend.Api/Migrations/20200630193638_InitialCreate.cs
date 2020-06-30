using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CharlieBackend.Api.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "accounts",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Role = table.Column<sbyte>(nullable: true, comment: "from enum of roles: 1 - student 2 - mentor 4 - admin"),
                    FirstName = table.Column<string>(type: "varchar(30)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                        .Annotation("MySql:Collation", "utf8mb4_0900_ai_ci"),
                    LastName = table.Column<string>(type: "varchar(30)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                        .Annotation("MySql:Collation", "utf8mb4_0900_ai_ci"),
                    Email = table.Column<string>(type: "varchar(50)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                        .Annotation("MySql:Collation", "utf8mb4_0900_ai_ci"),
                    Password = table.Column<string>(type: "varchar(65)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                        .Annotation("MySql:Collation", "utf8mb4_0900_ai_ci"),
                    Salt = table.Column<string>(type: "varchar(65)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                        .Annotation("MySql:Collation", "utf8mb4_0900_ai_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_accounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "courses",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(100)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                        .Annotation("MySql:Collation", "utf8mb4_0900_ai_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_courses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "themes",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(100)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                        .Annotation("MySql:Collation", "utf8mb4_0900_ai_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_themes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "mentors",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IdAccount = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mentors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccountOfMentor",
                        column: x => x.IdAccount,
                        principalTable: "accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "students",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IdAccount = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_students", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccountOfStudent",
                        column: x => x.IdAccount,
                        principalTable: "accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "studentgroups",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IdCourse = table.Column<long>(nullable: true),
                    Name = table.Column<string>(type: "varchar(100)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                        .Annotation("MySql:Collation", "utf8mb4_0900_ai_ci"),
                    StartDate = table.Column<DateTime>(type: "date", nullable: true),
                    FinishDate = table.Column<DateTime>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_studentgroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseOfStudentGroup",
                        column: x => x.IdCourse,
                        principalTable: "courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "mentorsofcourses",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IdCourse = table.Column<long>(nullable: true),
                    IdMentor = table.Column<long>(nullable: true),
                    MentorComment = table.Column<string>(type: "varchar(2048)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                        .Annotation("MySql:Collation", "utf8mb4_0900_ai_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mentorsofcourses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OfCourses",
                        column: x => x.IdCourse,
                        principalTable: "courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OfMentors",
                        column: x => x.IdMentor,
                        principalTable: "mentors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "lessons",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IdMentor = table.Column<long>(nullable: true),
                    IdStudentGroup = table.Column<long>(nullable: true),
                    IdTheme = table.Column<long>(nullable: true),
                    LessonDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_lessons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LessonsMentors",
                        column: x => x.IdMentor,
                        principalTable: "mentors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LessonsOfGroup",
                        column: x => x.IdStudentGroup,
                        principalTable: "studentgroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ThemeOfLesson",
                        column: x => x.IdTheme,
                        principalTable: "themes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "mentorsofstudentgroups",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IdMentor = table.Column<long>(nullable: true),
                    IdStudentGroup = table.Column<long>(nullable: true),
                    Comments = table.Column<string>(type: "varchar(1024)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                        .Annotation("MySql:Collation", "utf8mb4_0900_ai_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mentorsofstudentgroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MentorsOf",
                        column: x => x.IdMentor,
                        principalTable: "mentors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GroupsOf",
                        column: x => x.IdStudentGroup,
                        principalTable: "studentgroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "studentsofgroups",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IdStudentGroup = table.Column<long>(nullable: false),
                    IdStudent = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_studentsofgroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentOfGroup",
                        column: x => x.IdStudent,
                        principalTable: "students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupOfStudent",
                        column: x => x.IdStudentGroup,
                        principalTable: "studentgroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "visits",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IdStudent = table.Column<long>(nullable: true),
                    IdLesson = table.Column<long>(nullable: true),
                    StudentMark = table.Column<sbyte>(nullable: true),
                    Presence = table.Column<bool>(nullable: false),
                    Comments = table.Column<string>(type: "varchar(1024)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                        .Annotation("MySql:Collation", "utf8mb4_0900_ai_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_visits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VisitsLessons",
                        column: x => x.IdLesson,
                        principalTable: "lessons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VisitOfStudent",
                        column: x => x.IdStudent,
                        principalTable: "students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "FK_LessonsMentors",
                table: "lessons",
                column: "IdMentor");

            migrationBuilder.CreateIndex(
                name: "FK_LessonsOfGroup",
                table: "lessons",
                column: "IdStudentGroup");

            migrationBuilder.CreateIndex(
                name: "FK_ThemeOfLesson",
                table: "lessons",
                column: "IdTheme");

            migrationBuilder.CreateIndex(
                name: "FK_AccountOfMentor",
                table: "mentors",
                column: "IdAccount");

            migrationBuilder.CreateIndex(
                name: "FK_OfMentors",
                table: "mentorsofcourses",
                column: "IdMentor");

            migrationBuilder.CreateIndex(
                name: "AK_UniqueMentorAndCourse",
                table: "mentorsofcourses",
                columns: new[] { "IdCourse", "IdMentor" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "FK_GroupsOf",
                table: "mentorsofstudentgroups",
                column: "IdStudentGroup");

            migrationBuilder.CreateIndex(
                name: "AK_UniqueStudentGroupAndMentor",
                table: "mentorsofstudentgroups",
                columns: new[] { "IdMentor", "IdStudentGroup" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "FK_CourseOfStudentGroup",
                table: "studentgroups",
                column: "IdCourse");

            migrationBuilder.CreateIndex(
                name: "FK_AccountOfStudent",
                table: "students",
                column: "IdAccount");

            migrationBuilder.CreateIndex(
                name: "FK_StudentOfGroup",
                table: "studentsofgroups",
                column: "IdStudent");

            migrationBuilder.CreateIndex(
                name: "AK_UniqueStudentAndGroup",
                table: "studentsofgroups",
                columns: new[] { "IdStudentGroup", "IdStudent" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "FK_VisitsLessons",
                table: "visits",
                column: "IdLesson");

            migrationBuilder.CreateIndex(
                name: "AK_UniqueLessonAndStudent",
                table: "visits",
                columns: new[] { "IdStudent", "IdLesson" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "mentorsofcourses");

            migrationBuilder.DropTable(
                name: "mentorsofstudentgroups");

            migrationBuilder.DropTable(
                name: "studentsofgroups");

            migrationBuilder.DropTable(
                name: "visits");

            migrationBuilder.DropTable(
                name: "lessons");

            migrationBuilder.DropTable(
                name: "students");

            migrationBuilder.DropTable(
                name: "mentors");

            migrationBuilder.DropTable(
                name: "studentgroups");

            migrationBuilder.DropTable(
                name: "themes");

            migrationBuilder.DropTable(
                name: "accounts");

            migrationBuilder.DropTable(
                name: "courses");
        }
    }
}
