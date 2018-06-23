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

    //EventDataのデータをDBに書き込むためのインターフェイス
    interface IEmgDBRead : IDatabase
    {
        List<EventData> getEmgList(DateTime start, DateTime end);
        void setTable(string tablename);
    }

    interface IChpDataRead : IDatabase
    {
        List<string> getChpList();
        void setTable(string tablename);
    }

    //参加メンバーを管理するデータベースについて
    interface IJoinMemberDB : IDatabase
    {
        void cleartable();
        void droptable();
        void createtable();
        bool addMember(joinArg member);
        void deleteMember(string name);
        List<joinArg> getMemberList();
    }
}
