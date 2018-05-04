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
        private Session _session = new Session();
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
            _session.SetRoomInfo(ControllerContext.HttpContext, _wg, room);
            return View(room);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="room"></param>
        /// <returns></returns>
        public ActionResult EnteringToChat(string txtRoomPassword)
        {
            var room = _session.GetRoomInfo(ControllerContext.HttpContext, _wg);
            _session.SetRoomInputPassword(ControllerContext.HttpContext, room.RoomId, txtRoomPassword);
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
                var roomPasswordSession = _session.GetRoomInputPassword(ControllerContext.HttpContext, room.RoomId);

                //セッションに合言葉が保持されていなければ合言葉を入力させる
                if (roomPasswordSession == null)
                {
                    canEntering = false;
                }
                else
                {
                    //正しい合言葉がセッションに保持されていたら入室する
                    canEntering = roomPasswordSession == room.RoomPassword;
                }
            }

            return canEntering;
        }
        #endregion
    }
}