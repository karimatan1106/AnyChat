using System.Data.Entity;
using AnyChat.Models;

namespace AnyChat.Data
{
    //Enable-Migrations でマイグレーション機能を使用できるようにする
    //モデルを変更したらNugetPakegeManagerで以下の二つを実行
    //Add-Migration XXXXXXXXXXXX
    //Update-Database
    public class AnyChatDBContext : DbContext
    {
        #region プロパティ
        /// <summary>
        /// チャットテーブル
        /// </summary>
        public DbSet<Chat> Chats { get; set; }
        #endregion

        #region コンストラクタ
        public AnyChatDBContext() : base("AnyChat") { }
        #endregion
    }
}