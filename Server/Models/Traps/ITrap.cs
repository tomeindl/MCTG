﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Models
{
    interface ITrap
    {
        List<AbstractCard> ManipulateDeck(List<AbstractCard> deck);
    }
}
