using BurnSystems.FlexBG.Modules.DeponNet.GameM;
using BurnSystems.Logging;
using BurnSystems.ObjectActivation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.DeponNet.Rules.MilitaryTick
{
    public class MilitaryTickExecution
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
        private static ILog logger = new ClassLogger(typeof(MilitaryTickExecution));

        public static void Execute(IActivates container)
        {
            var tick = container.Create<MilitaryTickExecution>();
            tick.PerformTick();
        }

        /// <summary>
        /// Performs the tick
        /// </summary>
        private void PerformTick()
        {
            logger.LogEntry(LogLevel.Message, "Military Tick: " + this.CurrentGame.Id);
        }
    }
}
