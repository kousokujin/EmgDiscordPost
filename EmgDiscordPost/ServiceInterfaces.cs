using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EmgDiscordPost
{
    interface IPostService
    {
        void postStr(string content);
        void connect();
        void disconnect();
        Task PostAsync(string content);
    }

    interface IemgPost : IPostService
    {
        //緊急クエストの問い合わせが来た時のイベント
        event EventHandler OrderEmg;
        void addFillter(ReplayFillter fillter);
        void addOrderword(string word);


        //緊急クエストが始まる前・始まった時
        //Task postEmgTime(emgQuest emg,int interval);

        //緊急クエストの一覧を投稿
        //Task postListEmg(List<EventData> data, DateTime time, bool Lodos);

    }

    interface IjoinPost : IPostService
    {
        //リプライが来たとき
        event EventHandler replayEvent;

        //参加表明イベント
        event EventHandler joinEvent;

        //キャンセルが出た時
        event EventHandler cancelEvent;

        void addWord(string word);
        void addCancelword(string word);
    }
}
