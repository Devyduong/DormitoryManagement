namespace DormitoryManagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class editTableRent : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Rents", "Homefleet", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Rents", "Homefleet");
        }
    }
}
