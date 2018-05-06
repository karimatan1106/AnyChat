namespace AnyChat.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterTableRoomsAddRoomCreater : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Rooms", "RoomCreater", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Rooms", "RoomCreater");
        }
    }
}
