﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BurnSystems.FlexBG.Modules.GameInfoM
{
    /// <summary>
    /// Queries the gameinfo
    /// </summary>
    public interface IGameInfoProvider
    {
        GameInfo GameInfo
        {
            get;
        }
    }
}