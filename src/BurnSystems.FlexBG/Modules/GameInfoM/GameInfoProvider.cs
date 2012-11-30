using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;
using System.Text;
using BurnSystems.FlexBG.Modules.ConfigurationStorageM;
using System.Xml.Serialization;
using BurnSystems.Test;
using BurnSystems.ObjectActivation;

namespace BurnSystems.FlexBG.Modules.GameInfoM
{
    public class GameInfoProvider : IGameInfoProvider
    {
        [Inject]
        public GameInfoProvider(IConfigurationStorage storage)
        {
            Ensure.That(storage != null, "IConfigurationStorage not set in Constructor");

            var xmlInfo = storage.Documents
                .Elements("FlexBG")
                .Elements("Game")
                .Elements("GameInfo")
                .LastOrDefault();

            Ensure.That(xmlInfo != null, "Xml-Configuration within flexbg/game/info not found");

            var serializer = new XmlSerializer(typeof(GameInfo));
            this.GameInfo = serializer.Deserialize(xmlInfo.CreateReader()) as GameInfo;
        }

        public GameInfo GameInfo
        {
            get;
            private set;
        }
    }
}
