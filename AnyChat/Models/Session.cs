using System;
using System.Web;

namespace AnyChat.Models
{
    public class Session
    {
        /// <summary>
        /// セッションに保存したルーム情報を取得
        /// </summary>
        /// <returns></returns>
        public Room GetRoomInfo(HttpContextBase httpContext, Guid windowGuid)
        {
            return (Room)(httpContext.Session[$"roomInfo{windowGuid}"]);
        }
        /// <summary>
        /// ルーム情報をセッションにセット
        /// </summary>
        public void SetRoomInfo(HttpContextBase httpContext, Guid windowGuid, Room room)
        {
            httpContext.Session[$"roomInfo{windowGuid}"] = room;
        }
        /// <summary>
        /// セッションに保存した入力されたルームパスワードを取得
        /// </summary>
        public string GetRoomInputPassword(HttpContextBase httpContext, int roomId)
        {
            return httpContext.Session[$"roomPw{roomId}"]?.ToString();
        }
        /// <summary>
        /// 入力したルームパスワードをセッションにセット
        /// </summary>
        public void SetRoomInputPassword(HttpContextBase httpContext, int roomId, string inputRoomPassword)
        {
            httpContext.Session[$"roomPw{roomId}"] = inputRoomPassword;
        }
    }
}