using System;
using System.Collections.Generic;
using System.Text;
using Npgsql;

namespace EmgDiscordPost
{
    class PostgreSQL_ChpRead : postgreSQL, IChpDataRead
    {
        string tablename;

        public PostgreSQL_ChpRead(string address, string DBname, string user, string password) : base(address, DBname, user, password)
        {
            tablename = "PSO2ChpTable";
        }

        public void setTable(string table)
        {
            this.tablename = table;
        }

        public List<string> getChpList()
        {
            List<string> outputStr = new List<string>();

            string que = string.Format("SELECT id, chpname FROM {0};",tablename);
            List<List<object>> outtable = selectQue(que);

            foreach(List<object> o in outtable)
            {
                outputStr.Add(o[1] as string);
            }
            return outputStr;
        }
    }
}
