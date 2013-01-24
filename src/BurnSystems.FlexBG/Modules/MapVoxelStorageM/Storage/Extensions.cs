using BurnSystems.Logging;
using BurnSystems.Test;
using System.Collections.Generic;

namespace BurnSystems.FlexBG.Modules.MapVoxelStorageM.Storage
{
    /// <summary>
    /// Stores some extension methods make access easy
    /// </summary>
    public static class Extensions
    {
        static ILog logger = new ClassLogger(typeof(Extensions));

        /// <summary>
        /// Initializes the columns
        /// </summary>
        /// <param name="list"></param>
        public static void InitFields(this List<FieldTypeChangeInfo> list)
        {
            // First: Clear
            list.Clear();

            // Add air
            var info = new FieldTypeChangeInfo();
            info.Init();

            list.Add(info);
        }

        /// <summary>
        /// Checks, if the column is valud
        /// </summary>
        /// <param name="column">Column to be checked</param>
        /// <returns>true, if column is valid</returns>
        public static bool IsValid(this List<FieldTypeChangeInfo> column)
        {
            // Checks, if we have a value
            if (column.Count <= 0)
            {
                logger.LogEntry(LogLevel.Fail, "Column is not valid: column.Count <= 0");
                return false;
            }

            // Checks, if we have a maximum value
            if (column[0].ChangeHeight != float.MaxValue)
            {
                logger.LogEntry(LogLevel.Fail, "Column is not valid: column[0].ChangeHeight != float.MaxValue");
                return false;
            }

            // Checks if height information is ordered
            var lastHeight = float.MaxValue;
            var lastFieldType = column[0].FieldType;
            for (var m = 1; m < column.Count; m++)
            {
                var newHeight = column[m].ChangeHeight;
                var newFieldType = column[m].FieldType;
                if (newHeight >= lastHeight)
                {
                    logger.LogEntry(LogLevel.Fail, "Column is not valid: Not correctly ordered");
                    return false;
                }

                if (newFieldType == lastFieldType)
                {
                    // Field types have to change, otherwise we do not have a field change
                    logger.LogEntry(LogLevel.Fail, "Column is not valid: Fieldtype has not changed");
                    return false;
                }

                lastHeight = newHeight;
                lastFieldType = newFieldType;
            }

            return true;
        }

        /// <summary>
        /// Gets the field type for a certain column at a certain height
        /// </summary>
        /// <param name="column">Column to be queried</param>
        /// <param name="height">Height of the map</param>
        /// <returns>Fieldtype</returns>
        public static byte GetFieldType(this List<FieldTypeChangeInfo> column, float height)
        {
            byte result = 0;

            foreach (var fieldTypeInfo in column)
            {
                if (fieldTypeInfo.ChangeHeight >= height)
                {
                    result = fieldTypeInfo.FieldType;
                }
                else
                {
                    return result;
                }
            }

            return result;
        }

        /// <summary>
        /// Sets the field type for a certain range
        /// </summary>
        /// <param name="column">Column of the fieldtype</param>
        /// <param name="fieldType">Fieldtype to be set</param>
        /// <param name="startingHeight">Starting Height of new field type</param>
        /// <param name="endingHeight">Ending height of new field type</param>
        public static void SetFieldType(this List<FieldTypeChangeInfo> column, byte fieldType, float startingHeight, float endingHeight)
        {
            if (startingHeight < endingHeight)
            {
                SetFieldType(column, fieldType, endingHeight, startingHeight);
                return;
            }

            if (startingHeight == endingHeight)
            {
                // Nothing to do
                return;
            }

            // Findout now the current fieldtype at start and end
            var startingFieldType = column.GetFieldType(startingHeight);
            var endingFieldType = column.GetFieldType(endingHeight);

            // Removes all fields equal to or between boundaries
            column.RemoveAll(x =>
            {
                var height = x.ChangeHeight;
                return height <= startingHeight && height >= endingHeight;
            });

            if (column.GetFieldType(startingHeight) != fieldType || column.Count == 0)
            {
                // Adds top with new fieldtype
                var topField = new FieldTypeChangeInfo();
                topField.FieldType = fieldType;
                topField.ChangeHeight = startingHeight;
                column.Add(topField);
            }

            // Sorts
            column.Sort((x, y) =>
            {
                return y.ChangeHeight.CompareTo(x.ChangeHeight);
            });

            if (column.GetFieldType(endingHeight) != endingFieldType && endingHeight > float.MinValue)
            {
                // Adds bottom changing back to old field type
                var endField = new FieldTypeChangeInfo();
                endField.FieldType = endingFieldType;
                endField.ChangeHeight = endingHeight;
                column.Add(endField);
            }

            // Sorts
            column.Sort((x,y) =>
                {
                    return y.ChangeHeight.CompareTo(x.ChangeHeight);
                });

            // Check, if everything is correct
#if DEBUG
            Ensure.That(column.IsValid());
#endif
        }


        /// <summary>
        /// Gets an enumeration of heights, where a change to this fieldtype occurs. 
        /// If no change occures, null is returned
        /// </summary>
        /// <param name="column">Column to be evaluated</param>
        /// <param name="fieldType">Fieldtype being queried</param>
        public static IEnumerable<float> GetHeightsOfFieldType(this List<FieldTypeChangeInfo> column, byte fieldType)
        {
            for(var n = 0; n < column.Count; n++)
            {
                var change = column[n];
                if (change.FieldType == fieldType)
                {
                    yield return change.ChangeHeight;
                }
            }
        }
    }
}
