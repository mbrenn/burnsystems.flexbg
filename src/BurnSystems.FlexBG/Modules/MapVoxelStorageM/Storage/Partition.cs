using BurnSystems.Test;
using System;
using System.Collections.Generic;
using System.IO;

namespace BurnSystems.FlexBG.Modules.MapVoxelStorageM.Storage
{
    /// <summary>
    /// Defines one partition containing the information about field data
    /// </summary>
    public class Partition
    {
        /// <summary>
        /// Gets or sets the instance id
        /// </summary>
        public long InstanceId
        {
            get;
            private set;
        }

        /// <summary>
        /// Stores the size of partition. 
        /// It has PartitionSize * PartitionSize elements.
        /// </summary>
        public int PartitionSize
        {
            get;
            private set;
        }

        public int PartitionX
        {
            get;
            set;
        }

        public int PartitionY
        {
            get;
            set;
        }

        /// <summary>
        /// Stores the columns
        /// </summary>
        private List<List<FieldTypeChangeInfo>> columns;

        /// <summary>
        /// Initializes a new instance of the Partition class
        /// </summary>
        /// <param name="x">X-Coordinate of the Partition (number of partition, not real position)</param>
        /// <param name="y">Y-Coordinate of the Partition (number of partition, not real position)</param>
        /// <param name="partitionSize">Size of the partition</param>
        public Partition(long instanceId, int x, int y, int partitionSize)
        {
            this.InstanceId = instanceId;
            this.PartitionX = x;
            this.PartitionY = y;
            this.PartitionSize = partitionSize;

            this.columns = new List<List<FieldTypeChangeInfo>>(partitionSize * partitionSize);
            for (var n = 0; n < partitionSize * partitionSize; n++)
            {
                this.columns.Add(new List<FieldTypeChangeInfo>());
            }
        }

        /// <summary>
        /// Converts position to absolute position on map
        /// </summary>
        /// <param name="relX">Relative coordinate to partition</param>
        /// <param name="relY">Relative coordinate to partition</param>
        /// <param name="absX">Absolute coordinate of map</param>
        /// <param name="absY">Absolute coordinate of map</param>
        public void ConvertToAbsolute(int relX, int relY, out int absX, out int absY)
        {
            absX = this.PartitionX * this.PartitionSize + relX;
            absY = this.PartitionY * this.PartitionSize + relY;
        }

        /// <summary>
        /// Converts absolute to relative position on partition
        /// </summary>
        /// <param name="absX">Absolute coordinate of map</param>
        /// <param name="absY">Absolute coordinate of map</param>
        /// <param name="relX">Relative coordinate to partition</param>
        /// <param name="relY">Relative coordinate to partition</param>
        public void ConvertToRelative(int absX, int absY, out int relX, out int relY)
        {
            relX = absX - this.PartitionX * this.PartitionSize;
            relY = absY - this.PartitionY * this.PartitionSize;
        }

        /// <summary>
        /// Gets one column
        /// </summary>
        /// <param name="x">X-Position, relative to the partition</param>
        /// <param name="y">Y-Position, relative to the partition</param>
        /// <returns></returns>
        public List<FieldTypeChangeInfo> GetColumn(int x, int y)
        {
            // Checks boundaries
            Ensure.That(x >= 0 && x < this.PartitionSize && y >= 0 && y < this.PartitionSize, "x and y are not in partition");

            var index = y * this.PartitionSize + x;
            return this.columns[index];
        }

        /// <summary>
        /// Gets one column
        /// </summary>
        /// <param name="x">X-Position, relative to the partition</param>
        /// <param name="y">Y-Position, relative to the partition</param>
        /// <returns></returns>
        public void SetColumn(int x, int y, List<FieldTypeChangeInfo> column)
        {
            // Checks boundaries
            Ensure.That(x >= 0 && x < this.PartitionSize && y >= 0 && y < this.PartitionSize, "x and y are not in partition");

            var index = y * this.PartitionSize + x;
            this.columns[index] = column;
        }

        /// <summary>
        /// Initializes a partition and sets all fields to fieldtype 0.
        /// </summary>
        public void InitFields()
        {
            foreach (var list in this.columns)
            {
                list.Clear();
                list.InitFields();
            }
        }

