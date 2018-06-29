using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace EmgDiscordPost
{

    //リプライが来た時のフィルター(trueでイベントを実行しない)
    //public delegate bool ReplayFillter(string content);

    class emgPostToDiscord : DiscordService,IemgPost
    {
        //緊急クエスト問い合わせのワード
        List<string> orderwords;
        public event EventHandler OrderEmg;

        //trueの時実行しない
        //public ReplayFillter fillter;

        public emgPostToDiscord(string token, ulong id, string bot) : base(token, id, bot)
        {
            orderwords = new List<string>();
            ReceiveReplay += replayEvent;
        }

        public emgPostToDiscord(DiscordSocketClient client) : base(client)
        {
            orderwords = new List<string>();
            ReceiveReplay += replayEvent;
        }

        public void addOrderword(string word)
        {
            orderwords.Add(word);
        }

        //Discordにリプライがきたときのイベント
        private void replayEvent(object sender, EventArgs e)
        {
            if (e is ReceiveData)
            {
                ReceiveData data = e as ReceiveData;
                bool isOrder = false;

                foreach (string s in orderwords)
                {
                    if (data.content.Contains(s))
                    {
                        isOrder = true;
                    }
                }

               // bool isfillter = fillter(data.content);

                if (isOrder)    //問い合わせワードが含まれていたら
                {
                    OrderEmg?.Invoke(this, data);
                }
            }
        }

        public List<string> getOrderwords()
        {
            return orderwords;
        }

        /*
        public void addFillter(ReplayFillter fillter)
        {
            this.fillter += fillter;
        }
        */
        //緊急クエストが始まる前・始まった時に使う
        /*
        public async Task postEmgTime(emgQuest emg,int interval)
        {
            
            TimeSpan ts30 = new TimeSpan(0, 30, 0);
            TimeSpan ts60 = new TimeSpan(1, 0, 0);
            string eventname = emg.eventName;

            if (emg.live != "")
            {
                eventname = myFunction.getLiveEmgStr(emg);
            }

            if ((emg.eventTime - ts60) < DateTime.Now && (emg.eventTime - ts30) > DateTime.Now) //60分前
            {
                await PostAsync(string.Format("【60分前】{0} {1}", emg.eventTime.ToString("HH:mm"), eventname));
            }

            if (DateTime.Now > (emg.eventTime - ts30) && DateTime.Now < emg.eventTime)   //30分前
            {
                await PostAsync(string.Format("【30分前】{0} {1}", emg.eventTime.ToString("HH:mm"), eventname));
            }

            if (DateTime.Now > emg.eventTime)    //緊急開始
            {
                await PostAsync(string.Format("【30分前】{0} {1}", emg.eventTime.ToString("HH:mm"), emg.eventName));
            }
            

            if(interval == 0)
            {
                await PostAsync(string.Format("【緊急開始】{0} {1}", emg.eventTime.ToString("HH:mm"), emg.eventName));
            }
            else
            {
                await PostAsync(string.Format("【{0}分前】{1} {2}",interval, emg.eventTime.ToString("HH:mm"), emg.eventName));
            }

        }
        */

        //緊急クエストの一覧
        /*
        public async Task postListEmg(List<EventData> data, DateTime time,bool Lodos = false)
        {
            if(data.Count == 0)
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

            if (Lodos)
            {
                postStr += '\n';
                postStr += "本日はデイリーオーダー「バル・ロドス討伐(VH)」の日です。";
            }

            await PostAsync(postStr);
        }
        */
    }
}
