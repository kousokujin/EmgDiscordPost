using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Npgsql;
using NpgsqlTypes;

namespace EmgDiscordPost
{
    class PostgreSQL_joinDB : PostgreSQL_Loader,IJoinMemberDB
    {
        //string tablename;
        int id;

        public PostgreSQL_joinDB(string address, string DBname, string user, string password) : base(address, DBname, user, password)
        {
            tablename = "JoinMember";
            id = 0;
        }

        /*
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
        */

        public void createtable()
        {
            logOutput.writeLog("参加者テーブルを作成します。");
            string queStr = string.Format(
                "CREATE TABLE {0} (ID int primary key,name text,mainclass int,subclass int,note text,jointime timestamp NOT NULL DEFAULT now());",
                tablename);

            command(queStr);
        }

        //戻り値はUPDATEかどうか
        public bool addMember(joinArg member)
        {
            (int key, bool memberd) = isMemberd(member);
            List<NpgsqlParameter> parm = new List<NpgsqlParameter>();
            string que = "";

            if (!memberd)   //初めて追加
            {
                /*
                string values = string.Format("'{0}','{1}','{2}','{3}','{4}','{5}'",
                    id,
                    member.Author,
                    member.mainClass,
                    member.subClass,
                    member.content,
                    DateTime.Now.ToString());
                    */
                que = string.Format("INSERT INTO {0} (ID,name,mainclass,subclass,note,jointime) VALUES (:id,:author,:main,:sub,:note,'{1}');", tablename,DateTime.Now.ToString());
                
                /*
                parm.Add(new NpgsqlParameter("id", NpgsqlDbType.Integer) { Value = id });
                parm.Add(new NpgsqlParameter("author", NpgsqlDbType.Text) { Value = member.Author });
                parm.Add(new NpgsqlParameter("main", NpgsqlDbType.Integer) { Value = member.mainClass });
                parm.Add(new NpgsqlParameter("sub", NpgsqlDbType.Integer) { Value = member.subClass });
                parm.Add(new NpgsqlParameter("note", NpgsqlDbType.Text) { Value = member.content });
                */
                
                parm.Add(new NpgsqlParameter("id", NpgsqlDbType.Integer));
                parm.Add(new NpgsqlParameter("author", NpgsqlDbType.Text));
                parm.Add(new NpgsqlParameter("main", NpgsqlDbType.Integer));
                parm.Add(new NpgsqlParameter("sub", NpgsqlDbType.Integer));
                parm.Add(new NpgsqlParameter("note", NpgsqlDbType.Text));

                parm[0].Value = id;
                parm[1].Value = member.Author;
                parm[2].Value = (int)member.mainClass;
                parm[3].Value = (int)member.subClass;
                parm[4].Value = member.content;

                id++;

                logOutput.writeLog("{0}が参加しました。", member.Author);
            }
            else
            {

                if (member.content != "")   //備考の更新がないとき
                {
                    /*
                    que = string.Format("UPDATE {0} SET mainclass = {1},subclass = {2} WHERE id = {3};",
                                tablename,
                                member.mainClass,
                                member.subClass,
                                id);
                    */

                    que = string.Format("UPDATE {0} SET mainclass = :main ,subclass = :sub WHERE id = :id;",tablename);

                    /*
                    parm.Add(new NpgsqlParameter("id", DbType.Int32) { Value = key });
                    parm.Add(new NpgsqlParameter("main", DbType.Int32) { Value = member.mainClass });
                    parm.Add(new NpgsqlParameter("sub", DbType.Int32) { Value = member.subClass });
                    */

                    parm.Add(new NpgsqlParameter("id", NpgsqlDbType.Integer));
                    parm.Add(new NpgsqlParameter("main", NpgsqlDbType.Integer));
                    parm.Add(new NpgsqlParameter("sub", NpgsqlDbType.Integer));

                    parm[0].Value = key;
                    parm[1].Value = (int)member.mainClass;
                    parm[2].Value = (int)member.subClass;


                }
                else
                {
                    /*
                    que = string.Format("UPDATE {0} SET mainclass = {1},subclass = {2} ,note = {3} WHERE id = {4};",
                                tablename,
                                member.mainClass,
                                member.subClass,
                                member.content,
                                id);
                    */
                    que = string.Format("UPDATE {0} SET mainclass = :main ,subclass = :sub,note = :note WHERE id = :id;", tablename);
                    /*
                    parm.Add(new NpgsqlParameter("id", DbType.Int32) { Value = key });
                    parm.Add(new NpgsqlParameter("main", DbType.Int32) { Value = member.mainClass });
                    parm.Add(new NpgsqlParameter("sub", DbType.Int32) { Value = member.subClass });
                    parm.Add(new NpgsqlParameter("note", DbType.String) { Value = member.content });
                    */

                    parm.Add(new NpgsqlParameter("id", NpgsqlDbType.Integer));
                    parm.Add(new NpgsqlParameter("main", NpgsqlDbType.Integer));
                    parm.Add(new NpgsqlParameter("sub", NpgsqlDbType.Integer));
                    parm.Add(new NpgsqlParameter("note", NpgsqlDbType.Text));

                    parm[0].Value = key;
                    parm[1].Value = (int)member.mainClass;
                    parm[2].Value = (int)member.subClass;
                    parm[3].Value = member.content;

                }

                logOutput.writeLog("{0}を変更しました。",member.Author);
            }

            //command(que);

            List<object> outputobj = new List<object>();
            foreach(NpgsqlParameter p in parm)
            {
                outputobj.Add(p);
            }

            ListParamCommand(que, outputobj);

            return memberd;
        }

