using System;
using System.Collections.Generic;
using System.Text;

namespace EmgDiscordPost
{
    class ConsoleMain
    {
        MainCON con;
        bool end;

        public ConsoleMain(string file = "")
        {
            outputTitle();
            end = false;
            if (file == "")
            {
                con = new MainCON();
            }
            else
            {
                con = new MainCON(file, "TEXT");
            }
            ProcessPronpt();
        }

        public void ProcessPronpt()
        {
            do
            {
                outputPronpt();
                string command = Console.ReadLine();
                CommandProcess(command);
            } while (end == false);
        }

        private void outputPronpt()
        {
            logOutput.Console("EmgDiscordPost > ");
        }

        private string[] sepalate (string command)
        {
            string reCommand = command.Replace("　", " ");
            reCommand = reCommand.Replace("\t", "");

            string[] output = reCommand.Split(' ');
            return output;
        }

        private void CommandProcess(string command)
        {
            string[] sepCommadn = sepalate(command);

            switch (sepCommadn[0]) {
                case "":
                    //outputPronpt();
                    break;
                case "init":
                    con.FirstMigration();
                    break;
                case "clear":
                    con.cleartables();
                    break;
                case "help":
                    outputhelp();
                    break;
                case "lodos":
                    if (sepCommadn.Length >= 2)
                    {
                        switch (sepCommadn[1])
                        {
                            case "true":
                                con.setLodos(true);
                                logOutput.ConsoleLine("バル・ロドス通知を有効にしました。");
                                break;
                            case "false":
                                con.setLodos(false);
                                logOutput.ConsoleLine("バル・ロドス通知を無効にしました。");
                                break;
                            default:
                                logOutput.ConsoleLine("引数がちがいます。");
                                break;
                        }
                    }
                    else
                    {
                        logOutput.ConsoleLine("1つ以上の引数を指定してください。");
                    }

                    break;

                case "post":
                    if (sepCommadn.Length >= 2) {
                        con.sendPost(sepCommadn[1]);
                    }
                    else
                    {
                        logOutput.ConsoleLine("投稿する内容を入力してください。");
                    }
                    break;

                case "version":
                    outputVersion();
                    break;
                case "stop":
                    end = true;
                    break;
                case "quit":
                    end = true;
                    break;
                case "exit":
                    end = true;
                    break;
                default:
                    logOutput.ConsoleLine("コマンドが見つかりません");
                    break;
            }
        }

        private void outputhelp()
        {
            string output = "使い方\n";
            output += "lodos [true|false]:バル・ロドス通知の設定\n";
            output += "init:データベースのマイグレーション\n";
            output += "clear:データベースのクリア\n";
            output += "post [word]:Dicordに投稿";

            logOutput.ConsoleLine(output);
        }

        private void outputTitle()
        {
            Console.WriteLine("-----------------------------");
            outputVersion();
            Console.WriteLine("-----------------------------");

        }

        private void outputVersion()
        {
            Console.WriteLine("EmgDiscordPost");
            Console.WriteLine("Version {0}", myFunction.getAssemblyVersion());
            Console.WriteLine("Copyright (c) 2018 Kousokujin.");
            Console.WriteLine("Released under the MIT license.");
        }
    }
}
