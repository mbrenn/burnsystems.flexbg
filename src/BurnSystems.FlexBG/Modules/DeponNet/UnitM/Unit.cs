using BurnSystems.FlexBG.Modules.DeponNet.UnitM.Strategies;
using BurnSystems.FlexBG.Modules.DeponNet.UnitM.UnitJobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace BurnSystems.FlexBG.Modules.DeponNet.UnitM
{
    [Serializable]
    public class Unit
    {
        /// <summary>
        /// Stores the list of jobs
        /// </summary>
        private List<IJob> jobs = new List<IJob>();

        /// <summary>
        /// Gets or sets the unit id
        /// </summary>
        public long Id
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the owning player
        /// </summary>
        public long PlayerId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the position of the unit
        /// </summary>
        public Vector3D Position
        {
            get;
            set;
        }

        /// <summary>
        /// Stores the id of the unittype
        /// </summary>
        public int UnitTypeId
        {
            get;
            set;
        }

        /// <summary>
        /// Defines the strategy of the unit to opponents
        /// </summary>
        public UnitStrategy Strategy
        {
            get;
            set;
        }

        public List<IJob> Jobs
        {
            get
            {
                return this.jobs;
            }
        }

        /// <summary>
        /// Gets or sets the index of the current job in the list Jobs
        /// </summary>
        public int IndexCurrentJob
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes a new instance of the Unit class
        /// </summary>
        public Unit()
        {
            this.IndexCurrentJob = -1;

        }
    }
}
