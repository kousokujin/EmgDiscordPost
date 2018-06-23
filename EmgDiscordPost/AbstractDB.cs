using System;
using System.Collections.Generic;
using System.Text;

namespace EmgDiscordPost
{
    abstract class AbstractDB : IDatabase
    {
        protected string address;
        protected string DBname;
        protected string user;
        protected string password;

        public AbstractDB(string address,string DBname,string user,string password)
        {
            this.address = address;
            this.DBname = DBname;
            this.user = user;
            this.password = password;

            //connect();
        }

        //データベースへの接続
        public abstract object connect();

        //データベースから切断
        public abstract int disconnect(object obj);

        //Queryの実行
        public abstract object command(string que);

        //パラメータを指定してQuery実行
        public abstract object ListParamCommand(string que, List<object> par);

        //SELECT文
        //public abstract List<List<object>> selectQue(string que);
        public virtual List<List<object>> selectQue(string que)
        {
            List<object> par = new List<object>();
            return selectParamQue(que, par);
        }

        //SELECT文(パラメータ指定)
        public abstract List<List<object>> selectParamQue(string que, List<object> par);

        public abstract string getDBType();

        public object ParamCommand(string que,params object[] par)
        {
            List<object> objList = new List<object>();

            foreach(object o in par)
            {
                objList.Add(o);
            }

            return ListParamCommand(que, objList);
        }

    }
}
