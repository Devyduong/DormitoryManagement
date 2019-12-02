namespace DormitoryManagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addTableRoom : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Rooms",
                c => new
                    {
                        RoomId = c.Int(nullable: false, identity: true),
                        RoomName = c.String(maxLength: 100),
                        Homefleet = c.Int(),
                        ForGender = c.Int(),
                        BedNumber = c.Int(),
                        BedEmpty = c.Int(),
                        Included = c.String(maxLength: 200),
                        Status = c.Int(),
                    })
                .PrimaryKey(t => t.RoomId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Rooms");
        }
    }
}
