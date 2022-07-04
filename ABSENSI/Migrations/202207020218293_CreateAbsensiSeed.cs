namespace ABSENSI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    using BCrypt.Net;

    public partial class CreateAbsensiSeed : DbMigration
    {
        public override void Up()
        {
            string password = BCrypt.HashPassword("admin");
            Sql("INSERT INTO users(username, fullname, password, is_admin, inserted_at) VALUES('admin', 'John Doe','" + password + "', 1, GETDATE())");
            password = BCrypt.HashPassword("user");
            Sql("INSERT INTO users(username, fullname, password, is_admin, inserted_at) VALUES('user', 'Marcus Aulerius','" + password + "', 0, GETDATE())");
        }

        public override void Down()
        {
        }
    }
}
