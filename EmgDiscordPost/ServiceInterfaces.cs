﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EmgDiscordPost
{
    interface IPostService
    {
        void postStr(string content);
        void sendReplay(string content, User user);
        void connect();
        void disconnect();
        Task PostAsync(string content);
        Task ReplayAsync(string content, User user);
        event EventHandler ReceiveEvent;
        event EventHandler ReceiveReplay;
    }

    interface IemgPost : IPostService
    {
        //緊急クエストの問い合わせが来た時のイベント
        event EventHandler OrderEmg;
        //void addFillter(ReplayFillter fillter);
        void addOrderword(string word);
        List<string> getOrderwords();


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

        //参加者の問い合わせが来た時
        event EventHandler showMember;

        void addWord(string word);
        void addCancelword(string word);
        void addListword(string word);
        List<string> getOrderwords();
    }
}
