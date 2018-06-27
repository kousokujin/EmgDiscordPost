﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EmgDiscordPost
{
    class ChpController
    {
        chpDB DBcontroller;
        ChpPostController chpController;

        public ChpController(ChpPostController DB, ChpPostController post)
        {
            this.chpController = DB;
            this.chpController = post;
            initOrderword();
            chpController.notifyTime = DBcontroller.nextNotifyTime();

            chpController.notificationChpTime += chpTimeEvent;
        }

        public void initOrderword()
        {
            chpController.addWord("覇者の紋章");
        }

        public void initDB()
        {
            DBcontroller.initDB();
        }

        //覇者の紋章の通知時刻になった時のイベント
        private async void chpTimeEvent(object sender,EventArgs e)
        {
            //データベースから今週の覇者の紋章を取得
            List<string> chpList = DBcontroller.getChpList();
            await chpController.PostChp(chpList);

            //次の覇者の紋章通知時刻
            await Task.Run(() => { chpController.notifyTime = DBcontroller.nextNotifyTime(); } );

        }

        //覇者の紋章の問い合わせが来た時
        private async void orderRes(object sender,EventArgs e)
        {
            if(e is ReceiveData)
            {
                ReceiveData d = e as ReceiveData;
                List<string> chpList = DBcontroller.getChpList();
                await chpController.PostChp(chpList, d.Author);
            }
        }
    }
}
