using System;
using System.Collections.Generic;
using System.Text;
using Discord.WebSocket;
using Discord;
using System.Threading.Tasks;
//using System.Text.RegularExpressions;

namespace EmgDiscordPost
{
    class DiscordService : AbstractPostService
    {
        string bot_name;
        protected DiscordSocketClient client;
        //protected SocketTextChannel chn;

        string token;
        ulong channelID;

        //チャンネルに投稿された時
        //public event EventHandler ReceiveEvent;

        //リプライを受信した時
        //public event EventHandler ReceiveReplay;

        public DiscordService(string token,ulong channelID,string botname)
        {
            this.token = token;
            this.channelID = channelID;
            this.bot_name = botname;

            client = new DiscordSocketClient();
            //chn = client.GetChannel(channelID) as SocketTextChannel;

            connect();
        }

        public override void connect()
        {
            Task conection = StartClient();
            conection.Wait();
        }

        public override void disconnect()
        {
            client.Dispose();
        }

        private async Task StartClient()
        {
            await client.LoginAsync(TokenType.Bot, token);
            await client.StartAsync();

            client.MessageReceived += this.MessageReceived;
            //client.LatencyUpdated += LatencyEvent;
        }

        private Task MessageReceived(SocketMessage arg)
        {
            Task t = Task.Run(() =>
            {
                if (!arg.Author.Username.Equals(bot_name))
                {
                    (bool isRep, string mes) = isReplay(arg.Content);

                    if (isRep == true)
                    {
                        RunReplayEvent(this, new ReceiveData(arg.Author.ToString(), mes));
                    }
                    else
                    {
                        RunReceiveEvent(this, new ReceiveData(arg.Author.ToString(), arg.Content));
                    }
                }
            });

            return t;
        }

        public override void postStr(string content)
        {
            if(content == "")
            {
                return;
            }

            SocketTextChannel st = client.GetChannel(channelID) as SocketTextChannel;
            Task t = st.SendMessageAsync(content);
            logOutput.writeLog("Discordへ投稿「{0}」", content);
            t.Wait();
        }

        protected override (bool,string) isReplay(string mes)
        {
            ulong myID = client.CurrentUser.Id;
            string myIDstr = string.Format("<@{0}> ",myID);

            bool isRep = mes.Contains(myIDstr);

            if (!isRep)
            {
                return (isRep,mes);
            }

            string replace = mes.Replace(myIDstr,"");

            return (isRep, replace);
        }
    }

    /*
    class ReceiveData : EventArgs
    {
        public SocketMessage message;
        public string content;

        public ReceiveData(SocketMessage mes)
        {
            this.message = mes;
            this.content = mes.Content;
        }

        public ReceiveData(SocketMessage mes,string content)
        {
            this.message = mes;
            this.content = content;
        }
    }
    */
}
