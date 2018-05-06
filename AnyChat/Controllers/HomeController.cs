using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AnyChat.Data;
using AnyChat.Interfaces;
using AnyChat.Models;

namespace AnyChat.Controllers
{
    public class HomeController : Controller
    {
        #region フィールド
        private AnyChatDBContext _db = new AnyChatDBContext();
        private SessionRepository _sessionRepository;
        //private Session _session;
        #endregion

        #region コンストラクタ
        public HomeController(ISessionRepository sessionRepository)
        {
            _sessionRepository = (SessionRepository)sessionRepository;
            //_session = new Session(ControllerContext.HttpContext, new Guid());
        }
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

            //ルーム情報をセッションに保持する
            _sessionRepository.SetRoomInfo(roomInfo);

            if (canEntering)
            {
                return View(_db.Chats
                           .Where(x => x.RoomId == roomId)
                           .OrderByDescending(x => x.SpeechDateTIme));
            }
            else
            {
                return View("Entering", roomInfo);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="txtRoomPassword"></param>
        /// <returns></returns>
        [ValidateInput(false)]
        public ActionResult EnteringToChat(string txtRoomPassword)
        {
            var room = _sessionRepository.GetRoomInfo();

            //入力された合言葉をセッションに保持する
            _sessionRepository.SetRoomInputPassword(room.RoomId, txtRoomPassword);

            if (CanEnteringChatRoom(room))
            {
                return Redirect($"/Home/Chat?roomId={room.RoomId}");
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
                var roomPasswordSession = _sessionRepository.GetRoomInputPassword(room.RoomId);

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