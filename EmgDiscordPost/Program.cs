using System;
using System.Collections.Generic;

namespace EmgDiscordPost
{
    class Program
    {
        static void Main(string[] args)
        {
            //テストコード

            /*
            string address = "2400:2410:d0e1:6500:f7d3:f0cb:14b4:5dac";
            string user = "docker";
            string pass = "docker";
            string database = "docker";

            
            IChpDataRead chp = new PostgreSQL_ChpRead(address,database,user,pass);
            List<string> lst = chp.getChpList();

            foreach(string s in lst)
            {
                Console.WriteLine(s);
            }
            

            IEmgDBRead emg = new PostgreSQL_EmgRead(address, database, user, pass);
            DateTime start = DateTime.Now;
            DateTime end = new DateTime(start.Year, start.Month, start.Day, 0, 0, 0);

            end += new TimeSpan(24, 0, 0);
            List<EventData> emData = emg.getEmgList(start, end);

            foreach(EventData e in emData)
            {
                Console.WriteLine("[{0}]{1}", e.eventTime.ToString(), e.eventName);
            }
            
            Console.ReadLine();
            */

            string token = Console.ReadLine();
            ulong channelID = 348314939934375938;
            string BOT_NAME = "testApplication";

            DiscordService discord = new DiscordService(token, channelID, BOT_NAME);

            discord.ReceiveEvent += Recive;
            discord.ReceiveReplay += Replay;
            string content = Console.ReadLine();
            discord.postStr(content);

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
    }
}
