using System;
using System.Collections.Generic;
using System.Text;

namespace EmgDiscordPost
{
    class joinPostDiscord : DiscordService
    {
        //参加表明が出たとき
        event EventHandler joinEvent;

        public joinPostDiscord(string token,ulong id,string bot) : base(token, id, bot)
        {
            ReceiveReplay += replayEvent;
        }

        //Discordにリプライがきたときのイベント
        private void replayEvent(object sender, EventArgs e)
        {
            if (e is ReceiveData)
            {
                joinArg join;

                ReceiveData data = e as ReceiveData;
                string content = data.content.Replace("　", " ");//全角スペースを半角スペースに変換
                string[] splitedStr = content.Split(' ');
                string note = "";   //備考

                if(splitedStr[1] != null)
                {
                    note = splitedStr[1];
                }

                if (splitedStr[0].Length >= 2)
                {
                    string classStr = splitedStr[0];
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
                        //クラス割当なしでイベント発生
                        join = new joinArg(data.Author, "", JobClass.None, JobClass.None);
                        joinEvent(this, join);
                        return;
                    }

                    if (splitedStr[0].Length == 4) //サブクラス割当
                    {
                        char[] subClassChr = { classStr[2], classStr[3] }; //サブクラス文字列
                        string subClassStr = new string(subClassChr);

                        JobClass subclass = convertJobclass(subClassStr);

                        if(subclass == JobClass.Hr || subclass == JobClass.None)    //サブクラスがヒーロまたは割当なしの場合は割当なしでエントリー
                        {
                            join = new joinArg(data.Author, "", JobClass.None, JobClass.None);
                            joinEvent(this, join);
                            return;
                        }

                        join = new joinArg(data.Author, "", mainclass, subclass);
                        joinEvent(this, join);
                    }
                }



            }
        }

        private JobClass convertJobclass(string classStr)   //文字列から列挙型へ変換
        {
            JobClass returnClass = JobClass.None;

            switch (classStr)
            {
                case "hu":
                    returnClass = JobClass.Hu;
                    break;
                case "fi":
                    returnClass = JobClass.Fi;
                    break;
                case "ra":
                    returnClass = JobClass.Ra;
                    break;
                case "gu":
                    returnClass = JobClass.Gu;
                    break;
                case "fo":
                    returnClass = JobClass.Fo;
                    break;
                case "te":
                    returnClass = JobClass.Te;
                    break;
                case "br":
                    returnClass = JobClass.Br;
                    break;
                case "bo":
                    returnClass = JobClass.Bo;
                    break;
                case "su":
                    returnClass = JobClass.Su;
                    break;
                case "hr":
                    returnClass = JobClass.Hr;
                    break;
                default:
                    returnClass = JobClass.None;
                    break;
            }

            return returnClass;

        }
    }

    class joinArg : ReceiveData
    {
        JobClass mainClass;
        JobClass subClass;

        public joinArg(string author,string content,JobClass mainClass,JobClass subClass) : base(author,content)
        {
            this.mainClass = mainClass;
            this.subClass = subClass;
        }  
    }

    //PSO2職業列挙型
    enum JobClass
    {
        Hu,Fi,Ra,Gu,Fo,Te,Br,Bo,Su,Hr,None
    }
}
