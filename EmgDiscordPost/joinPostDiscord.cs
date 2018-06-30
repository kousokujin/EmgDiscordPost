using System;
using System.Collections.Generic;
using System.Text;
using Discord.WebSocket;
using Discord;

namespace EmgDiscordPost
{
    class joinPostDiscord : DiscordService,IjoinPost
    {
        //参加表明が出たとき
        public event EventHandler joinEvent;
        //キャンセルが出た時
        public event EventHandler cancelEvent;
        //参加者の問い合わせが来たとき
        public event EventHandler showMember;

        //リプライが来たとき
        public event EventHandler replayEvent;

        List<string> joinwords;
        List<string> cancelword;
        List<string> listwords;

        public joinPostDiscord(string token,ulong id,string bot) : base(token, id, bot)
        {
            ReceiveReplay += replayEventProcess;
            joinwords = new List<string>();
            cancelword = new List<string>();
            listwords = new List<string>();
        }

        public joinPostDiscord(DiscordSocketClient client)  : base(client)
        {
            ReceiveReplay += replayEventProcess;
            joinwords = new List<string>();
            cancelword = new List<string>();
        }

        public void addWord(string word)
        {
            joinwords.Add(word);
        }

        public void addCancelword(string word)
        {
            cancelword.Add(word);
        }

        public void addListword(string word)
        {
            listwords.Add(word);
        }

        public List<string> getOrderwords()
        {
            List<string> output = new List<string>();

            foreach(string s in joinwords)
            {
                output.Add(s);
            }

            foreach(string s in cancelword)
            {
                output.Add(s);
            }

            foreach(string s in listwords)
            {
                output.Add(s);
            }

            return output;
        }

        //Discordにリプライがきたときのイベント
        private void replayEventProcess(object sender, EventArgs e)
        {
            if (e is DiscordReceive)
            {
                 DiscordReceive data = e as DiscordReceive;

                foreach(string s in cancelword) //キャンセルが出た時
                {
                    if(s == data.content)
                    {
                        //キャンセルが出た時
                        cancelEvent?.Invoke(this, data);
                        return;
                    }
                }

                foreach (string s in listwords)  //参加者の問い合わせが来た時
                {
                    if (s == data.content)
                    {
                        showMember?.Invoke(this, data);
                        return;
                    }
                }

                foreach (string s in joinwords)  //参加する場合
                {
                    if(s == data.content)
                    {
                        //参加クラス未定で参加
                        DiscordJoinArg join = new DiscordJoinArg(data.message, client.CurrentUser.Id,JobClass.None, JobClass.None);
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
                    DiscordJoinArg join = new DiscordJoinArg(data.message, client.CurrentUser.Id,mainclass, subClass,note);
                    joinEvent(this, join);
                    return;
                }

                //なにもない場合にはリプライイベント
                replayEvent?.Invoke(this, data);
            }
        }
    }

    
    interface IjoinArg
    {
        JobClass getMainclass();
        JobClass getSubclass();
        string getName();
        string getNote();
    }

    class DiscordJoinArg : DiscordReceive,IjoinArg
    {
        public JobClass mainClass;
        public JobClass subClass;

        public DiscordJoinArg(SocketMessage message,ulong id, JobClass mainClass, JobClass subClass,string note = "") : base(message,id)
        {
            this.mainClass = mainClass;
            this.subClass = subClass;
            content = note;
        }
        
        public JobClass getMainclass()
        {
            return mainClass;
        }

        public JobClass getSubclass()
        {
            return subClass;
        }

        public string getName()
        {
            (string name, string id) = separate();
            return name;
        }

        public string getID()
        {
            (string name, string id) = separate();
            return id;
        }

        public string getNote()
        {
            return content;
        }

        private (string name,string id) separate()
        {
            string[] separate = message.Author.ToString().Split('#');

            return (separate[0], separate[1]);
        }
    }

    /*
    class DiscordJoinArg : joinArg
    {
        public string discordID;

        public DiscordJoinArg(string author, string content, JobClass mainclass, JobClass subclass) : base("", content, mainclass, subclass)
        {
            string[] separate = author.Split('#');
            Author = separate[0];
            discordID = separate[1];
        }
    }
    */

    //PSO2職業列挙型
    enum JobClass
    {
        None,Hu,Fi,Ra,Gu,Fo,Te,Br,Bo,Su,Hr
    }
}
