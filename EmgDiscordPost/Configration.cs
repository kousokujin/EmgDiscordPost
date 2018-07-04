using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace EmgDiscordPost
{
    class Configration
    {
        //データベース関連
        public string DBaddress;
        public string database;
        public string user;
        public string password;

        //Discord情報
        public string botname;
        public ulong channelID;
        public string token;

        //ファイル名
        string filename;

        //マイグレーション
        public bool isMigration = false;

        public Configration(string filename = "config/config.xml", string filetype = "XML")
        {
            this.filename = filename;
            var cof = new configClass();
            object cofObj;

            if (XmlFileIO.isExist(filename) == false)
            {
                logOutput.writeLog("設定ファイルが見つかりません。");
                init();
                return;
            }

            if (filetype == "XML")
            {
                bool loaded = XmlFileIO.xmlLoad(cof.GetType(), this.filename, out cofObj);
                if (loaded == false && cofObj is configClass)
                {
                    logOutput.writeLog("設定ファイルの読み込みに失敗しました。");
                    init();
                    return;
                }

                cof = cofObj as configClass;
                setData(cof);
                return;
            }

            if(filetype == "FILE")
            {
                configClass cofTmp;
                (cofTmp,this.isMigration) = setConfig(this.filename);

                if(cofTmp == null)  //読み込み失敗
                {
                    logOutput.writeLog("設定ファイルの読み込みに失敗しました。");
                    init();
                    return;
                }

                setData(cofTmp);
                savefile();
            }

            //default
            logOutput.writeLog("設定ファイルが見つかりません。");
            init();

        }

        private void setData(configClass cof)
        {
            this.database = cof.database;
            this.DBaddress = cof.DBaddress;
            this.user = cof.user;
            this.password = cof.password;

            this.botname = cof.botname;
            this.channelID = ulong.Parse(cof.channelID);
            this.token = cof.token;

            bool enable = ulong.TryParse(cof.channelID, out this.channelID);
            while (!enable)
            {
                Console.Write("ChannelID:");
                string chStr = Console.ReadLine();
                enable = ulong.TryParse(chStr, out this.channelID);
            }
        }

        private void generateSetting()
        {
            System.Threading.Thread.Sleep(1000);
            Console.WriteLine("Database Setting");
            Console.Write("Database Address:");
            DBaddress = Console.ReadLine();
            Console.Write("Database Name:");
            database = Console.ReadLine();
            Console.Write("UserName:");
            user = Console.ReadLine();
            Console.Write("Password:");
            password = Console.ReadLine();

            Console.Write("Discord Setting");
            Console.Write("Bot Name:");
            botname = Console.ReadLine();
            Console.Write("Bot Token:");
            token = Console.ReadLine();

            var isConvert = false;
            do
            {
                Console.Write("ChannelID:");
                string tmpID = Console.ReadLine();

                isConvert = ulong.TryParse(tmpID, out channelID);

                if(isConvert == true)
                {
                    channelID = ulong.Parse(tmpID);
                }

            } while (!isConvert);
        }

        private void init()
        {
            generateSetting();
            savefile();
        }

        public bool savefile()
        {
            var cof = new configClass();
            cof.DBaddress = this.DBaddress;
            cof.database = this.database;
            cof.user = this.user;
            cof.password = this.password;

            cof.botname = this.botname;
            cof.channelID = this.channelID.ToString();
            cof.token = this.token;

            bool res = XmlFileIO.xmlSave(cof.GetType(), this.filename, cof);

            return res;
        }

        //ファイルから
        private (configClass cc, bool migration) setConfig(string filename)
        {
            List<string> textfile = openTextfile(filename);

            string server = "";
            string database = "";
            string user = "";
            string password = "";
            bool mig = false;

            string token = "";
            string channelIDStr = "";
            string botname = "";
            ulong chID = 0;


            foreach (string s in textfile)
            {
                string[] sepalate = s.Split('=');

                switch (sepalate[0])
                {
                    case "server":
                        server = sepalate[1];
                        break;
                    case "database":
                        database = sepalate[1];
                        break;
                    case "user":
                        user = sepalate[1];
                        break;
                    case "password":
                        password = sepalate[1];
                        break;
                    case "init":
                        if (sepalate[1] == "true" || sepalate[1] == "1" || sepalate[1] == "yes")
                        {
                            mig = true;
                        }
                        break;
                    case "token":
                        token = sepalate[1];
                        break;
                    case "chanel":
                        channelIDStr = sepalate[1];
                        break;
                    case "botname":
                        botname = sepalate[1];
                        break;
                }
            }

            if(channelIDStr == "" && ulong.TryParse(channelIDStr,out chID) == false)
            {
                chID = ulong.Parse(channelIDStr);
                return (null, false);
            }

            if (server != "" && database != "" && user != "" && password != "" && token != "" && botname != "")
            {
                configClass output = new configClass();
                output.DBaddress = server;
                output.database = database;
                output.user = user;
                output.password = password;

                output.token = token;
                output.botname = botname;
                output.channelID = chID.ToString();

                return (output, mig);
            }
            else
            {
                return (null, false);
            }
        }

        private List<string> openTextfile(string filename)  //ファイルを開いて1行ごとに配列にする
        {
            List<string> output = new List<string>();

            using (StreamReader read = new StreamReader(filename))
            {
                string line;
                while ((line = read.ReadLine()) != null)
                {
                    string addline = line.Replace(" ", "");
                    addline = addline.Replace("\t", "");
                    addline = addline.Replace("\n", "");
                    if (addline[0] != '#')  //コメントアウト
                    {
                        output.Add(addline);
                    }
                }
            }

            return output;
        }
    }

    //ファイルに保存するクラス
    [Serializable]
    public class configClass
    {
        //データベース関連
        public string DBaddress;
        public string database;
        public string user;
        public string password;

        //Discord情報
        public string botname;
        public string channelID;
        public string token;
    }
}
