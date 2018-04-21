namespace AnyChat.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateTable_Chats2 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Chats", "UserId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Chats", "UserId", c => c.Int(nullable: false));
        }
    }
}
