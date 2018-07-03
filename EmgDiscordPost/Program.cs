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
            ulong id = 4;

            MainCON con = new MainCON(address, database, user, pass, botname, id, token);
            con.FirstMigration();

            Console.ReadLine();
        }
    }
}
