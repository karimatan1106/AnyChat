using System.Collections.Generic;
using System.Data.Entity;
using AnyChat.Data;

namespace AnyChat.Models
{
    public class AnyChatInitializer : DropCreateDatabaseIfModelChanges<AnyChatDBContext>
    {
        protected override void Seed(AnyChatDBContext context)
        {
            var chats = new List<Chat>
            {
                new Chat
                {
                    RandamTitle = "hoge",
                    RandamUrl = "hogeUrl",
                    Comment = "hogeTarou",
                },
            };

            context.Chats.AddRange(chats);
            context.SaveChanges();
        }
    }
}