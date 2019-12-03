namespace DormitoryManagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addTableRent : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Rents",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Renter = c.String(maxLength: 20),
                        Room = c.Int(),
                        TheLease = c.String(maxLength: 50),
                        TotalFee = c.Double(),
                        Paid = c.Double(),
                        CreateDate = c.DateTime(storeType: "date"),
                        StartDate = c.DateTime(storeType: "date"),
                        EndDate = c.DateTime(storeType: "date"),
                        Status = c.Int(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Rents");
        }
    }
}
