using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EmgDiscordPost
{
    abstract class AbstractPostService  : IPostService
    {
        //チャンネルに投稿された時
        public event EventHandler ReceiveEvent;

        //リプライを受信した時
        public event EventHandler ReceiveReplay;

        protected bool eventLoopSwitch = false;

        //サービスに文字列を投稿する
        abstract public void postStr(string content);

        //ユーザーに返信
        abstract public void sendReplay(string content, User username);

        abstract public void connect();
        abstract public void disconnect();
        abstract protected (bool, string) isReplay(string message);

        virtual public async Task PostAsync(string content)
        {
            await Task.Run(() =>
            {
                postStr(content);
            });
        }

        virtual public async Task ReplayAsync(string content,User user)
        {
            await Task.Run(() =>
            {
                sendReplay(content, user);
            });
        }

        //イベントの発火
        protected void RunReceiveEvent(object sender,ReceiveData data)
        {
            ReceiveEvent?.Invoke(sender, data);
        }

        protected void RunReplayEvent(object sender,ReceiveData data)
        {
            ReceiveReplay?.Invoke(sender, data);
        }



    }

    class ReceiveData : EventArgs
    {
        public string Author;
        public string content;

        public ReceiveData(string Author, string mes)
        {
            this.content = mes;
            this.Author = Author;
        }
    }

    class User
    {
        public string username;

        public User(string Username)
        {
            this.username = Username;
        }
    }
}
