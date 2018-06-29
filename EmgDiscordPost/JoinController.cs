using System;
using System.Collections.Generic;
using System.Text;

namespace EmgDiscordPost
{
    class JoinController
    {
        joinDB joinDatabase;
        joinServiceController postService;
        IDBConfig configDB;

        public JoinController(joinDB DBController,joinServiceController Service,IDBConfig config)
        {
            this.joinDatabase = DBController;
            this.postService = Service;
            this.configDB = config;

            postService.joinEvent += joinEventFromService;
        }

        //参加の申請が来た時
        private async void joinEventFromService(object sender,EventArgs e)
        {
            if(e is IjoinArg)
            {
                IjoinArg join = e as IjoinArg;
                int result = joinDatabase.addMember(join);
                string postStr = "";

                if(result == 0)
                {
                    string jobStr = myFunction.ConvertJob(join.getMainclass(), join.getSubclass());
                    if(jobStr == "")
                    {
                        jobStr += "クラス未定";
                    }

                    postStr = string.Format("{0}が{1}でエントリーしました。",join.getName(),jobStr);
                }
                else
                {
                    postStr = "現在エントリー期間外です。";
                }

                await postService.AsyncPostService(postStr);
            }
        }

        //キャンセルが出た時
        private async void cancelEventFromService(object sender,EventArgs e)
        {
            if(e is ReceiveData)
            {
                ReceiveData data = e as ReceiveData;
                if (joinDatabase.isJoinable())
                {
                    joinDatabase.cancelMember(data.Author);
                    await postService.AsyncPostService(string.Format("{0}のエントリーをキャンセルしました。", data.Author));
                }
                else
                {
                    await postService.AsyncPostService("現在エントリーの期間外です。");
                }
            }
        }

        //参加者の一覧
        private async void postMemberList(object sender, EventArgs e)
        {
            List<IjoinArg> member = joinDatabase.getMember();
            if (member.Count != 0)
            {
                string postStr = "現在エントリーしている人は以下の通りです。\n";
                int count = 1;

                foreach (IjoinArg m in member)
                {
                    string name = m.getName();
                    string jobclass = myFunction.ConvertJob(m.getMainclass(), m.getSubclass());

                    if (jobclass != "")
                    {
                        jobclass = string.Format("({0})", jobclass);
                    }

                    string addtext = string.Format("{0}{1} {2}", name, jobclass, m.getNote());

                    if (count != member.Count)
                    {
                        addtext += '\n';
                    }

                    postStr += addtext;
                    count++;
                }

                await postService.AsyncPostService(postStr);
            }
            else
            {
                await postService.AsyncPostService("現在誰もエントリーしていません。");
            }
        }
    }
}
