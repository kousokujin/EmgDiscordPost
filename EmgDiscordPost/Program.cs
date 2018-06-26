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

            IJoinMemberDB db = new PostgreSQL_joinDB(address, database, user, pass);
            joinArg arg = new joinArg("kousokujin", "test", JobClass.Bo, JobClass.Hu);
            db.droptable();
            db.createtable();
            db.addMember(arg);
            arg = new joinArg("kousokujin", "", JobClass.Fo, JobClass.Te);
            db.addMember(arg);
            */

            Console.Write("Token:");
            string token = Console.ReadLine();
            ulong id = 0000000000000;
            joinPostDiscord ser = new joinPostDiscord(token, id, "testApplication");
            ser.joinEvent += joinEv;

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

        static void joinEv(object sender, EventArgs data) {
            if(data is joinArg)
            {
                joinArg j = data as joinArg;
                Console.WriteLine("From:{0} Class:{1}{2} note{3}",j.Author,j.mainClass,j.subClass,j.content);
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
