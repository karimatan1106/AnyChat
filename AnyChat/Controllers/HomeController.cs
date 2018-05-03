using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AnyChat.Data;
using AnyChat.Models;

namespace AnyChat.Controllers
{
    public class HomeController : Controller
    {
        #region フィールド
        private AnyChatDBContext _db = new AnyChatDBContext();
        private Guid _wg = new Guid();
        #endregion

        #region アクションメソッド
        /// <summary>
        /// インデックス
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View(_db.Rooms.OrderByDescending(x => x.RoomId));
        }
        /// <summary>
        /// チャット
        /// </summary>
        /// <returns></returns>
        public ActionResult Chat(int roomId)
        {
            var roomInfo = _db.Rooms.First(x => x.RoomId == roomId);
            var canEntering = CanEnteringChatRoom(roomInfo);

            if (canEntering)
            {
                return View(_db.Chats
                           .Where(x => x.RoomId == roomId)
                           .OrderByDescending(x => x.SpeechDateTIme));
            }
            else
            {
                return RedirectToAction("Entering", roomInfo);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="room"></param>
        /// <returns></returns>
        public ActionResult Entering(Room room)
        {
            SetRoomInfo(room);
            return View(room);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="room"></param>
        /// <returns></returns>
        public ActionResult EnteringToChat(string txtRoomPassword)
        {
            var room = GetRoomInfo();
            SetRoomInputPassword(room.RoomId, txtRoomPassword);
            if (CanEnteringChatRoom(room))
            {
                return RedirectToAction("Chat", room);
            }
            else
            {
                return View("Entering", room);
            }
        }
        #endregion

        #region メソッド
        /// <summary>
        /// 
        /// </summary>
        /// <param name="room"></param>
        /// <returns></returns>
        private bool CanEnteringChatRoom(Room room)
        {
            var canEntering = false;

            //パブリックルームであれば常に入室可能
            if (room.IsPublicRoom)
            {
                canEntering = true;
            }
            else
            {
                var roomPasswordCookie = GetRoomInputPassword(room.RoomId);

                //クッキーに合言葉が保持されていなければ合言葉を入力させる
                if (roomPasswordCookie == null)
                {
                    canEntering = false;
                }
                else
                {
                    //正しい合言葉がクッキーに保持されていたら入室する
                    canEntering = roomPasswordCookie == room.RoomPassword;
                }
            }

            return canEntering;
        }
        /// <summary>
        /// セッションに保存したルーム情報を取得
        /// </summary>
        /// <returns></returns>
        private Room GetRoomInfo()
        {
            return (Room)(ControllerContext.HttpContext.Session[$"roomInfo{_wg}"]);
        }
        /// <summary>
        /// ルーム情報をセッションにセット
        /// </summary>
        /// <param name="roomId"></param>
        private void SetRoomInfo(Room room)
        {
            ControllerContext.HttpContext.Session[$"roomInfo{_wg}"] = room;
        }
        /// <summary>
        /// セッションに保存した入力されたルームパスワードを取得
        /// </summary>
        /// <param name="roomId"></param>
        private string GetRoomInputPassword(int roomId)
        {
            return ControllerContext.HttpContext.Session[$"roomPw{roomId}"]?.ToString();
        }
        /// <summary>
        /// 入力したルームパスワードをセッションにセット
        /// </summary>
        /// <param name="roomId"></param>
        private void SetRoomInputPassword(int roomId, string inputRoomPassword)
        {
            ControllerContext.HttpContext.Session[$"roomPw{roomId}"] = inputRoomPassword;
        }
        #endregion
    }
}