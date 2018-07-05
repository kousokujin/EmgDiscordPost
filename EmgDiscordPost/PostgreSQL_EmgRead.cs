using System;
using System.Collections.Generic;
using System.Text;

namespace EmgDiscordPost
{
    class PostgreSQL_EmgRead : PostgreSQL_Loader, IEmgDBRead
    {
        //string tablename;

        public PostgreSQL_EmgRead(string address, string DBname, string user, string password) : base(address, DBname, user, password)
        {
            tablename = "PSO2EventTable";
        }


        /*
        public void setTable(string t)
        {
            this.tablename = t;
        }
        */

        public List<EventData> getEmgList(DateTime start,DateTime end)
        {
            List<EventData> outputData = new List<EventData>();
            DateTime fixStart;
            DateTime fixEnd;

            if(start > end) //startのほうが遅い
            {
                fixStart = end;
                fixEnd = start;
            }
            else
            {
                fixStart = start;
                fixEnd = end;
            }

            string que = string.Format("SELECT id, emgname,livename,emgtime,emgtype FROM {0} WHERE emgtime >= '{1}' AND emgtime <= '{2}' ORDER BY emgtime ASC;", tablename,fixStart.ToString(),fixEnd.ToString());
            List<List<object>> outtable = selectQue(que);

            int count = 0;
            foreach (List<object> o in outtable)
            {
                int? Ntype = o[4] as int?;
                
                string emgname = o[1] as string;
                string livename = o[2] as string;
                DateTime? Ntime = o[3] as DateTime?;
                count++;

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

            //Console.WriteLine("getDB:Start{0},end:{1}",start.ToString("HH:mm:ss"),end.ToString("HH:mm:ss"));
            return outputData;

        }
    }
}
