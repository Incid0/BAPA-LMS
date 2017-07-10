namespace BAPA_LMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SqlSeedRest : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO Activities (Type, Name, Description, StartTime, EndTime, Module_Id) VALUES (4, 'Hello World', 'Vi skapar ett hello world program', 2017/07/10, 2017/07/28, 1)");
            Sql("INSERT INTO Activities (Type, Name, Description, StartTime, EndTime, Module_Id) VALUES (3, 'Random aktivitet', 'Vi spelar poker', 2017/07/10, 2017/07/11, 1)");
            Sql("INSERT INTO Activities (Type, Name, Description, StartTime, EndTime, Module_Id) VALUES (2, 'Sopsortering', 'Viktigt med miljötänk', 2017/07/10, 2017/07/28, 1)");
        }
        
        public override void Down()
        {
            Sql("DELETE FROM Activities WHERE Id IN (1,2,3)");
        }
    }
}
