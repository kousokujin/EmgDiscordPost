using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EmgDiscordPost
{
    class EmgPostController : AbstractServiceController
    {
        IemgPost emgService;

        //イベント
        public event EventHandler notificationTime; //60,30,0の通知時刻になった時(実際は30分毎)
        public event EventHandler todayEmgOrder; //今日の緊急クエストの問い合わせが来た時
        public event EventHandler tomorrowEmgOrder; //明日の緊急クエストの問い合わせが来た時
        public event EventHandler changeDay;    //日付が変わった時
        public event EventHandler MaintainTime; //水曜日17時
        public event EventHandler LodosNotify;   //バル・ロドスの通知の日

        //次の通知の時刻
        private DateTime nextNotify;
        //次の日付が変わった時の通知時間
        private DateTime nextDay;
        //次のメンテナンス後の通知
        private DateTime Maintain;
        //次のバル・ロドスの通知
        private DateTime LodosDay;


        //イベントループ
        private bool loop;

        public EmgPostController(IemgPost service) : base(service)
        {
            this.emgService = service;
            InitaddWords();
            setNextNotify();
            setNextDay();
            setLodosDay();

            service.OrderEmg += OrderReplay;
            //service.addFillter(evFill);

            loop = false;
            Task t = StartLoop();
        }

        //リプライが来た時のフィルター
        /*
        private bool evFill(string content)
        {
            string[] contentSprit = content.Replace("　", " ").Split(' '); //全角スペースを半角スペースに変換
            (JobClass mainclass, JobClass subclass) = myFunction.convertJobClass(contentSprit[0]);
            return (mainclass != JobClass.None && subclass != JobClass.None) || (mainclass == JobClass.Hr);
        }
        */

        //緊急クエストが始まる時に使う
        public async Task postEmgTime(emgQuest emg,int interval)
        {
            if (interval == 0)
            {
                await emgService.PostAsync(string.Format("【緊急開始】{0} {1}", emg.eventTime.ToString("HH:mm"), emg.eventName));
            }
            else
            {
                string liveStr = myFunction.getLiveEmgStr(emg);
                await emgService.PostAsync(string.Format("【{0}分前】{1} {2}", interval, emg.eventTime.ToString("HH:mm"), liveStr));
            }
        }

        //緊急クエストの一覧
        public async Task postListEmg(List<EventData> data, DateTime time, bool Lodos = false)
        {
            if (data.Count == 0)
            {
                return;
            }

            string postStr = string.Format("{0}の緊急クエストは以下の通りです。\n", time.ToString("M月d日"));

            int count = 0;
            foreach (EventData e in data)
            {
                if (e is emgQuest)
                {
                    postStr += e.eventName;
                }
                count++;

                if (count != data.Count)
                {
                    postStr += '\n';
                }
            }

            if (Lodos && LodosCalculator.calcRodosDay(time))
            {
                postStr += '\n';
                postStr += "本日はデイリーオーダー「バル・ロドス討伐(VH)」の日です。";
            }

            await emgService.PostAsync(postStr);
        }

        public async Task LodosPost()
        {
            string postStr = string.Format("デイリーオーダー「バル・ロドス討伐(VH)」の日があと30分で終わります。\n オーダーは受注しましたか？次回のバル・ロドス討伐(VH)の日は{0}です。",
                LodosCalculator.nextRodosDay(DateTime.Now + new TimeSpan(1, 0, 0, 0)));
            await emgService.PostAsync(postStr);
        }

        //初期問い合わせのワードを追加
        private void InitaddWords()
        {
            addWord("今日の緊急");
            addWord("明日の緊急");
        }

        override public void addWord(string word)
        {
            emgService.addOrderword(word);
        }

        public override bool isWord(string word)
        {
            bool isOutput = false;

            foreach(string s in emgService.getOrderwords())
            {
                if(s == word)
                {
                    isOutput = true;
                }
            }

            return isOutput;
        }
        //次の通知時間
        private void setNextNotify()
        {
            DateTime now = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, 0, 0);
            TimeSpan ts30 = new TimeSpan(0, 30, 0);

            if(DateTime.Now.Minute > 30)
            {
                now += ts30;
            }

            nextNotify = now + ts30;
        }

        //日付が変わった時の通知時間
        private void setNextDay()
        {
            DateTime now = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            TimeSpan ts1day = new TimeSpan(1, 0, 0, 0);

            nextDay = (now + ts1day);
        }

        //次のメンテナンス後の通知
        private void setNextmaintain()
        {
            int getDays = 7 - ((int)DateTime.Now.DayOfWeek + 4) % 7;

            if (getDays == 7)   //水曜日の時
            {
                DateTime dt1700 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 17, 0, 0);   //今日の17:00
                if (DateTime.Now < dt1700)
                {
                    getDays = 0;
                }
            }

            Maintain = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 17, 0, 0) + new TimeSpan(getDays, 0, 0, 0);
        }

        //バル・ロドスの通知の日の設定
        private void setLodosDay()
        {
            DateTime nextLodos;

            if (LodosCalculator.calcRodosDay(DateTime.Now))
            {
                if(DateTime.Now > new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 30, 0)) //23:30以降
                {
                    nextLodos = LodosCalculator.nextRodosDay(DateTime.Now + new TimeSpan(1, 0, 0, 0));
                }
                else
                {
                    nextLodos = LodosCalculator.nextRodosDay(DateTime.Now);
                }
            }
            else
            {
                nextLodos = LodosCalculator.nextRodosDay(DateTime.Now);
            }

            LodosDay = nextLodos;
        }

        private void EventLoop()    //イベントループ
        {
            while (loop)
            {
                if(DateTime.Now > nextNotify)
                {
                    notificationTime?.Invoke(this, new notificationTime(nextNotify));
                    setNextNotify();
                }

                if(DateTime.Now > nextDay)
                {
                    changeDay?.Invoke(this, new notificationTime(nextDay));
                    setNextDay();
                }

                if(DateTime.Now > Maintain)
                {
                    MaintainTime?.Invoke(this, new notificationTime(Maintain));
                    setNextmaintain();
                }

                if(DateTime.Now > LodosDay)
                {
                    LodosNotify?.Invoke(this, new notificationTime(LodosDay));
                    setLodosDay();
                }

                System.Threading.Thread.Sleep(1000);
            }
        }

        private async Task StartLoop()
        {
            loop = true;
            await Task.Run(() => { EventLoop(); });
        }

        //emgServiceから問い合わせが来た時のイベント
        public void OrderReplay(object sender,EventArgs e)
        {
            if (e is ReceiveData)
            {
                ReceiveData d = e as ReceiveData;

                if (d.content == "今日の緊急")
                {
                    todayEmgOrder?.Invoke(sender, d);
                }

                if(d.content == "明日の緊急")
                {
                    tomorrowEmgOrder?.Invoke(sender, d);
                }
            }
        }
    }

    class notificationTime : EventArgs
    {
        public DateTime time;

        public notificationTime(DateTime dt)
        {
            time = dt;
        }
    }
}
