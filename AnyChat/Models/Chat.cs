using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnyChat.Models
{
    public class Chat
    {
        public int ChatId { get; set; }
        public string UserName { get; set; }
        public string RandamTitle { get; set; }
        public string RandamUrl { get; set; }
        public string Speech { get; set; }
        public DateTime SpeechDateTIme { get; set; }
        public int RoomId { get; set; }
        [ForeignKey("RoomId")]
        public virtual Room Room { get; set; }
    }
}