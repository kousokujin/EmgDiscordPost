using System;
//using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmgDiscordPost
{
    class logOutput
    {
        //static StreamWriter writer;
        //static FileStream stream;
        private static string filename;
        private static DateTime dt;
        private static string date;
        private static string time;
        private static List<string> logQue;

        private static bool loop = false;

        public static void writeLog(string str)
        {
            if(logQue == null)
            {
                logQue = new List<string>();
            }
            if(loop == false)
            {
                Task t = startloop();
            }

            logQue.Add(str);
        }

        public static void writeLog(string log,params string[] args)
        {
            string str = string.Format(log, args);
            writeLog(str);
        }

        public static void init(string name)
        {
            filename = name;
        }

        public static async Task startloop()
        {
            loop = true;
            await Task.Run(() => {
                QueLoop();
            });
        }

        private static void QueLoop()
        {
            while (true)
            {
                /*
                foreach (string s in logQue)
                {
                    outputlogfile(s);
                    //logQue.Remove(s);
                }
                logQue.Clear();
                */
                
                if (logQue.Count != 0)
                {
                    outputlogfile(logQue[0]);
                    logQue.RemoveAt(0);
                }
                System.Threading.Thread.Sleep(50);
            }
        }

        private static void outputlogfile(string str)
        {
            if (filename == null)
            {
                filename = @"config/log.txt";
            }

            if (File.Exists(filename) == false)
            {
                string directory = Regex.Replace(filename, "/.+$", string.Empty);
                Directory.CreateDirectory(directory);
            }

            dt = DateTime.Now;
            date = dt.ToString("yyyy/MM/dd");
            time = dt.ToString("HH:mm:ss");
            string text = string.Format("[{0} {1}]{2}", date, time, str);

            try
            {
                using (FileStream file = new FileStream(filename, FileMode.Append))
                {
                    using (StreamWriter writer = new StreamWriter(file, Encoding.UTF8))
                    {
                        writer.WriteLine(text);
                        //System.Console.WriteLine(text);
                    }
                }
            }
            catch (FieldAccessException)
            {
                //System.Console.WriteLine(text);
                System.Console.WriteLine("ログファイルへの書き込みに失敗しました。");
            }
            catch (System.Security.SecurityException)
            {
                //System.Console.WriteLine(text);
                System.Console.WriteLine("ログファイルへのアクセス権がありません。");
            }
            catch (System.IO.IOException)
            {
                System.Console.WriteLine("ログファイルが使用中です。");
            }

            System.Console.WriteLine(text);
        }
    }
}
