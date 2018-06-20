using System;
using System.Collections.Generic;
using System.Text;

namespace EmgDiscordPost
{
    class joinServiceController : AbstractServiceController
    {
        IjoinPost joinClient;

        //参加表明が出たとき
        public event EventHandler joinEvent;
        //キャンセルが出た時
        public event EventHandler cancelEvent;

        public joinServiceController(IjoinPost joinClient) : base(joinClient)
        {
            this.joinClient = joinClient;
            joinClient.joinEvent += joinEventProcess;
            joinClient.cancelEvent += cancelEventProcess;

            initaddWord();
        }

        public void initaddWord()   //参加・キャンセルワードの追加
        {
            joinClient.addWord("参加");
            joinClient.addWord("行きます");
            joinClient.addWord("join");
            joinClient.addCancelword("キャンセル");
            joinClient.addCancelword("cancel");
        }


        //参加イベント
        private void joinEventProcess(object sender,EventArgs e)
        {
            joinEvent?.Invoke(sender, e);
        }

        //キャンセルイベント
        private void cancelEventProcess(object sender,EventArgs e)
        {
            cancelEvent?.Invoke(sender, e);
        }


    }
}
