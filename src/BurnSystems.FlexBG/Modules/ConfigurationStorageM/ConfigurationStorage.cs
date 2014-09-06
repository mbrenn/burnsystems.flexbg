using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace BurnSystems.FlexBG.Modules.ConfigurationStorageM
{
    /// <summary>
    /// Defines the configuration storage
    /// </summary>
    public class ConfigurationStorage : IConfigurationStorage
    {
        /// <summary>
        /// Stores the configuration documents
        /// </summary>
        private List<XDocument> configurationDocuments = new List<XDocument>();

        public IConfigurationStorage Add(XDocument document)
        {
            this.configurationDocuments.Add(document);
            return this;
        }

        public ConfigurationStorage()
        {
        }

        /// <summary>
        /// Loads all xmlfiles from directory
        /// </summary>
        /// <param name="directoryPath">Directory from which the xml files shall be loaded</param>
        public ConfigurationStorage AddFromDirectory(string directoryPath)
        {
            foreach (var file in Directory.GetFiles(directoryPath, "*.xml").OrderBy(x => x))
            {
                this.Add(XDocument.Load(file));
            }

            return this;
        }

        /// <summary>
        /// Gets the configuration documents
        /// </summary>
        public IEnumerable<System.Xml.Linq.XDocument> Documents
        {
            get { return this.configurationDocuments; }
        }
    }
}
