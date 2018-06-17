using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EmgDiscordPost
{
    abstract class AbstractDBController
    {
        protected string tablename;
        public IDatabase db;

        public AbstractDBController(IDatabase db,string tablename)
        {
            this.db = db;
            this.tablename = tablename;
            //db.connect();
        }

        //データベースにテーブルを設定
        abstract protected void setDBtable();
    }
}
