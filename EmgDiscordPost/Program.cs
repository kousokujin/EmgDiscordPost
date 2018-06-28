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
            
            Console.Write("Token:");
            string token = Console.ReadLine();
            ulong id = 000000000;

            IChpDataRead chpData = new PostgreSQL_ChpRead(address, database, user, pass);
            IChpTimeDB timeData = new PostgreSQL_chp_confDB(address, database, user, pass);
            chpDB chpDBcon = new chpDB(chpData, timeData);

            IemgPost post = new emgPostToDiscord(token, id, "testApplication");
            ChpPostController postCon = new ChpPostController(post);

            ChpController chp = new ChpController(chpDBcon, postCon);

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
