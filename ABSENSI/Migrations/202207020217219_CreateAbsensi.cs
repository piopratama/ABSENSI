namespace ABSENSI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateAbsensi : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        user_id = c.Int(nullable: false, identity: true),
                        username = c.String(),
                        fullname = c.String(),
                        password = c.String(),
                        is_admin = c.Boolean(nullable: false),
                        inserted_at = c.DateTime(),
                        updated_at = c.DateTime(),
                        deleted_at = c.DateTime(),
                    })
                .PrimaryKey(t => t.user_id);
            
            CreateTable(
                "dbo.UserSchedules",
                c => new
                    {
                        user_schedule_id = c.Int(nullable: false, identity: true),
                        schedule_start = c.DateTime(nullable: false),
                        schedule_end = c.DateTime(nullable: false),
                        user_id = c.Int(nullable: false),
                        attendance = c.Boolean(nullable: false),
                        lat = c.String(),
                        lng = c.String(),
                        inserted_at = c.DateTime(),
                        updated_at = c.DateTime(),
                        deleted_at = c.DateTime(),
                    })
                .PrimaryKey(t => t.user_schedule_id)
                .ForeignKey("dbo.Users", t => t.user_id, cascadeDelete: true)
                .Index(t => t.user_id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserSchedules", "user_id", "dbo.Users");
            DropIndex("dbo.UserSchedules", new[] { "user_id" });
            DropTable("dbo.UserSchedules");
            DropTable("dbo.Users");
        }
    }
}
