namespace DormitoryManagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addTableHomeFleet : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.HomeFleets",
                c => new
                    {
                        HFID = c.Int(nullable: false, identity: true),
                        HFName = c.String(maxLength: 50),
                        NumberOfRoom = c.Int(),
                        PricePerRoom = c.Int(),
                        Status = c.Int(),
                    })
                .PrimaryKey(t => t.HFID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.HomeFleets");
        }
    }
}
