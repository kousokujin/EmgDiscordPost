using System;
using System.Collections.Generic;
using System.Text;
using Discord.WebSocket;
using Discord.Commands;
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

        public DiscordService(string token, ulong channelID, string botname)
        {
            this.token = token;
            this.channelID = channelID;
            this.bot_name = botname;

            client = new DiscordSocketClient();
            //chn = client.GetChannel(channelID) as SocketTextChannel;

            connect();
        }

        public DiscordService(DiscordSocketClient client)
        {
            this.client = client;
            connect();
        }

        public override void connect()
        {
            Task conection = StartClient();
            try
            {
                conection.Wait();
            }catch(System.AggregateException e)
            {
                logOutput.writeLog(e.Message);
            }
            logOutput.writeLog("Discordに接続しました");
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
                if (!arg.Author.Username.Equals(bot_name) && arg.Channel.Id == channelID)
                {
                    (bool isRep, string mes) = isReplay(arg.Content);

                    if (isRep == true)
                    {
                        //RunReplayEvent(this, new ReceiveData(arg.Author.ToString(), mes));
                        RunReplayEvent(this, new DiscordReceive(arg,mes));
                    }
                    else
                    {
                        //RunReceiveEvent(this, new ReceiveData(arg.Author.ToString(), arg.Content));
                        RunReceiveEvent(this, new DiscordReceive(arg));
                    }
                }
            });

            return t;
        }

        public override void postStr(string content)
        {
            if (content == "")
            {
                return;
            }
            SocketTextChannel st = client.GetChannel(channelID) as SocketTextChannel;
            Task t = st.SendMessageAsync(content);
            logOutput.writeLog("Discordへ投稿「{0}」", content);
            t.Wait();
        }

        //なんかうまくいかないので使用禁止（postStr(string)を使う）
        public override void sendReplay(string content, User username)
        {
            if (content == "")
            {
                return; //なにもしない
            }

            if (username is User)
            {
                string UserStr = string.Format("@{0}", username.username);
                postStr(string.Format("{0} {1}", UserStr, content));

                return;
            }

            DiscordUser disUser = username as DiscordUser;

            if(!(disUser.user is SocketGuildUser))
            {
                return;
            }

            SocketGuildUser guilduser = disUser.user as SocketGuildUser;
            string userStr = string.Format("@<{0}>",guilduser.Id);
            postStr(string.Format("{0} {1}", userStr, content));
        }

        protected override (bool, string) isReplay(string mes)
        {
            ulong myID = client.CurrentUser.Id;
            string myIDstr = string.Format("<@{0}> ", myID);

            bool isRep = mes.Contains(myIDstr);

            if (!isRep)
            {
                return (isRep, mes);
            }

            string replace = mes.Replace(myIDstr, "");

            return (isRep, replace);
        }
    }

    class DiscordReceive : ReceiveData
    {
        public SocketMessage message;

        public DiscordReceive(SocketMessage mes) : base(mes.Author.Username, mes.Content)
        {
            this.message = mes;
        }

        public DiscordReceive(SocketMessage mes, string content) : base(mes.Author.Username, content)
        {
            this.message = mes;
        }
    }

    class DiscordUser : User
    {
        public SocketUser user;

        public DiscordUser(SocketUser user) : base(user.ToString())
        {
            this.user = user;    
        }
    }
}
