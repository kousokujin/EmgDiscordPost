using System;
using System.Collections.Generic;
using System.Text;

namespace EmgDiscordPost
{
    class chpDB : AbstractDBController
    {
        IChpDataRead DB;
        IChpTimeDB chpConf;

        public chpDB(IChpDataRead DB,IChpTimeDB chpConf):base(DB, "PSO2ChpTable")
        {
            this.DB = DB;
            this.chpConf = chpConf;
        }

        protected override void setDBtable()
        {
            this.DB.setTable("PSO2ChpTable");
        }

        public void addTime(int week,int hour,int min,int sec)
        {
            chpConf.addChpTable(week, hour, min,sec);
        }

        public DateTime nextNotifyTime()
        {
            DateTime notify = new DateTime();
            TimeSpan ts = new TimeSpan(365, 0, 0, 0);//とりあえず1年

            List<DateTime> lstTime = chpConf.getNotifyTime();

            foreach(DateTime d in lstTime)
            {
                if((d - DateTime.Now) < ts)
                {
                    notify = d;
                }
            }

            return notify;
        }
    }
}
