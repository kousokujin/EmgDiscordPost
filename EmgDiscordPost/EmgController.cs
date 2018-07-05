using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EmgDiscordPost
{
    class EmgController
    {
        EmgPostController post;
        EmgDBGet DBget;
        IDBConfig DBconfig;

        public EmgController(EmgPostController post, EmgDBGet DBget, IDBConfig DBconfig)
        {
            this.post = post;
            this.DBget = DBget;
            this.DBconfig = DBconfig;

            //イベントの登録
            post.notificationTime += EmgEventNotify;
            post.todayEmgOrder += TodayEmgOrder;    //内容がすべて同じなので同じイベントで
            post.changeDay += NextDayEvent;
            post.MaintainTime += TodayEmgOrder;
            post.tomorrowEmgOrder += TomorrowEmgOrder;
            post.LodosNotify += endLodos;

            InitaddWords();
        }


        //初期問い合わせのワードを追加
        private void InitaddWords()
        {
            post.addWord("今日の緊急");
            post.addWord("明日の緊急");
            post.addWord("緊急");
        }

        //イベント

        //30分毎に発生
        private async void EmgEventNotify(object sender,EventArgs e)
        {
            notificationTime time = e as notificationTime;
            EventData ev = DBget.getNextEvent(time.time - new TimeSpan(0,1,0)); //1分だけ前に
            TimeSpan ts30 = new TimeSpan(0, 30, 0);
            TimeSpan ts60 = new TimeSpan(1, 0, 0);

            if (ev is emgQuest)
            {
                if((time.time >= ev.eventTime - ts60) && (time.time < ev.eventTime - ts30))     //60分前
                {
                    await post.postEmgTime(ev as emgQuest, 60);
                }

                if((time.time >= ev.eventTime - ts30) && (time.time < ev.eventTime))    //30分前
                {
                    await post.postEmgTime(ev as emgQuest, 30);
                }

                if((time.time >= ev.eventTime)) //開始
                {
                    await post.postEmgTime(ev as emgQuest, 0);
                }
            }
        }

        //今日の緊急クエスト一覧
        private async void TodayEmgOrder(object sender,EventArgs e)
        {
            (string value,bool isExist) = DBconfig.getValue("Lodos");

            bool Lodos = (isExist && value == "true");
            List<EventData> lst = DBget.getListEvent(DateTime.Now);

            if (lst.Count > 0)
            {
                await post.postListEmg(lst, DateTime.Now, Lodos);
            }
            else
            {
                await post.AsyncPostService("今日の緊急クエストはありません。");
            }
        }

        //日付が変わった時
        private async void NextDayEvent(object sender,EventArgs e)
        {
            (string value, bool isExist) = DBconfig.getValue("Lodos");

            bool Lodos = (isExist && value == "true");
            List<EventData> lst = DBget.getListEvent(DateTime.Now);

            if (lst.Count > 0)  //緊急クエストが1つ以上ある場合だけ投稿
            {
                await post.postListEmg(lst, DateTime.Now, Lodos);
            }
        }

        //明日の緊急クエスト
        private async void TomorrowEmgOrder(object sender,EventArgs e)
        {
            (string value, bool isExist) = DBconfig.getValue("Lodos");
            List<EventData> lst = DBget.getListEvent(DateTime.Now + new TimeSpan(1,0,0,0));

            if (lst.Count > 0)
            {
                await post.postListEmg(lst, DateTime.Now + new TimeSpan(1, 0, 0, 0), false);
            }
            else
            {
                await post.AsyncPostService("明日の緊急クエストはありません。");
            }

            //DBconfig.updateValue("Lodos", "true");
        }

        //バル・ロドスの日がおわる30分前
        private async void endLodos(object sender,EventArgs e)
        {
            (string value, bool isExist) = DBconfig.getValue("Lodos");
            bool Lodos = (isExist && value == "true");

            if(Lodos) await post.LodosPost();
        }

        //バル・ロドス通知のDB設定
        public void setLodos(bool isLodos)
        {
            DBconfig.updateValue("Lodos", isLodos.ToString());
        }
    }
}
