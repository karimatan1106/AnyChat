﻿$(function () {
    //通知の許可を求める
    Push.Permission.request();

    //サーバとの接続オブジェクト作成
    var connection = $.hubConnection();

    //Hubのプロキシ・オブジェクトを作成
    var echo = connection.createHubProxy("chatHub");

    //発言された場合に発火
    //発言をdivChatに追加する
    echo.on("enterEscapeTxtChat", function (title, url, speech) {
        $("#divChat").prepend(
            "<div><a href=\"\"><span style=\"font-size:18px\">" + title + "</span></a></div>" +
            "<div><span style=\"color:#006621; font-size:13px;\">" + url + "</span></div>" +
            "<div style=\"margin-bottom: 20px;line-height:100%;\"><span style=\"font-size:13px;\">" + speech + "</span></div>"
        );
    });
    //発言された場合に発火
    //自分以外に発言の通知を行う
    echo.on("pushNotification", function (speech) {
        // hasを使うとboolで許可情報を取得できます
        if (Push.Permission.get() !== Push.Permission.DENIED
            && Push.Permission.has()) {
            Push.create(speech, {
                //body: '更新をお知らせします！',
                //icon: 'icon.png',
                timeout: 5000, // 通知が消えるタイミング
                vibrate: [100, 100, 100], // モバイル端末でのバイブレーション秒数
                onClick: function () {
                    // 通知がクリックされた場合の設定
                    window.focus();
                    this.close();
                }
            });
        }
    });
    //txtChat の入力ボックスでエンターキーが押された場合のイベントハンドラ
    $("#txtSpeech").keypress(function (e) {
        var speech = $("#txtSpeech").val();
        if (e.which == 13
            && speech !== "") {
            echo.invoke("EnterEscapeTxtSpeech", speech);
            echo.invoke("PushNotification", speech);
            $("#txtSpeech").val("");
        }
    });

    //セッションタイムアウトした場合に再接続を行う
    connection.disconnected(function () {
        setTimeout(function () {
            $.connection.hub.start();
        }, 5000); // Restart connection after 5 seconds.
    });

    //接続を開始
    connection.start(function () {
    });
});
