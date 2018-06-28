using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EmgDiscordPost
{
    class EmgDBGet : AbstractDBController
    {
        IEmgDBRead Emgdb;

        public EmgDBGet(IEmgDBRead db) : base(db, "PSO2EventTable")
        {
            Emgdb = db;
        }

        protected override void setDBtable()
        {
            Emgdb.setTable(tablename);
        }

        //timeで指定した時刻から一番近い緊急クエストを返す
        //現状は緊急クエストのみ
        public EventData getNextEvent(DateTime time)
        {
            DateTime end = time + new TimeSpan(2, 0, 0, 0); //timeから2日後くらいまで
            List<EventData> lst = Emgdb.getEmgList(time, end);

            if(lst.Count == 0)
            {
                return new noEvent();
            }

            EventData next = lst[0];

            foreach(EventData d in lst)
            {
                if((next.eventTime > d.eventTime) && d is emgQuest)
                {
                    next = d;
                }
            }

            return next;
        }

        //timeで指定した日の緊急クエスト一覧を返す
        public List<EventData> getListEvent(DateTime time)
        {
            DateTime fixTime = new DateTime(time.Year, time.Month, time.Day, 0, 0, 0);
            return Emgdb.getEmgList(fixTime, fixTime + new TimeSpan(1, 0, 0, 0));
        }
    }
}
