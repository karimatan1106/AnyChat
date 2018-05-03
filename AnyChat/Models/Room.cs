using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace AnyChat.Models
{
    public class Room
    {
        public int RoomId { get; set; }
        [AllowHtml]
        [Required(ErrorMessage = "ルーム名を入力してください")]
        public string RoomName { get; set; }
        [AllowHtml]
        [Required(ErrorMessage = "あいことばを入力してください")]
        public string RoomPassword { get; set; }
        public DateTime CreateDateTime { get; set; }
        public bool IsPublicRoom { get; set; }
        public int DisplayOrder { get; set; }

        public virtual ICollection<Chat> Chats { get; set; }
    }
}