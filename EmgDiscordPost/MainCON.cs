using System;
using System.Collections.Generic;
using System.Text;

namespace EmgDiscordPost
{
    class MainCON
    {
        //いろいろな情報

        //データベース関連
        string DBaddress;
        string database;
        string user;
        string password;

        //Discord情報
        string botname;
        ulong channelID;
        string token;

        //覇者の紋章関係
        IChpDataRead chpData;
        IChpTimeDB chpTimeDB;
        chpDB chpDBCon;
        IemgPost chpPost;
        ChpPostController chpPostCon;
        ChpController chpCon;

        //緊急クエスト関連
        IEmgDBRead emgDBRead;
        IemgPost emgPost;
        EmgDBGet EmgDB;
        EmgController EmgCon;

        //緊急クエスト参加
        IJoinMemberDB joinMemDB;
        IjoinPost joinPost;
        EmgPostController emgPostCon;
        joinDB jDB;
        JoinController joinCon;

        //設定を記述するデータベース
        IDBConfig configDB;
        joinServiceController joinSerCon;

        //その他
        IPostService discord;
        DefaultDiscordController defCon;

        public MainCON(string DBaddress, string database, string user, string password, string botname, ulong channelID, string token)
        {
            this.DBaddress = DBaddress;
            this.database = database;
            this.user = user;
            this.password = password;
            this.botname = botname;
            this.channelID = channelID;
            this.token = token;

            //覇者の紋章コンストラクタ
            chpData = new PostgreSQL_ChpRead(DBaddress, database, user, password);
            chpTimeDB = new PostgreSQL_chp_confDB(DBaddress, database, user, password);
            chpDBCon = new chpDB(chpData, chpTimeDB);
            chpPost = new emgPostToDiscord(token, channelID, botname);
            chpPostCon = new ChpPostController(chpPost);
            chpCon = new ChpController(chpDBCon, chpPostCon);

            //緊急クエストコンストラクタ
            emgDBRead = new PostgreSQL_EmgRead(DBaddress, database, user, password);
            configDB = new PostgreSQL_configLoader(DBaddress, database, user, password);
            emgPost = new emgPostToDiscord(token, channelID, botname);
            emgPostCon = new EmgPostController(emgPost);
            EmgDB = new EmgDBGet(emgDBRead);
            EmgCon = new EmgController(emgPostCon, EmgDB, configDB);

            //緊急クエスト参加コンストラクタ
            joinMemDB = new PostgreSQL_joinDB(DBaddress, database, user, password);
            joinPost = new joinPostDiscord(token, channelID, botname);
            jDB = new joinDB(joinMemDB, emgDBRead);
            joinSerCon = new joinServiceController(joinPost);
            joinCon = new JoinController(jDB, joinSerCon, configDB);

            //その他コンストラクタ
            List<AbstractServiceController> lstService = new List<AbstractServiceController>();
            lstService.Add(chpPostCon);
            lstService.Add(emgPostCon);
            lstService.Add(joinSerCon);
            discord = new DiscordService(token, channelID, botname);
            defCon = new DefaultDiscordController(discord, lstService);

        }

        //一番最初のマイグレーション
        public void FirstMigration()
        {
            //設定DB
            configDB.droptable();
            configDB.createtable();

            //覇者の紋章の時間
            chpTimeDB.droptable();
            chpTimeDB.createtable();

            //参加メンバーテーブル
            joinMemDB.droptable();
            joinMemDB.createtable();
        }
    }
}
