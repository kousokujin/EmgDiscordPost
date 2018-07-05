using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EmgDiscordPost
{
    class joinDB : AbstractDBController
    {
        IJoinMemberDB joindatabase;
        IEmgDBRead emg;
        bool EventLoop;
        Task LoopTask;

        DateTime nextJoinDelete;    //次の参加者削除チェック時間

        public joinDB(IJoinMemberDB db, IEmgDBRead emg) : base(db, "JoinMember")
        {
            joindatabase = db;
            this.emg = emg;
            setNextJoinDelTime();
            EventLoop = false;
            LoopTask = startLoop();
        }

        override protected void setDBtable()
        {
            tablename = "JoinMember";
        }

        //0:メンバー追加
        //1:メンバー更新
        //2:追加時間外
        public int addMember(IjoinArg member)
        {
            bool isEmgEnable = isJoinable();

            if (isEmgEnable)
            {
                bool isMember = joindatabase.addMember(member);
                if (isMember) return 1;
                return 0;
            }
            else
            {
                return 2;
            }
        }

        public bool isJoinable()
        {
            bool isEmgEnable = isExistEmg(DateTime.Now, DateTime.Now + new TimeSpan(0, 30, 0));
            return isEmgEnable;
        }

        private bool isExistEmg(DateTime start, DateTime end)
        {
            List<EventData> ev = emg.getEmgList(start, end);
            bool isEmgEnable = false;

            foreach (EventData e in ev)
            {
                if (e is emgQuest)
                {
                    isEmgEnable = true;
                }
            }

            return isEmgEnable;
        }

        public void cancelMember(string name)
        {
            joindatabase.deleteMember(name);
        }

        //参加者一覧
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

        public void setNextJoinDelTime()
        {
            DateTime now30 = DateTime.Now + new TimeSpan(0, 30, 0);
            int min = 0;
            if (now30.Minute >= 30)
            {
                min = 30;
            }

            nextJoinDelete = new DateTime(now30.Year, now30.Month, now30.Day, now30.Hour, min, 0);
            //Console.WriteLine(nextJoinDelete.ToString("HH:mm:ss"));
        }

        //参加者のテーブルを削除するイベントループ
        public void Loop()
        {
            while (EventLoop)
            {
                if (DateTime.Now > nextJoinDelete)
                {
                    //Console.WriteLine("delete");
                    DateTime temp = nextJoinDelete;
                    setNextJoinDelTime();

                    DateTime start = temp - new TimeSpan(1, 0, 0);
                    DateTime end = temp - new TimeSpan(0, 30, 0);
                    bool isExist = isExistEmg(start, end);

                    if (isExist)
                    {
                        joindatabase.cleartable();  //参加者クリア
                    }

                    //setNextJoinDelTime();
                }

                System.Threading.Thread.Sleep(1000);
            }
        }

        public async Task startLoop()
        {
            EventLoop = true;
            await Task.Run(() => { Loop(); });
        }
    }
}
