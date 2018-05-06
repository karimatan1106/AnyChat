using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AnyChat.Data;
using AnyChat.Interfaces;
using AnyChat.Models;

namespace AnyChat.Controllers
{
    public class RoomsController : Controller
    {
        #region フィールド
        private AnyChatDBContext _db = new AnyChatDBContext();
        private SessionRepository _sessionRepository;
        #endregion

        #region コンストラクタ
        public RoomsController(ISessionRepository sessionRepository)
        {
            _sessionRepository = (SessionRepository)sessionRepository;
            //_session = new Session(ControllerContext.HttpContext, new Guid());
        }
        #endregion
        // GET: Rooms
        public ActionResult Index(ISessionRepository sessionReposiotry)
        {
            return View(_db.Rooms.ToList());
        }

        // GET: Rooms/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Room room = _db.Rooms.Find(id);
            if (room == null)
            {
                return HttpNotFound();
            }
            return View(room);
        }

        // GET: Rooms/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Rooms/Create
        // 過多ポスティング攻撃を防止するには、バインド先とする特定のプロパティを有効にしてください。
        // 詳細については、https://go.microsoft.com/fwlink/?LinkId=317598 を参照してください。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "RoomId,RoomCreater,RoomName,RoomPassword,CreateDateTime,IsPublicRoom,DisplayOrder")] Room room)
        {
            if (ModelState.IsValid)
            {
                var roomPassword = room.RoomPassword;

                room.RoomGuid = Guid.NewGuid();
                room.RoomPassword = SafePassword.GetSaltedPassword(room.RoomGuid.ToString(), roomPassword);
                room.RoomName = HttpUtility.HtmlEncode(room.RoomName);
                room.RoomCreater = HttpUtility.HtmlEncode(room.RoomCreater);
                room.CreateDateTime = DateTime.Now;
                
                room = _db.Rooms.Add(room);
                _db.SaveChanges();

                //設定した合言葉をセッションに保存する
                _sessionRepository.SetRoomInputPassword(room.RoomGuid, roomPassword);
            }

            return Redirect($"/Home/Chat?roomId={room.RoomId}");
        }

        // GET: Rooms/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Room room = _db.Rooms.Find(id);
            if (room == null)
            {
                return HttpNotFound();
            }
            return View(room);
        }

        // POST: Rooms/Edit/5
        // 過多ポスティング攻撃を防止するには、バインド先とする特定のプロパティを有効にしてください。
        // 詳細については、https://go.microsoft.com/fwlink/?LinkId=317598 を参照してください。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "RoomId,RoomName,RoomPassword,CreateDateTime,IsPublicRoom,DisplayOrder")] Room room)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(room).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(room);
        }

        // GET: Rooms/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Room room = _db.Rooms.Find(id);
            if (room == null)
            {
                return HttpNotFound();
            }
            return View(room);
        }

        // POST: Rooms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Room room = _db.Rooms.Find(id);
            _db.Rooms.Remove(room);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
