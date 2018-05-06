using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace AnyChat.Models
{
    public class SafePassword
    {
        #region 定数
        private const int StretchCount = 1000;
        #endregion

        #region メソッド
        /// <summary>
        /// salt + ハッシュ化したパスワードを取得
        /// </summary>
        public static string GetSaltedPassword(string userId, string password)
        {
            password = HttpUtility.HtmlEncode(password);
            var salt = GetSha256(userId);
            return GetSha256(salt + password);
        }
        /// <summary>
        /// salt + ストレッチングしたパスワードを取得(推奨)
        /// </summary>
        public static string GetStretchedPassword(string userId, string password)
        {
            var salt = GetSha256(userId);
            var hash = "";

            for (var i = 0; i < StretchCount; ++i)
            {
                hash = GetSha256(hash + salt + password);
            }

            return hash;
        }
        /// <summary>
        /// 文字列から SHA256 のハッシュ値を取得
        /// </summary>
        private static string GetSha256(string target)
        {
            var byteValue = Encoding.UTF8.GetBytes(target);

            var sha256 = SHA256.Create();
            var hash = sha256.ComputeHash(byteValue);

            var buf = new StringBuilder();

            for (var i = 0; i < hash.Length; i++)
            {
                // 16進の数値を文字列として取り出す
                buf.AppendFormat("{0:X2}", hash[i]);
            }

            return buf.ToString();
        }
        #endregion
    }
}