using Microsoft.EntityFrameworkCore.Migrations;
using System.IO;

namespace CharlieBackend.Api.Migrations
{
    public partial class RemoveMarksFrom_Visit_Totable_Marks_ : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(File.ReadAllText("../scripts/Remove marks from visit to table Marks.sql"));

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
