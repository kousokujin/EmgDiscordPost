using System;
using System.Collections.Generic;
using System.Text;

namespace EmgDiscordPost
{
    abstract class EventData
    {
        public DateTime eventTime;
        public string eventName;

        public EventData(DateTime time, string evnt)
        {
            this.eventTime = time;
            this.eventName = evnt;
        }
    }

    class emgQuest : EventData  //緊急クエスト
    {
        public string live;
        public bool liveEnable;

        public emgQuest(DateTime time, string Event) : base(time, Event)
        {
            liveEnable = false;
            live = "";
        }

        public emgQuest(DateTime time, string Event, string liveName) : base(time, Event)
        {
            live = liveName;
            liveEnable = true;
        }

        public emgQuest(DateTime time, string Event, string livenName, bool liveEnable) : base(time, Event)
        {
            live = livenName;
            this.liveEnable = liveEnable;

        }
    }

    class casino : EventData    //カジノイベント
    {
        public casino(DateTime time) : base(time, "カジノイベント")
        {

        }
    }

    class noEvent : EventData //イベントなし
    {
        public noEvent() : base(new DateTime(), "NONE")
        {

        }
    }
}
