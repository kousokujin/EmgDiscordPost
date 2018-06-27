using System;
using System.Collections.Generic;
using System.Text;
using Npgsql;

namespace EmgDiscordPost
{
    interface IDatabase
    {
        object connect();
        int disconnect(object obj);
        object ListParamCommand(string que, List<object> par);
        object ParamCommand(string que, params object[] par);
        object command(string que);
        string getDBType();
    }

    interface IDBLoader : IDatabase
    {
        void setTable(string tablename);
        void droptable();
        void cleartable();
    }

    //EventDataのデータをDBに書き込むためのインターフェイス
    interface IEmgDBRead : IDBLoader
    {
        List<EventData> getEmgList(DateTime start, DateTime end);
        //void setTable(string tablename);
    }

    interface IChpDataRead : IDBLoader
    {
        List<string> getChpList();
        //void setTable(string tablename);
    }

    //覇者の紋章通知時間に関するインターフェース
    interface IChpTimeDB : IDBLoader
    {
        /*
        void setTable(string tablename);
        void createtable();
        void droptable();
        void cleartable();
        */
        List<DateTime> getNotifyTime();
        void addChpTable(int week, int hour, int min, int sec);
        void createtable();
    }

    //参加メンバーを管理するデータベースについて
    interface IJoinMemberDB : IDBLoader
    {
        /*
        void cleartable();
        void droptable();
        */
        void createtable();
        bool addMember(joinArg member);
        void deleteMember(string name);
        List<joinArg> getMemberList();
    }

    //データベースから設定を読み込むインターフェイス
    interface IDBConfig : IDBLoader
    {
        void updateValue(string item, string value);
    }


}
