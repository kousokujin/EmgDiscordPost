using System;
using System.Collections.Generic;
using System.Text;

namespace EmgDiscordPost
{
    abstract class AbstractServiceController
    {
        protected IPostService service;

        public AbstractServiceController(IPostService service)
        {
            this.service = service;
        }

        //Discordのリプライで反応する言葉
        public abstract void addWord(string word);

        //Discordのリプライで反応するかどうか
        public abstract bool isWord(string word);

    }
}
