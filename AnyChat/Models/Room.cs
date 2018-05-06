using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace AnyChat.Models
{
    public class Room
    {
        public int RoomId { get; set; }
        public Guid RoomGuid { get; set; }
        [AllowHtml]
        [Required(ErrorMessage = "ルーム作成者を入力してください")]
        public string RoomCreater { get; set; }
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