using BurnSystems.FlexBG.Modules.BackgroundWorkerM.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.BackgroundWorkerM.Logic
{
    /// <summary>
    /// Returns a predicate, which executes an event by real time
    /// </summary>
    public class ExecuteByRealTime : ITimePredicate
    {
        /// <summary>
        /// Stores the date of the last execution
        /// </summary>
        private DateTime lastExecution = DateTime.MinValue;

        /// <summary>
        /// Stores the intervaltimes
        /// </summary>
        private TimeSpan intervalTime;

        public ExecuteByRealTime(TimeSpan intervalTime)
        {
            this.intervalTime = intervalTime;
        }

        /// <summary>
        /// Gets the next execution time
        /// </summary>
        /// <param name="container">Container to be used</param>
        /// <returns>Datetime of next</returns>
        public DateTime GetNextExecutionTime(ObjectActivation.IActivates container)
        {
            return this.lastExecution + this.intervalTime;
        }

        /// <summary>
        /// Refreshes the time
        /// </summary>
        /// <param name="container"></param>
        public void RefreshTime(ObjectActivation.IActivates container)
        {
            this.lastExecution = DateTime.Now;
        }
    }
}
