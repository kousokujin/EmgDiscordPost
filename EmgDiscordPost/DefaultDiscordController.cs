using System;
using System.Collections.Generic;
using System.Text;

namespace EmgDiscordPost
{
    //このbotで処理できない文字列について応答する
    class DefaultDiscordController  :   AbstractServiceController
    {
        List<AbstractServiceController> other;
        Random rnd;

        public DefaultDiscordController(IPostService service,List<AbstractServiceController> otherservice) : base(service)
        {
            this.other = otherservice;
            service.ReceiveReplay += replayEvent;
            //シード値は適当
            rnd = new Random(DateTime.Now.Day * DateTime.Now.Second);
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
                        service.PostAsync(randomWord());
                        return;
                        
                }
            }
        }

        private string randomWord()
        {
            int random = rnd.Next() % 7;

            switch (random)
            {
                case 0:
                    return "何言ってんだこいつ";
                case 1:
                    return ":sunglasses:";
                case 2:
                    return "酒井様かっこいい！！";
                case 3:
                    return "おっぱい";
                case 4:
                    return "https://www.google.com/search?q=%E7%84%BC%E8%82%89&client=firefox-b&source=lnms&tbm=isch&sa=X&ved=0ahUKEwjZ5oKP54ncAhXWzmEKHVrJAZoQ_AUICygC&biw=1447&bih=917";
                case 5:
                    return "苦情は@Arisan_04までおねがいします。";
                case 6:
                    return ":x:";
                default:
                    return "http://pso2.jp/";
            }
        }
    }
}
