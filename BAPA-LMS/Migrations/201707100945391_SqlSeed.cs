namespace BAPA_LMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SqlSeed : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO Modules ( Name, Description, StartDate, EndDate, Course_Id) VALUES ( 'C# Basic', 'Grundläggande inom C#', 2017/07/10, 2017/07/28, 1)");
        }
        
        public override void Down()
        {
            Sql("DELETE FROM Modules WHERE Id IN (1)");
        }
    }
}
