namespace AnyChat.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterTableRoomsAddRoomGuid : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Rooms", "RoomGuid", c => c.Guid(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Rooms", "RoomGuid");
        }
    }
}
