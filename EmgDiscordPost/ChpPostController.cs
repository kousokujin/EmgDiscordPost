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

        public event EventHandler notificationChpTime;  //覇者の紋章の通知の時間になった時のイベント
        public event EventHandler orderEvent;   //覇者の紋章の問い合わせが来た時

        Task loop;

        //覇者の紋章だけどIemgPostを流用
        public ChpPostController(IemgPost service) : base(service)
        {
            this.chpService = service;
            this.service = service;
            notifyTime = new DateTime();
            EventLoop = false;

            chpService.OrderEmg += orderRecieve;

            loop = StartLoop();
        }

        //覇者の紋章をPOST
        public async Task PostChp(List<string> chpList)
        {
            string postStr = generateListChpStr(chpList);
            await chpService.PostAsync(postStr);
        }

        //覇者の紋章のリプライ
        public async Task ReplayChp(List<string> chpList,User u)
        {
            string postStr = generateListChpStr(chpList);
            await chpService.ReplayAsync(postStr, u);
            //await 
        }

        private string generateListChpStr(List<string> chpList)
        {
            string Str = "今週の覇者の紋章キャンペーンは以下の通りです\n";

            int count = 0;

            foreach (string s in chpList)
            {
                Str += s;
                count++;

                if (count != chpList.Count)
                {
                    Str += '\n';
                }
            }

            return Str;
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

        //オーダーが来たとき
        private void orderRecieve(object sender,EventArgs e)
        {
            orderEvent?.Invoke(sender, e);
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
