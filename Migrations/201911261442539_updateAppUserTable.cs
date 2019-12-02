namespace DormitoryManagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateAppUserTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Owner", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "Owner");
        }
    }
}
