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
                if(next.eventTime > d.eventTime)
                {
                    next = d;
                }
            }

            return next;

        }
    }
}
