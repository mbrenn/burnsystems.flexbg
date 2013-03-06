using BurnSystems.FlexBG.Modules.DeponNet.GameM;
using BurnSystems.Logging;
using BurnSystems.ObjectActivation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.DeponNet.Rules.EconomyTick
{
    public class EconomyTickExecution
    {
        [Inject(ByName = "CurrentGame")]
        public Game CurrentGame
        {
            get;
            set;
        }

        /// <summary>
        /// Logger to be used
        /// </summary>
        private static ILog logger = new ClassLogger(typeof(EconomyTickExecution));

        public static void Execute(IActivates container)
        {
            var tick = container.Create<EconomyTickExecution>();
            tick.PerformTick();
        }

        /// <summary>
        /// Performs the tick
        /// </summary>
        private void PerformTick()
        {
            logger.LogEntry(LogLevel.Message, "Economy Tick: " + this.CurrentGame.Id);
        }
    }
}
