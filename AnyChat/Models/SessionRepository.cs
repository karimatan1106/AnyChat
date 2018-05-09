using AnyChat.Interfaces;
using System;
using System.Configuration;
using System.Web;
using System.Web.Configuration;

namespace AnyChat.Models
{
    public class SessionRepository : ISessionRepository, IDisposable
    {
        #region メソッド
        /// <summary>
        /// セッションIDを取得
        /// </summary>
        /// <returns></returns>
        public string GetWindowGuid()
        {
            return HttpContext.Current.Session.SessionID;
        }
        /// <summary>
        /// セッションに保存したルーム情報を取得
        /// </summary>
        /// <returns></returns>
        public Room GetRoomInfo()
        {
            var wg = GetWindowGuid();
            return (Room)(HttpContext.Current.Session[$"roomInfo{wg}"]);
        }
        /// <summary>
        /// ルーム情報をセッションにセット
        /// </summary>
        public void SetRoomInfo(Room room)
        {
            var wg = GetWindowGuid();
            HttpContext.Current.Session[$"roomInfo{wg}"] = room;
        }
        /// <summary>
        /// セッションに保存した入力されたルームパスワードを取得
        /// </summary>
        public string GetRoomInputPassword(Guid roomGuid)
        {
            return HttpContext.Current.Session[$"roomPw{roomGuid.ToString()}"]?.ToString();
        }
        /// <summary>
        /// 入力したルームパスワードをセッションにセット
        /// </summary>
        public void SetRoomInputPassword(Guid roomGuid, string inputRoomPassword)
        {
            HttpContext.Current.Session[$"roomPw{roomGuid.ToString()}"] = SafePassword.GetSaltedPassword(roomGuid.ToString(), inputRoomPassword);
        }

        public void Dispose()
        {
        }
        #endregion
    }
}