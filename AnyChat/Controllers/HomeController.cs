using System.Linq;
using System.Web.Mvc;
using AnyChat.Data;

namespace AnyChat.Controllers
{
    public class HomeController : Controller
    {
        #region フィールド
        private AnyChatDBContext _db = new AnyChatDBContext();
        #endregion

        #region アクションメソッド
        /// <summary>
        /// インデックスページ
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View(_db.Chats.OrderByDescending(x => x.CommentDateTIme));
        }
        #endregion
    }
}