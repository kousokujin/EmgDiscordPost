using System;
using System.Collections.Generic;
using System.Text;

namespace EmgDiscordPost
{
    class joinPostDiscord : DiscordService,IjoinPost
    {
        //参加表明が出たとき
        public event EventHandler joinEvent;

        //リプライが来たとき
        public event EventHandler replayEvent;

        List<string> joinwords;

        public joinPostDiscord(string token,ulong id,string bot) : base(token, id, bot)
        {
            ReceiveReplay += replayEventProcess;
            joinwords = new List<string>();
            initaddWord();
        }

        public void addWord(string word)
        {
            joinwords.Add(word);
        }

        public void initaddWord()   //参加とするワード
        {
            addWord("参加");
            addWord("参加");
            addWord("行きます");
            addWord("join");

        }

        //Discordにリプライがきたときのイベント
        private void replayEventProcess(object sender, EventArgs e)
        {
            if (e is ReceiveData)
            {
                ReceiveData data = e as ReceiveData;

                foreach (string s in joinwords)  //参加する場合
                {
                    if(s == data.content)
                    {
                        //参加クラス未定で参加
                        joinArg join = new joinArg(data.Author, "", JobClass.None, JobClass.None);
                        joinEvent(this, join);
                        return;
                    }
                }

                //参加クラスを定義した場合の処理
                string content = data.content.Replace("　", " "); //全角スペースを半角スペースに変換
                string[] splitedStr = content.Split(' ');
                string note = "";   //備考

                if(splitedStr.Length > 1)
                {
                    note = splitedStr[1];
                }

                (JobClass mainclass, JobClass subClass) = myFunction.convertJobClass(splitedStr[0]);

                if((mainclass != JobClass.None && subClass != JobClass.None)||(mainclass == JobClass.Hr)) //メインクラスもサブクラスも定義されてる場合
                {
                    joinArg join = new joinArg(data.Author, note, mainclass, subClass);
                    joinEvent(this, join);
                    return;
                }

                //なにもない場合にはリプライイベント
                replayEvent(this, data);

                /*
                if (splitedStr[0].Length >= 2)
                {
                    string classStr = splitedStr[0].ToLower();
                    char[] mainClassChr = { classStr[0], classStr[1] }; //メインクラス文字列
                    string mainClassStr = new string(mainClassChr);

                    JobClass mainclass = convertJobclass(mainClassStr);
                    
                    if(mainclass == JobClass.Hr)    //ヒーローの場合
                    {
                        join = new joinArg(data.Author, note, mainclass, JobClass.None);
                        joinEvent(this, join);
                        return;
                    }

                    if(mainclass == JobClass.None)  //何も割り当てられなかった場合
                    {
                        //リプライとしてイベントを実行
                        ReceiveData rd = new ReceiveData(data.Author, data.content);
                        replayEvent(this, rd);
                        return;
                    }

                    if (splitedStr[0].Length == 4) //サブクラス割当
                    {
                        char[] subClassChr = { classStr[2], classStr[3] }; //サブクラス文字列
                        string subClassStr = new string(subClassChr);

                        JobClass subclass = convertJobclass(subClassStr);

                        if(subclass == JobClass.Hr || subclass == JobClass.None)    //サブクラスがヒーロまたは割当なしの場合は割当なしでエントリー
                        {
                            //リプライとしてイベントを実行
                            ReceiveData rd = new ReceiveData(data.Author, data.content);
                            replayEvent(this, rd);
                            return;
                        }

                        join = new joinArg(data.Author, note, mainclass, subclass);
                        joinEvent(this, join);
                        return;
                    }


                }
                */
            }
        }
    }

    class joinArg : ReceiveData
    {
        public JobClass mainClass;
        public JobClass subClass;

        public joinArg(string author,string content,JobClass mainClass,JobClass subClass) : base(author,content)
        {
            this.mainClass = mainClass;
            this.subClass = subClass;
        }  
    }

    //PSO2職業列挙型
    enum JobClass
    {
        None,Hu,Fi,Ra,Gu,Fo,Te,Br,Bo,Su,Hr
    }
}
