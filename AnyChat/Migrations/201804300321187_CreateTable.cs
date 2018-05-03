namespace AnyChat.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Chats",
                c => new
                    {
                        ChatId = c.Int(nullable: false, identity: true),
                        UserName = c.String(),
                        RandamTitle = c.String(),
                        RandamUrl = c.String(),
                        Speech = c.String(),
                        SpeechDateTIme = c.DateTime(nullable: false),
                        RoomId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ChatId)
                .ForeignKey("dbo.Rooms", t => t.RoomId, cascadeDelete: true)
                .Index(t => t.RoomId);
            
            CreateTable(
                "dbo.Rooms",
                c => new
                    {
                        RoomId = c.Int(nullable: false, identity: true),
                        RoomName = c.String(),
                        RoomPassword = c.String(),
                        CreateDateTime = c.DateTime(nullable: false),
                        IsPublicRoom = c.Boolean(nullable: false),
                        DisplayOrder = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.RoomId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Chats", "RoomId", "dbo.Rooms");
            DropIndex("dbo.Chats", new[] { "RoomId" });
            DropTable("dbo.Rooms");
            DropTable("dbo.Chats");
        }
    }
}
