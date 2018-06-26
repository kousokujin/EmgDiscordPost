using System;
using System.Collections.Generic;
using System.Text;

namespace EmgDiscordPost
{
    class PostgreSQL_configLoader : PostgreSQL_Loader,IDBConfig
    {
        public PostgreSQL_configLoader(string address,string DBname,string user,string password) : base(address, DBname, user, password)
        {
            tablename = "config";
        }

        public void createtable()
        {
            logOutput.writeLog("設定テーブルを作成します。");
            string que = string.Format("CREATE TABLE {0} (item text primary key,value text);", tablename);
            command(que);
        }

        public void updateValue(string item,string value)
        {
            //なぞ
            string que = string.Format("UPDATE {0} SET value = {1} WHERE item = {2}; INSERT INTO {0} (item,value) SELECT {1},{2} WHERE NOT EXISTS (SELECT item FROM {0} WHERE item = {2});", tablename, value, item);
            command(que);
            logOutput.writeLog("設定を更新しました。");
        }

       
    }
}
