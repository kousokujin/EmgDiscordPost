# EmgDiscordPost
[PSO2emergencyGetter](https://github.com/kousokujin/PSO2emergencyGetter)によって取得された覇者の紋章・緊急クエストの情報に応じてDiscordに通知を行うアプリケーション。

# 使い方
起動するとPostgreSQLサーバのアドレスとデータベースなどが聞かれるので入力していく。
設定ファイルを記述し、起動時に設定ファイルの場所を引数で指定することで省略が可能。

# 設定ファイルの書き方
```shell
server = [データベースのアドレス]
database = [データベース名]
user = [データベースのユーザー名]
password = [データベースのパスワード]
botname = [Discordのアプリケーション名]
channel = [緊急クエストの通知などを行うDiscordのチャンネルID]
token = [botのトークン]
init = [true/false]
```
initはデータベースの初期化を行うかどうかを指定。

# Discord Botの登録
1.[こちら](https://discordapp.com/developers/applications/me)のサイトでDiscord Botを作成する。

2.「Create a Bot User」ボタンを押して、表示されるtokenの文字列をどっかに記録。

3.ClientIDを探してhttp://discordapp.com/oauth2/authorize?client_id=[clientID]&scope=bot&permissions=0 にアクセスしてbotを登録。[Client ID]にClientIDをいれる。

# 緊急クエストの参加など
Discordのbot宛に「参加」などとリプライを送る。クラスで指定して参加することも可能。

例: @[botの名前] FoTe  -> フォーステクターで参加

なお緊急クエストへの参加は緊急クエストが始まる30分前から可能。
# ダウンロード
[ダウンロード](https://github.com/kousokujin/EmgDiscordPost/releases)

# Licence
Copyright (c) 2018 kousokujin.

Released under the MIT license.
