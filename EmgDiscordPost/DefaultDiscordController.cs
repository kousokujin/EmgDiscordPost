using System;
using System.Collections.Generic;
using System.Text;

namespace EmgDiscordPost
{
    //このbotで処理できない文字列について応答する
    class DefaultDiscordController  :   AbstractServiceController
    {
        List<AbstractServiceController> other;

        public DefaultDiscordController(IPostService service,List<AbstractServiceController> otherservice) : base(service)
        {
            this.other = otherservice;
            service.ReceiveReplay += replayEvent;
        }

        public override bool isWord(string word)
        {
            bool output = true;

            foreach(AbstractServiceController s in other)
            {
                if(s.isWord(word) == true)
                {
                    output = false;
                }
            }

            return output;
        }

        public override void addWord(string word)
        {
            return; //なにもしない
        }

        private void replayEvent(object sender,EventArgs e)
        {
            if (e is ReceiveData) {
                ReceiveData data = e as ReceiveData;

                bool isReplay = isWord(data.content);

                if (isReplay == false) return;

                switch (data.content)
                {
                    case "33-4":
                        service.PostAsync("なんでや！阪神関係ないやろ！");
                        return;
                    case "ｱｱｱｯ":
                        service.PostAsync("ﾚｸﾞｻﾞﾌｫﾝ");
                        return;
                    case "まるめし":
                        service.PostAsync("まるめしはあっち");
                        return;
                    case "変態":
                        service.PostAsync("変態はあっち");
                        return;
                    case "今日の天気":
                        service.PostAsync("自分で調べろ");
                        return;
                    case "明日の天気":
                        service.PostAsync("自分で調べろ");
                        return;
                    case "おっぱい":
                        service.PostAsync("揉んでみたいよな");
                        return;
                    case "ちんぽ":
                        service.PostAsync("ﾎﾞﾛﾝ");
                        return;
                    case "Hey Siri":
                        service.PostAsync("OK Google");
                        return;
                    case "OK Google":
                        service.PostAsync("Hey Siri");
                        return;
                    default:
                        service.PostAsync("何いってんだこいつ");
                        return;
                        
                }
            }
        }
    }
}
