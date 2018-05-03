using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web;
using AnyChat.Data;
using AnyChat.Models;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace AnyChat.Hubs
{
    [HubName("chatHub")]
    public class ChatHub : Hub
    {
        #region フィールド
        private Random _random;
        private Dictionary<int, (string title, string url)> _randomDic;
        #endregion

        #region コンストラクタ
        public ChatHub()
        {
            //ランダムなタイトル・URLの初期値をセット
            _randomDic = new Dictionary<int, (string, string)>
            {
                { 0, ("C Sharp - Wikipedia - ウィキペディア","https://ja.wikipedia.org/wiki/C_Sharp")},
                { 1, ("C# のガイド | Microsoft Docs","https://docs.microsoft.com/ja-jp/dotnet/csharp/")},
                { 2, ("C# によるプログラミング入門 | ++C++; // 未確認飛行 C","ufcpp.net/study/csharp/")},
                { 3, ("連載 C#入門 - ＠IT","www.atmarkit.co.jp › Insider.NET")},
                { 4, ("歴戦のC#プログラマが教えるおすすめレシピとは？ 最新の便利コード","https://codezine.jp/article/detail/9444")},
                { 5, ("C# 基礎文法 最速再入門【7.0対応】 - Build Insider","https://www.buildinsider.net/language/quickref/csharp/01")},
                { 6, ("初心者のためのC#プログラミング入門 - libro","libro.tuyano.com/index2?id=1204003")},
                { 7, ("【初心者向け】C#プログラミングのおすすめの4つの学習方法","https://web-camp.io")},
                { 8, ("C#のあまり見かけない便利な5つの記法 - Qiita","https://qiita.com › C#")},
                { 9, ("C#でプログラミング言語っぽいものを作ってみる - Qiita","https://qiita.com › C#")},
                { 10, ("==演算子とEqualsメソッドの違いとは？［C#］：.NET TIPS - ＠IT","www.atmarkit.co.jp › Insider.NET")},
                { 11, ("C# の Linq が python の2倍遅い、は嘘 - Qiita","https://qiita.com › Python")},
                { 12, ("C#でフラグ扱いの変数名にフラグと命名してはいけない - PG日誌","takachan.hatenablog.com/entry/2018/02/21/012100")},
                { 13, ("C#とは｜知っておきたいプログラミング言語の特徴 | CAREER HACK","careerhack.en-japan.com › インターネット用語辞典")},
                { 14, ("C# - ドキュメント | SendGrid","https://sendgrid.kke.co.jp › ... › インテグレーション › サンプルコード › v2 Mail")},
                { 15, ("C# で Cloud Spanner を使ってみる | Cloud Spanner のドキュメント","https://cloud.google.com/spanner/docs/getting-started/csharp/?hl=ja")},
                { 16, ("初心者のためのC#プログラミング入門 - libro","libro.tuyano.com/index2?id=1204003")},
                { 17, ("C# | Aiming 開発者ブログ","https://developer.aiming-inc.com/category/csharp/")},
                { 18, ("C#の主要インターフェース解説：IObservable<T>、IObserver<T>","garicchi.hatenablog.jp/entry/2014/09/17/200000")},
                { 19, ("C# 6.0 の新機能 | ちょまど帳 | @chomado 's","https://chomado.com › プログラミング › C")},
          };

            //ランダムクラスにシードを与える
            _random = new Random(DateTime.Now.Millisecond);
        }
        #endregion

        #region メソッド
        /// <summary>
        /// txtSpeech の入力ボックスでエンターキーが押された際に発火
        /// </summary>
        /// <param name="roomId"></param>
        /// <param name="groupName"></param>
        /// <param name="speech"></param>
        public void EnterEscapeTxtSpeech(string roomId, string groupName, string speech)
        {
            ////GearHostにデプロイする場合に以下の手法だと上手くいかない
            ////日本時間のタイムゾーン情報の取得
            //var jstTimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Tokyo Standard Time");

            ////日本時間(UTC+9)への変換を行う
            //var jst = new DateTimeOffset(DateTime.Now,jstTimeZoneInfo.BaseUtcOffset);
            //var speechDatetime = jst.ToString("yyyy/MM/dd HH:mm:ss");

            //日本時間(UTC+9)への変換を行う
            var jst = DateTime.UtcNow.AddHours(9);
            var speechDatetime = jst.ToString("yyyy/MM/dd HH:mm:ss");

            //入力文字列の中にURLが存在するかどうかを判定だけする
            var urlPattern = new Regex(@"(https?|ftp)(:\/\/[-_.!~*\'()a-zA-Z0-9;\/?:\@&=+\$,%#]+)");
            var urlPatternMatch = urlPattern.Match(speech);

            //入力文字列をサニタイズする
            speech = HttpUtility.HtmlEncode(speech);

            //入力文字列の中にURLが存在する場合はアンカータグに変換する
            if (urlPatternMatch.Success)
            {
                speech = speech.Replace(urlPatternMatch.Value, $"<a href=\"{urlPatternMatch}\">{urlPatternMatch}</a>");
            }

            var (title, url) = _randomDic[_random.Next(_randomDic.Count)];
            speech = speechDatetime + " - " + speech;

            //グループに発言を書き込む
            Clients.Group(groupName).enterEscapeTxtChat(title, url, speech);

            var chat = new Chat
            {
                UserName = Environment.UserName,
                RandamTitle = title,
                RandamUrl = url,
                Speech = speech,
                SpeechDateTIme = jst,
                RoomId = int.Parse(roomId),
            };

            using (var db = new AnyChatDBContext())
            {
                db.Chats.Add(chat);
                db.SaveChanges();
            }

            //グループ内の自分以外にプッシュ通知を行う
            Clients.Group(groupName, new[] { Context.ConnectionId }).pushNotification(speech);
        }
        // 指定されたグループへ参加する
        public void Join(string groupName)
        {
            Groups.Add(Context.ConnectionId, groupName);
            Clients.Group(groupName).joinNotify(Context.ConnectionId);
        }
        #endregion
    }
}