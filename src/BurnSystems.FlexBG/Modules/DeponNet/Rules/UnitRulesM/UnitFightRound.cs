using BurnSystems.FlexBG.Modules.DeponNet.PlayerM.Interface;
using BurnSystems.FlexBG.Modules.DeponNet.UnitM;
using BurnSystems.FlexBG.Modules.DeponNet.UnitM.Data;
using BurnSystems.FlexBG.Modules.DeponNet.UnitM.Interfaces;
using BurnSystems.Logging;
using BurnSystems.ObjectActivation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.DeponNet.Rules.UnitRulesM
{
    public class UnitFightRound
    {
        /// <summary>
        /// Stores the logger
        /// </summary>
        private ClassLogger logger = new ClassLogger(typeof(UnitFightRound));

        [Inject]
        public IUnitManagement UnitManagement
        {
            get;
            set;
        }

        [Inject]
        public IPlayerManagement PlayerManagement
        {
            get;
            set;
        }

        [Inject(ByName = "CurrentGame")]
        public long CurrentGameId
        {
            get;
            set;
        }

        /// <summary>
        /// Executes the fight round
        /// </summary>
        public void ExecuteFightRound()
        {
            logger.LogEntry(LogLevel.Notify, "Starting FightRound");
            var units = this.UnitManagement.GetAllUnits().ToList();
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            foreach (var unit in units)
            {
                // Get all units in scope
                var fightingUnits = units.Where(
                    x => this.MightFight(unit, x) && x != unit);

                foreach (var fightUnit in fightingUnits)
                {
                    //logger.LogEntry(LogLevel.Verbose, string.Format("{0} attacks {1}", unit.Id, fightUnit.Id));
                }
            }

            stopWatch.Stop();
            logger.LogEntry(LogLevel.Notify, string.Format("Ending FightRound: {0} ms", stopWatch.ElapsedMilliseconds));
        }

        /// <summary>
        /// Checks, if both units might fight
        /// </summary>
        /// <param name="attacker">Attacker unit</param>
        /// <param name="defender">Defender unit</param>
        /// <returns>true, if both units fight</returns>
        public bool MightFight(Unit attacker, Unit defender)
        {
            if (attacker.Strategy == null)
            {
                return false;
            }

            var distance = (attacker.Position - defender.Position).Length;
            if (distance < attacker.Strategy.AttackRadius)
            {
                return true;
            }

            return false;
        }
    }
}
