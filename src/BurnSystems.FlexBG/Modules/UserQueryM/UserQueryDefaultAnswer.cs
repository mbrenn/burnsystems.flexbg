﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.UserQueryM
{
    /// <summary>
    /// Gives always the default answer
    /// </summary>
    public class UserQueryDefaultAnswer : IUserQuery
    {
        /// <summary>
        /// Asks the user a certain question and gives a set of choices
        /// </summary>
        /// <param name="question">Question to be asked</param>
        /// <param name="choices">Choices to be possible or null if every answer is acceptable</param>
        /// <param name="defaultChoice">Default choice of the user</param>
        /// <returns>Resulting choice</returns>
        public string Ask(string question, IEnumerable<string> choices, string defaultChoice)
        {
            return defaultChoice;
        }
    }
}
