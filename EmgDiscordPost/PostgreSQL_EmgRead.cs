using System;
using System.Collections.Generic;
using System.Text;

namespace EmgDiscordPost
{
    class PostgreSQL_EmgRead : postgreSQL, IEmgDBRead
    {
        string tablename;

        public PostgreSQL_EmgRead(string address, string DBname, string user, string password) : base(address, DBname, user, password)
        {
            tablename = "PSO2EventTable";
        }


        public void setTable(string t)
        {
            this.tablename = t;
        }

        public List<EventData> getEmgList(DateTime start,DateTime end)
        {
            List<EventData> outputData = new List<EventData>();

            string que = string.Format("SELECT id, emgname,livename,emgtime,emgtype FROM {0} WHERE emgtime >= '{1}' AND emgtime < '{2}' ORDER BY emgtime ASC;", tablename,start.ToString(),end.ToString());
            List<List<object>> outtable = selectQue(que);

            foreach (List<object> o in outtable)
            {
                int? Ntype = o[4] as int?;
                string emgname = o[1] as string;
                string livename = o[2] as string;
                DateTime? Ntime = o[3] as DateTime?;

                //nullチェック
                if(Ntype != null && Ntime != null)
                {
                    EventData ev;

                    int type = (int)Ntype;
                    DateTime time = (DateTime)Ntime;

                    switch (type)
                    {
                        case 1:
                            ev = new casino(time);
                            outputData.Add(ev);
                            break;
                        case 0:
                            if(livename != "")
                            {
                                ev = new emgQuest(time, emgname, livename);
                            }
                            else
                            {
                                ev = new emgQuest(time, emgname);
                            }
                            outputData.Add(ev);
                            break;
                    }
                }


            }

            return outputData;

        }
    }
}
