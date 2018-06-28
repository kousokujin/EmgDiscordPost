using System;
using System.Collections.Generic;
using System.Text;
using Npgsql;

namespace EmgDiscordPost
{
    class postgreSQL : AbstractDB
    {
        //private NpgsqlConnection connection;

        public postgreSQL(string address, string DBname, string user, string password) : base(address,DBname,user,password)
        {

        }

        public override object connect()
        {
            string connectStr = string.Format("Server={0};Port=5432;User Id={1};Password={2};Database={3};", address, user, password, DBname);
            NpgsqlConnection connection = new NpgsqlConnection(connectStr);

            try
            {
                connection.Open();
            }
            catch (System.Net.Sockets.SocketException)
            {
                logOutput.writeLog("データベースへの接続に失敗しました。");
                return 1;
            }
            catch (System.TimeoutException)
            {
                logOutput.writeLog("タイムアウトしました。");
                return 1;
            }

            //logOutput.writeLog("データベースに接続しました。");
            return connection;
        }

        public NpgsqlConnection NpgConnect()
        {
            object objConnect = connect();
            NpgsqlConnection np = objConnect as NpgsqlConnection;

            return np;
        }

        public override int disconnect(object obj)
        {
            if (obj is NpgsqlConnection)
            {
                NpgsqlConnection con = obj as NpgsqlConnection;
                con.Close();
            }
            return 0;
        }

        public override object command(string que)
        {
            if(que == "")
            {
                return 1;
            }

            using (NpgsqlConnection con = NpgConnect())
            {
                using (NpgsqlCommand command = new NpgsqlCommand(que, con))
                {
                    if (con != null)
                    {

                        try
                        {
                            var result = command.ExecuteReader();
                            disconnect(con);
                            return result;
                        }
                        catch (Npgsql.PostgresException e)
                        {
                            logOutput.writeLog("SQLを実行できません。({0})", e.MessageText);
                            return 1;
                        }
                    }
                    else
                    {
                        logOutput.writeLog("データベースへのクエリの実行ができません。");
                        return 1;
                    }
                }
            }
        }

        public override List<List<object>> selectQue(string que)
        {
            List<object> par = new List<object>();
            return selectParamQue(que, par);
        }

        public override List<List<object>> selectParamQue(string que,List<object> par)
        {
            List<List<object>> table = new List<List<object>>();
            List<NpgsqlParameter> npgPars = new List<NpgsqlParameter>();

            //パラメータの設定
            foreach(object o in par)
            {
                if(o is NpgsqlParameter)
                {
                    npgPars.Add(o as NpgsqlParameter);
                }
            }

            using(var connect = NpgConnect())
            {
                //connect.Open();

                using(var cmd = new NpgsqlCommand(que, connect))
                {
                    foreach(NpgsqlParameter np in npgPars)
                    {
                        cmd.Parameters.Add(np);
                    }

                    try
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                List<object> coloum = new List<object>();
                                int c = reader.FieldCount;

                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    string filedtype = reader.GetFieldType(i).Name;
                                    object obj = new object();

                                    switch (filedtype)
                                    {
                                        case "Int16":
                                            obj = reader.GetInt16(i);
                                            break;

                                        case "Int32":
                                            obj = reader.GetInt32(i);
                                            break;

                                        case "Int64":
                                            obj = reader.GetInt64(i);
                                            break;

                                        case "String":
                                            obj = reader.GetString(i);
                                            break;

                                        case "DateTime":
                                            obj = reader.GetDateTime(i);
                                            break;


                                    }

                                    if (obj != null)
                                    {
                                        coloum.Add(obj);
                                    }
                                }

                                table.Add(coloum);
                            }
                        }
                    }
                    catch (System.InvalidOperationException)
                    {
                        logOutput.writeLog("データベースの接続に失敗しました。");
                        return table;
                    }
                    catch (Npgsql.PostgresException)
                    {
                        logOutput.writeLog("テーブルが存在しません。");
                        return table;
                    }
                }

                return table;
            }
        }

        public override object ListParamCommand(string que, List<object> par)
        {
            using (NpgsqlConnection connection = NpgConnect())
            {
                using (NpgsqlCommand command = new NpgsqlCommand(que, connection))
                {

                    foreach (object obj in par)
                    {
                        if (obj is NpgsqlParameter)
                        {
                            NpgsqlParameter np = obj as NpgsqlParameter;
                            command.Parameters.Add(np);
                        }
                    }

                    var result = command.ExecuteReader();
                    return result;
                }
            }
            /*
            disconnect(connection);
            return result;
            */

        }

        public override string getDBType()
        {
            return "POSTGRE_SQL";
        }

    }
}