        /// <summary>
        /// Stores the partition in the stream
        /// </summary>
        /// <param name="stream">Stream, where partition will be added</param>
        public void Store(Stream stream)
        {
#if DEBUG
            Ensure.That(this.IsValid());
#endif

            var data = new byte[5];
            for (var y = 0; y < this.PartitionSize; y++)
            {
                for (var x = 0; x < this.PartitionSize; x++)
                {
                    var column = this.GetColumn(x, y);

                    foreach (var info in column)
                    {
                        var byteHeight = BitConverter.GetBytes(info.ChangeHeight);
#if DEBUG
                        Ensure.That(byteHeight.Length == 4);
#endif
                        stream.WriteByte(info.FieldType);
                        stream.Write(byteHeight, 0, 4);
                    }
                }
            }
        }

        /// <summary>
        /// Loads partition from stream
        /// </summary>
        /// <param name="stream">Stream, where partition will be retrieved</param>
        public void Load(Stream stream)
        {
            var data = new byte[5];

            var current = -1;
            List<FieldTypeChangeInfo> column = null;

            while (true)
            {
                var dataRead = stream.Read(data, 0, 5);
                if (dataRead == 0)
                {
                    break;
                }

                Ensure.That(dataRead == 5);
                // Loads data from stream

                var fieldChangeInfo = new FieldTypeChangeInfo();
                fieldChangeInfo.FieldType = data[0];
                fieldChangeInfo.ChangeHeight = BitConverter.ToSingle(data, 1);

                // Checks, if we are on a new column
                if (fieldChangeInfo.ChangeHeight == float.MaxValue)
                {
                    current++;

                    var y = current / this.PartitionSize;
                    var x = current - y * this.PartitionSize;

                    column = this.GetColumn(x, y);
                    column.Clear();
                }

#if DEBUG
                Ensure.That(column != null);
#endif

                // Adds fieldchange info to column
                column.Add(fieldChangeInfo);
            }

#if DEBUG
            Ensure.That(this.IsValid());
#endif
        }
                
        /// <summary>
        /// Checks, if the column is valid
        /// </summary>
        /// <param name="x">X-Coordinate of the column</param>
        /// <param name="y">Y-Coordinate of the column</param>
        /// <returns>true, if column is valid</returns>
        public bool IsValid(int x, int y)
        {
            var column = this.GetColumn(x, y);
            return column.IsValid();
        }

        /// <summary>
        /// Checks for all columns, if information is valid
        /// </summary>
        /// <returns>true, if complete partition is valid</returns>
        public bool IsValid()
        {
            for (var x = 0; x < this.PartitionSize; x++)
            {
                for (var y = 0; y < this.PartitionSize; y++)
                {
                    if (!this.IsValid(x, y))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Gets the fieldtype for a certain position
        /// </summary>
        /// <param name="x">Relative X-Coordinate</param>
        /// <param name="y">Relative Y-Coordinate</param>
        /// <param name="height">Queried Height</param>
        /// <returns>Evaluated field type</returns>
        public byte GetFieldType(int x, int y, float height)
        {
            return this.GetColumn(x, y).GetFieldType(height);
        }

        /// <summary>
        /// Sets the field type for a certain range
        /// </summary>
        /// <param name="x">Relative X-Coordinate</param>
        /// <param name="y">Relative Y-Coordinate</param>
        /// <param name="fieldType">Fieldtype to be set</param>
        /// <param name="startingHeight">Starting Height of new field type</param>
        /// <param name="endingHeight">Ending height of new field type</param>
        public void SetFieldType(int x, int y, byte fieldType, float startingHeight, float endingHeight)
        {
            this.GetColumn(x, y).SetFieldType(fieldType, startingHeight, endingHeight);
        }

        /// <summary>
        /// Converts to string
        /// </summary>
        /// <returns>Converted string</returns>
        public override string ToString()
        {
            return string.Format("{2}: {0}-{1}", this.PartitionX, this.PartitionY, this.InstanceId);
        }
    }
}
