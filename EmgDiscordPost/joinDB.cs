using System;
using System.Collections.Generic;
using System.Text;

namespace EmgDiscordPost
{
    class joinDB : AbstractDBController
    {
        IJoinMemberDB joindatabase;
        IEmgDBRead emg;

        public joinDB(IJoinMemberDB db,IEmgDBRead emg) : base(db, "JoinMember")
        {
            joindatabase = db;
            this.emg = emg;
        }

        override protected void setDBtable()
        {
            tablename = "JoinMember";
        }

        //0:メンバー追加
        //1:追加時間外
        public int addMember(IjoinArg member)
        {
            List<EventData> ev = emg.getEmgList(DateTime.Now, DateTime.Now + new TimeSpan(0, 30, 0));
            bool isEmgEnable = false;

            foreach (EventData e in ev)
            {
                if (e is emgQuest)
                {
                    isEmgEnable = true;
                }
            }

            if (isEmgEnable)
            {
                joindatabase.addMember(member);
                return 0;
            }
            else
            {
                return 1;
            }
        }

        public void cancelMember(string name)
        {
            joindatabase.deleteMember(name);
        }

        public List<IjoinArg> getMember()
        {
            return joindatabase.getMemberList();
        }

        //テーブルの初期化
        public void initDB()
        {
            joindatabase.droptable();
            joindatabase.createtable();
        }

        public void clearTB()
        {
            joindatabase.cleartable();
        }
    }
}
