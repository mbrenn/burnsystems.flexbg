using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace BurnSystems.FlexBG.Modules.DeponNet.UnitM.UnitJobs
{
    [Serializable]
    public class MoveJob : BaseJob
    {
        /// <summary>
        /// Defines the target position of the unit
        /// </summary>
        public Vector3D TargetPosition
        {
            get;
            set;
        }

        /// <summary>
        /// Calculates the velocity of the unit which is defined as distance per tick
        /// </summary>
        public double Velocity
        {
            get;
            set;
        }

        public override string ToString()
        {
            return string.Format(
                "To ({0}) with velocity {1}",
                this.TargetPosition.ToString(),
                this.Velocity.ToString());
        }
    }
}
