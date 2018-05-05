using AnyChat.Interfaces;
using System;
using System.Web;

namespace AnyChat.Models
{
    public class SessionRepository : ISessionRepository, IDisposable
    {
        #region コンストラクタ
        public SessionRepository()
        {
            //クッキーを取得する場合は Request
            if (HttpContext.Current.Request.Cookies["anyChatWg"]== null)
            {
                //クッキーを設定する場合は Response
                HttpContext.Current.Response.Cookies["anyChatWg"].Value = Guid.NewGuid().ToString();
            }
        }
        #endregion

        #region メソッド
        /// <summary>
        /// セッションに保持したWindowGuidをセット
        /// </summary>
        /// <returns></returns>
        public Guid GetWindowGuid()
        {
            return Guid.Parse(HttpContext.Current.Request.Cookies["anyChatWg"].Value);
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
        public string GetRoomInputPassword(int roomId)
        {
            return HttpContext.Current.Session[$"roomPw{roomId}"]?.ToString();
        }
        /// <summary>
        /// 入力したルームパスワードをセッションにセット
        /// </summary>
        public void SetRoomInputPassword(int roomId, string inputRoomPassword)
        {
            HttpContext.Current.Session[$"roomPw{roomId}"] = inputRoomPassword;
        }

        public void Dispose()
        {
        }
        #endregion
    }
}