namespace BAPA_LMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class KursInfoVM : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CourseDetailViewModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        StartDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Modules", "CourseDetailViewModel_Id", c => c.Int());
            CreateIndex("dbo.Modules", "CourseDetailViewModel_Id");
            AddForeignKey("dbo.Modules", "CourseDetailViewModel_Id", "dbo.CourseDetailViewModels", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Modules", "CourseDetailViewModel_Id", "dbo.CourseDetailViewModels");
            DropIndex("dbo.Modules", new[] { "CourseDetailViewModel_Id" });
            DropColumn("dbo.Modules", "CourseDetailViewModel_Id");
            DropTable("dbo.CourseDetailViewModels");
        }
    }
}
