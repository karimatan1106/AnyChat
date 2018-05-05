﻿using System;
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
        private AnyChatDBContext db = new AnyChatDBContext();
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
            return View(db.Rooms.ToList());
        }

        // GET: Rooms/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Room room = db.Rooms.Find(id);
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
        public ActionResult Create([Bind(Include = "RoomId,RoomName,RoomPassword,CreateDateTime,IsPublicRoom,DisplayOrder")] Room room)
        {
            if (ModelState.IsValid)
            {
                room.RoomName = HttpUtility.HtmlEncode(room.RoomName);
                room.RoomPassword = HttpUtility.HtmlEncode(room.RoomPassword);
                room.CreateDateTime = DateTime.Now;
                room.RoomId = db.Rooms.Max(x => x.RoomId) + 1;
                db.Rooms.Add(room);
                db.SaveChanges();

                //設定した合言葉をセッションに保存する
                _sessionRepository.SetRoomInputPassword(room.RoomId, room.RoomPassword);
            }

            return RedirectToAction("Index", "Home");
        }

        // GET: Rooms/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Room room = db.Rooms.Find(id);
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
                db.Entry(room).State = EntityState.Modified;
                db.SaveChanges();
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
            Room room = db.Rooms.Find(id);
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
            Room room = db.Rooms.Find(id);
            db.Rooms.Remove(room);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
