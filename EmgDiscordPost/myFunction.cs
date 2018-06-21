﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace EmgDiscordPost
{
    static class myFunction //クラスにするほどでもない関数群
    {
        static public long convertToUnixTime(DateTime time)    //UNIX時間に変換
        {
            DateTime UNIX_EPOCH = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            DateTime target = time.ToUniversalTime();
            TimeSpan ts = target - UNIX_EPOCH;

            return (long)ts.Seconds;
        }

        static public string getAssemblyVersion()
        {
            System.Diagnostics.FileVersionInfo ver = System.Diagnostics.FileVersionInfo.GetVersionInfo(
                System.Reflection.Assembly.GetExecutingAssembly().Location);

            string version = ver.ProductVersion;

            return version;
        }

        static public string getLiveEmgStr(emgQuest e, string section = "->")   //クーナライブがある時に使う
        {
            if (e.liveEnable == true)
            {
                if (Regex.IsMatch(e.live, "^クーナスペシャルライブ「.*」") == true) //他のライブの時は無理
                {
                    //もっといい方法がありそう
                    string str = Regex.Replace(e.live, "^クーナスペシャルライブ「", "");
                    str = Regex.Replace(str, "」$", "");
                    return string.Format("{0}{1}{2}", str, section, e.eventName);
                }
                else
                {
                    return string.Format("{0}{1}{2}", e.live, section, e.eventName);
                }
            }

            return e.eventName;
        }

        static public (JobClass mainclass,JobClass subclass) convertJobClass(string str)
        {
            if (str.Length >= 2)
            {
                string classStr = str.ToLower();
                char[] mainClassChr = { classStr[0], classStr[1] }; //メインクラス文字列
                string mainClassStr = new string(mainClassChr);

                JobClass mainclass = convertJobclass(mainClassStr);

                if (mainclass == JobClass.Hr)    //ヒーローの場合
                {
                    return (mainclass, JobClass.None);
                }

                if (mainclass == JobClass.None)  //何も割り当てられなかった場合
                {
                    return (JobClass.None, JobClass.None);
                }

                if (str.Length == 4) //サブクラス割当
                {
                    char[] subClassChr = { classStr[2], classStr[3] }; //サブクラス文字列
                    string subClassStr = new string(subClassChr);

                    JobClass subclass = convertJobclass(subClassStr);

                    if (subclass == JobClass.Hr || subclass == JobClass.None)    //サブクラスがヒーロまたは割当なしの場合は割当なしでエントリー
                    {
                        return (JobClass.None, JobClass.None);
                    }

                    return (mainclass,subclass);
                }


            }

            return (JobClass.None, JobClass.None);
        }

        static public JobClass convertJobclass(string classStr)   //文字列から列挙型へ変換
        {
            JobClass returnClass = JobClass.None;

            switch (classStr)
            {
                case "hu":
                    returnClass = JobClass.Hu;
                    break;
                case "fi":
                    returnClass = JobClass.Fi;
                    break;
                case "ra":
                    returnClass = JobClass.Ra;
                    break;
                case "gu":
                    returnClass = JobClass.Gu;
                    break;
                case "fo":
                    returnClass = JobClass.Fo;
                    break;
                case "te":
                    returnClass = JobClass.Te;
                    break;
                case "br":
                    returnClass = JobClass.Br;
                    break;
                case "bo":
                    returnClass = JobClass.Bo;
                    break;
                case "su":
                    returnClass = JobClass.Su;
                    break;
                case "hr":
                    returnClass = JobClass.Hr;
                    break;
                default:
                    returnClass = JobClass.None;
                    break;
            }

            return returnClass;

        }

        /*
        static public string generateEmgArrStr(List<EventData> evn)
        {
            string output = "";

            foreach (EventData e in evn)
            {
                if (e is emgQuest)
                {
                    string eventStr = getLiveEmgStr((emgQuest)e);
                    output += string.Format("{0} {1}\n", e.eventTime.ToString("HH:mm"), eventStr);
                }
            }

            return output;
        }

        static public string EmgArrStrNumbered(List<EventData> evn)
        {
            string output = "";
            int number = 1;

            foreach (EventData e in evn)
            {
                string eventStr = "";
                if (e is emgQuest)
                {
                    eventStr = getLiveEmgStr((emgQuest)e);
                }
                else
                {
                    eventStr = e.eventName;
                }

                output += string.Format("[{0}] {1} {2}\n", number,e.eventTime.ToString("MM/dd HH:mm"), eventStr);
                number++;
            }

            return output;
        }
        */
    }
}