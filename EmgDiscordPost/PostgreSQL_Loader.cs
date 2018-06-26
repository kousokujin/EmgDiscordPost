using System;
using System.Collections.Generic;
using System.Text;

namespace EmgDiscordPost
{
    class PostgreSQL_Loader : postgreSQL,IDBLoader
    {
        protected string tablename;
        
        public PostgreSQL_Loader(string address,string DBname,string user,string password) : base(address, DBname, user, password)
        {
            tablename = "";
        }

        public virtual void setTable(string name)
        {
            this.tablename = name;
        }

        public virtual void droptable()
        {
            logOutput.writeLog("{0}を削除します。",tablename);
            command(string.Format("DROP TABLE {0};", tablename));
        }

        public virtual void cleartable()
        {
            logOutput.writeLog("{0}をクリアします。",tablename);
            string que = string.Format("truncate table {0} restart identity", tablename);
            command(que);
        }

    }
}
