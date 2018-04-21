namespace AnyChat.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateTable_Chats : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Chats",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        UserName = c.String(),
                        RandamTitle = c.String(),
                        RandamUrl = c.String(),
                        Comment = c.String(),
                        CommentDateTIme = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Chats");
        }
    }
}
