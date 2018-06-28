using System;
using System.Collections.Generic;

namespace EmgDiscordPost
{
    class Program
    {
        static void Main(string[] args)
        {
            //テストコード
            
            string address = "2400:2410:d0e1:6500:f7d3:f0cb:14b4:5dac";
            string user = "docker";
            string pass = "docker";
            string database = "docker";
            string botname = "testApplication";

            Console.Write("Token:");
            string token = Console.ReadLine();
            ulong id = 0;

            //覇者の紋章
            IChpDataRead chpData = new PostgreSQL_ChpRead(address, database, user, pass);
            IChpTimeDB timeData = new PostgreSQL_chp_confDB(address, database, user, pass);
            chpDB chpDBcon = new chpDB(chpData, timeData);

            IemgPost post = new emgPostToDiscord(token, id, botname);
            ChpPostController postCon = new ChpPostController(post);

            ChpController chp = new ChpController(chpDBcon, postCon);
            chp.initDB();
            DateTime notify = DateTime.Now + new TimeSpan(0, 0, 30);
            //timeData.addChpTable((int)notify.DayOfWeek, notify.Hour, notify.Minute, notify.Second);
            //chp.reloadNotifyTime();

            //緊急クエスト
            IemgPost emgPost = new emgPostToDiscord(token, id, botname);
            IEmgDBRead emgDBread = new PostgreSQL_EmgRead(address, database, user, pass);
            IDBConfig dbConfig = new PostgreSQL_configLoader(address, database, user, pass);
            EmgPostController emgPostCon = new EmgPostController(emgPost);
            EmgDBGet emgDB = new EmgDBGet(emgDBread);
            EmgController emgCon = new EmgController(emgPostCon, emgDB, dbConfig);
            dbConfig.droptable();
            dbConfig.createtable();

            Console.ReadLine();
        }

        static void Recive(object sender,EventArgs data)
        {
            if (data is ReceiveData)
            {
                ReceiveData d = data as ReceiveData;
                Console.WriteLine("受信:{0}", d.content);
            }
        }

        static void Replay(object sender, EventArgs data)
        {
            if (data is ReceiveData)
            {
                ReceiveData d = data as ReceiveData;
                Console.WriteLine("リプライ:{0}", d.content);
            }
        }

        static void cancelEv(object sender, EventArgs data)
        {
            if (data is ReceiveData)
            {
                ReceiveData j = data as ReceiveData;
                Console.WriteLine("Cancel:{0}", j.Author);
            }
        }
    }
}
