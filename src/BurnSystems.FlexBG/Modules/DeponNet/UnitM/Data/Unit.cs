using BurnSystems.FlexBG.Modules.DeponNet.UnitM.Strategies;
using BurnSystems.FlexBG.Modules.DeponNet.UnitM.UnitJobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace BurnSystems.FlexBG.Modules.DeponNet.UnitM.Data
{
    [Serializable]
    public class Unit
    {
        private long id;

        private long playerId;

        public Vector3D Position;

        private long unitTypeId;

        private UnitStrategy strategy;

        private int currentJobIndex;

        /// <summary>
        /// Stores the list of jobs
        /// </summary>
        private List<IJob> jobs = new List<IJob>();

        /// <summary>
        /// Stores the instances
        /// </summary>
        private List<UnitInstance> instances = new List<UnitInstance>();

        /// <summary>
        /// Gets or sets the unit id
        /// </summary>
        public long Id
        {
            get { return this.id; }
            set { this.id = value; }
        }

        /// <summary>
        /// Gets or sets the owning player
        /// </summary>
        public long PlayerId
        {
            get { return this.playerId; }
            set { this.playerId = value; }
        }

        /// <summary>
        /// Stores the id of the unittype
        /// </summary>
        public long UnitTypeId
        {
            get { return this.unitTypeId; }
            set { this.unitTypeId = value; }
        }

        /// <summary>
        /// Defines the strategy of the unit to opponents
        /// </summary>
        public UnitStrategy Strategy
        {
            get { return this.strategy; }
            set { this.strategy = value; }
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
            get { return this.currentJobIndex; }
            set { this.currentJobIndex = value; }
        }

        public List<UnitInstance> Instances
        {
            get { return this.instances; }
        }

        /// <summary>
        /// Initializes a new instance of the Unit class
        /// </summary>
        public Unit()
        {
            this.IndexCurrentJob = -1;
        }

        public double Amount
        {
            get { return this.Instances.Count; }
        }
    }
}
