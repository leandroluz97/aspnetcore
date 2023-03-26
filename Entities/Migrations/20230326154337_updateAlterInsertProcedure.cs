using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    /// <inheritdoc />
    public partial class updateAlterInsertProcedure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //Down(migrationBuilder);
            //string sp_AddPerson = @"
            //    ALTER PROCEDURE [dbo].[InsertPerson]
            //    (@PersonId uniqueidentifier, @PersonName nvarchar(40), @Email nvarchar(50), @DateOfBirth datetime2(7), @Gender varchar(10), @CountryId uniqueidentifier, @Address nvarchar(200), @ReceiveNewsLetters bit, @TIN nvarchar(max))
            //    AS BEGIN
            //        INSERT INTO [dbo].[Persons](PersonId, PersonName, Email, DateOfBirth, Gender, CountryId, Address, ReceiveNewsLetters, TIN) VALUES (@PersonId, @PersonName, @Email, @DateOfBirth, @Gender, @CountryId, @Address, @ReceiveNewsLetters, @TIN)
            //    END";
            //migrationBuilder.Sql(sp_AddPerson);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //string sp_AddPerson = @"
            //    DROP PROCEDURE [dbo].[InsertPerson]
            //";
            //migrationBuilder.Sql(sp_AddPerson);
        }
    }
}
