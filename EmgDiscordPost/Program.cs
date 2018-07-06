using System;
using System.Collections.Generic;

namespace EmgDiscordPost
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                ConsoleMain con = new ConsoleMain(args[0]);
            }
            else
            {
                ConsoleMain con = new ConsoleMain();
            }
        }
    }
}
