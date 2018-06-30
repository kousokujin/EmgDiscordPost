using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EmgDiscordPost
{
    class ChpController
    {
        chpDB DBcontroller;
        ChpPostController chpController;

        public ChpController(chpDB DB, ChpPostController post)
        {
            this.DBcontroller = DB;
            this.chpController = post;
            initOrderword();
            chpController.notifyTime = DBcontroller.nextNotifyTime();

            chpController.notificationChpTime += chpTimeEvent;
            chpController.orderEvent += orderRes;
        }

        public void initOrderword()
        {
            chpController.addWord("覇者の紋章");
            chpController.addWord("覇者");
        }

        public void initDB()
        {
            DBcontroller.initDB();
        }

        public void reloadNotifyTime()
        {
            chpController.notifyTime = DBcontroller.nextNotifyTime();
        }

        //覇者の紋章の通知時刻になった時のイベント
        private async void chpTimeEvent(object sender,EventArgs e)
        {
            //データベースから今週の覇者の紋章を取得
            List<string> chpList = DBcontroller.getChpList();
            await chpController.PostChp(chpList);

            //次の覇者の紋章通知時刻
            await Task.Run(() => { reloadNotifyTime(); } );

        }

        //覇者の紋章の問い合わせが来た時
        private async void orderRes(object sender,EventArgs e)
        {
            /*
            if(e is ReceiveData)
            {
                ReceiveData data = e as ReceiveData;
                User u = new User(data.Author);
                List<string> chpList = DBcontroller.getChpList();
                await chpController.ReplayChp(chpList, u);
            }
            */

            //データベースから今週の覇者の紋章を取得
            List<string> chpList = DBcontroller.getChpList();
            await chpController.PostChp(chpList);
            await Task.Run(() => { reloadNotifyTime(); });
        }
    }
}
