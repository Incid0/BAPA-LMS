
namespace BAPA_LMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedFKToMembers : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AspNetUsers", "Course_Id", "dbo.Courses");
            DropIndex("dbo.AspNetUsers", new[] { "Course_Id" });
            AddColumn("dbo.AspNetUsers", "Course_Id1", c => c.Int());
            AlterColumn("dbo.AspNetUsers", "Course_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.AspNetUsers", "Course_Id1");
            AddForeignKey("dbo.AspNetUsers", "Course_Id1", "dbo.Courses", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUsers", "Course_Id1", "dbo.Courses");
            DropIndex("dbo.AspNetUsers", new[] { "Course_Id1" });
            AlterColumn("dbo.AspNetUsers", "Course_Id", c => c.Int());
            DropColumn("dbo.AspNetUsers", "Course_Id1");
            CreateIndex("dbo.AspNetUsers", "Course_Id");
            AddForeignKey("dbo.AspNetUsers", "Course_Id", "dbo.Courses", "Id");
        }
    }
}
