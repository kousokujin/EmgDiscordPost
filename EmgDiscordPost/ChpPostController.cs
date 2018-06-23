using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EmgDiscordPost
{
    class ChpPostController : AbstractServiceController
    {
        IemgPost chpService;
        public DateTime notifyTime;  //覇者の紋章の通知時刻
        bool EventLoop;

        public event EventHandler notificationChpTime;  //覇者の紋章の通知の次官になった時のイベント

        //覇者の紋章だけどIemgPostを流用
        public ChpPostController(IemgPost service, DateTime initNotifyTime) : base(service)
        {
            this.chpService = service;
            notifyTime = initNotifyTime;

            EventLoop = false;
        }

        //覇者の紋章をPOST
        public async Task PostChp(List<string> chpList)
        {
            string postStr = "今週の覇者の紋章キャンペーンは以下の通りです\n";

            int count = 0;

            foreach (string s in chpList)
            {
                postStr += s;
                count++;

                if (count != chpList.Count)
                {
                    postStr += '\n';
                }

                await chpService.PostAsync(postStr);
            }

        }

        public override void addWord(string word)
        {
            chpService.addOrderword(word);   
        }

        public override bool isWord(string word)
        {
            bool isOutput = false;

            foreach (string s in chpService.getOrderwords())
            {
                if (s == word)
                {
                    isOutput = true;
                }
            }

            return isOutput;
        }

        private void Loop() //イベントループ
        {
            while (EventLoop)
            {
               if (DateTime.Now > notifyTime)
               {
                    notificationChpTime?.Invoke(this, new notificationTime(notifyTime));
               }

                System.Threading.Thread.Sleep(1000);
            }
        }

        private async Task StartLoop()
        {
            EventLoop = true;
            await Task.Run(() => {
                Loop();
            });
        }

    }
}
