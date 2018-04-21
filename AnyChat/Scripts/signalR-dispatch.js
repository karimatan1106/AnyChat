$(function () {
    // 許可情報を取得
    Push.Permission.get(); // denied || granted || denied
    Push.Permission.request();
    //サーバとの接続オブジェクト作成
    var connection = $.hubConnection();

    //Hubのプロキシ・オブジェクトを作成
    var echo = connection.createHubProxy("chatHub");

    //サーバから呼び出される関数を登録
    echo.on("enterEscapeTxtChat", function (title, url, comment) {
        $("#divChat").prepend(
            "<div><a href=\"\"><span style=\"font-size:18px\">" + title + "</span></a></div>" +
            "<div><span style=\"color:#006621; font-size:13px;\">" + url + "</span></div>" +
            "<div style=\"margin-bottom: 20px;line-height:100%;\"><span style=\"font-size:13px;\">" + comment + "</span></div>"
        );
    });

    //txtChat の入力ボックスでエンターキーが押された場合のイベントハンドラ
    $("#txtSpeech").keypress(function (e) {
        var speech = $("#txtSpeech").val();
        if (e.which == 13
            && speech !== "") {
            echo.invoke("EnterEscapeTxtChat", speech);

            // hasを使うとboolで許可情報を取得できます
            if (Push.Permission.get() !== Push.Permission.DENIED
                && Push.Permission.has()) {
                Push.create(speech, {
                    //body: '更新をお知らせします！',
                    //icon: 'icon.png',
                    timeout: 3000, // 通知が消えるタイミング
                    vibrate: [100, 100, 100], // モバイル端末でのバイブレーション秒数
                    onClick: function () {
                        // 通知がクリックされた場合の設定
                        this.close();
                    }
                });
            }

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

    var escapeHTML = function (val) {
        return $('<div />').text(val).html();
    };
});
