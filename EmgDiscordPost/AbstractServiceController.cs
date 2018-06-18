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

    }
}
