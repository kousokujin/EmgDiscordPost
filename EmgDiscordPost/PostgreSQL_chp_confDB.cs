using System;
using System.Collections.Generic;
using System.Text;

namespace EmgDiscordPost
{
    class PostgreSQL_chp_confDB : postgreSQL,IChpTimeDB
    {
        string tablename;

        public PostgreSQL_chp_confDB(string address,string DBname,string user,string password) : base(address, DBname, user, password)
        {
            tablename = "chpTimeTable";
        }

        public void setTable(string tablename)
        {
            this.tablename = tablename;
        }

        public void createtable()
        {
            logOutput.writeLog("覇者の紋章時刻テーブルを作成します。");
            string que = string.Format("CREATE TABLE {0} (ID int primary key,week int,hour int,min int,sec int);", tablename);
            command(que);
        }

        public void droptable()
        {
            logOutput.writeLog("覇者の紋章時刻テーブルを削除します。");
            command(string.Format("DROP TABLE {0};", tablename));
        }

        public void cleartable()
        {
            logOutput.writeLog("覇者の紋章時刻テーブルの内容を削除します。");
            command(string.Format("TRUNCATE table {0} restart identity;", tablename));
        }

        //データベースから通知時間を取得
        public List<DateTime> getNotifyTime()
        {
            string que = string.Format("SELECT week,hour,min,sec FROM {0};", tablename);
            List<List<object>> table = selectQue(que);
            List<DateTime> outputList = new List<DateTime>();

            foreach(List<object> objList in table)
            {
                int week = (int)objList[0];
                int hour = (int)objList[1];
                int min = (int)objList[2];
                int sec = (int)objList[3];

                if(week != 7)
                {
                    outputList.Add(calcChpTime(week, hour, min, sec));
                }
                else //毎日
                {
                    for(int i = 0; i < 7; i++)
                    {
                        outputList.Add(calcChpTime(i, hour, min, sec));
                    }
                }
            }

            return outputList;
        }

        public void addChpTable(int week, int hour,int min,int sec)
        {
            //IDを決定する
            string getIDque = string.Format("SELECT id FROM {0};", tablename);
            List<List<object>> idTable = selectQue(getIDque);
            int id = 0;
            foreach (List<object> obLst in idTable)
            {
                int tmpid = (int)obLst[0];
                if(id < tmpid)
                {
                    id = tmpid;
                }
            }
            id++;

            //データベースにデータをいれる
            string que = string.Format("INSERT INTO {0} (ID,week,hour,min,sec) VALUES ({1},{2},{3},{4},{5});",
                tablename,
                id,
                week,
                hour,
                min,
                sec);

            command(que);
        }

        private DateTime calcChpTime(int week, int hour, int min, int sec)
        {
            DateTime now = DateTime.Now;
            DateTime tmp = new DateTime(now.Year, now.Month, now.Day, hour, min, sec);
            int nowWeek = (int)now.DayOfWeek;

            if (week > nowWeek)
            {
                tmp += new TimeSpan(week - nowWeek, 0, 0, 0);
            }

            if (week < nowWeek)
            {
                tmp += new TimeSpan(7 - nowWeek + week, 0, 0, 0);
            }

            if (week == nowWeek)
            {
                TimeSpan d = now - tmp;

                if (d.Seconds >= 0)  //正だったら来週
                {
                    tmp += new TimeSpan(7, 0, 0, 0);
                }
            }

            return tmp;
        }
    }
}
