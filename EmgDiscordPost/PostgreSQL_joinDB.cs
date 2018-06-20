using System;
using System.Collections.Generic;
using System.Text;

namespace EmgDiscordPost
{
    class PostgreSQL_joinDB : postgreSQL
    {
        string tablename;
        int id;

        public PostgreSQL_joinDB(string address,string DBname,string user,string password) : base(address, DBname, user, password)
        {
            tablename = "JoinMember";
            id = 0;
        }

        public void setTable(string tableName)
        {
            this.tablename = tableName;
        }

        public void cleartable()
        {
            logOutput.writeLog("参加者テーブル内容を削除します。");
            string que = string.Format("truncate table {0} restart identity", tablename);
            command(que);
        }

        public void droptable()
        {
            logOutput.writeLog("参加者テーブルを削除します。");
            command(string.Format("DROP TABLE {0};", tablename));
        }

        public void createtable()
        {
            logOutput.writeLog("参加者テーブルを作成します。");
            string queStr = string.Format(
                "CREATE TABLE {0} (ID int primary key,name text,mainclass int,subclass int,note text,jointime timestamp);",
                tablename);

            command(queStr);
        }

        public void addMember(joinArg member)
        {
            string values = string.Format("'{0}','{1}','{2}','{3}','{4}','{5}'",
                id,
                member.Author,
                member.mainClass,
                member.subClass,
                member.content,
                DateTime.Now.ToString());
            string que = string.Format("INSERT INTO {0} (ID,name,mainclass,subclass,note,jointime VALUES ({1});",tablename,values);

            id++;
        }
    }
}