        //キャンセルをする
        public void deleteMember(string name)
        {
            string que = string.Format("DELETE FROM {0} WHERE name = :name;", tablename);
            //NpgsqlParameter parm = new NpgsqlParameter("name", DbType.String) { Value = name };
            NpgsqlParameter parm = new NpgsqlParameter("name", NpgsqlDbType.Text);
            parm.Value = name;
            List<object> ListParam = new List<object>();
            ListParam.Add(parm);

            ListParamCommand(que, ListParam);

            logOutput.writeLog("{0}の参加をキャンセルしました。", name);

        }

        //メンバー一覧を取得
        public List<joinArg> getMemberList()
        {
            string que = string.Format("SELECT name,mainclass,subclass,note FROM {0} ;", tablename);
            List<List<object>> res = selectQue(que);
            List<joinArg> outList = new List<joinArg>();

            foreach(List<object> lstObj in res)
            {
                string name = (string)lstObj[0];
                JobClass mainclass = (JobClass)((int)lstObj[1]);
                JobClass subClass = (JobClass)((int)lstObj[2]);
                string note = (string)lstObj[3];

                joinArg tmp = new joinArg(name, note, mainclass, subClass);
                outList.Add(tmp);
            }

            return outList;
        }

        private (int key, bool membered) isMemberd(joinArg member)
        {
            string QueStr = string.Format("SELECT ID,name FROM {0} WHERE name = :name;", tablename);
            List<object> para = new List<object>();
            //para.Add(new NpgsqlParameter("name", DbType.String) { Value = member.Author });
            NpgsqlParameter p = new NpgsqlParameter("name", NpgsqlDbType.Text);
            p.Value = member.Author;
            para.Add(p);
            

            List<List<object>> table = selectParamQue(QueStr,para);
            bool isMember = false;
            int id = 0;

            foreach (List<object> o in table)
            {
                foreach(object obj in o)
                {
                    int tempID = 0;
                    string tempMember = "";

                    if(obj is string)
                    {
                        tempMember = obj as string;
                    }

                    if(obj is int)
                    {
                        int NulltempID = (int)obj;
                    }

                    if(tempMember == member.Author)
                    {
                        id = tempID;
                        isMember = true;
                    }
                }
            }

            return (id, isMember);
        }
    }
}
