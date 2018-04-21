using System;

namespace AnyChat.Models
{
    public class Chat
    {
        public int Id { get; set; }
        //public int UserId { get; set; }
        public string UserName { get; set; }
        public string RandamTitle { get; set; }
        public string RandamUrl { get; set; }
        public string Comment { get; set; }
        public DateTime CommentDateTIme { get; set; }
    }
}