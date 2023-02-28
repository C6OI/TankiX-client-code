using System;
using UnityEngine;

namespace Tanks.Lobby.ClientSettings.API {
    public class Quality {
        public enum QualityLevel {
            Fastest = 0,
            Minimum = 1,
            Meduim = 2,
            High = 3,
            Maximum = 4,
            Ultra = 5
        }

        public static Quality fastest = new("Fastest", 0);

        public static Quality ultra = new("Ultra", 5);

        public static Quality high = new("High", 3);

        public static Quality maximum = new("Maximum", 4);

        public static Quality medium = new("Medium", 2);

        public static Quality mininum = new("Minimum", 1);

        static readonly Quality[] qualities = new Quality[6] { fastest, mininum, medium, high, maximum, ultra };

        public Quality(string name, int level) {
            Name = name;
            Level = level;
        }

        public string Name { get; }

        public int Level { get; }

        public static void ValidateQualities() {
            for (int i = 0; i < QualitySettings.names.Length; i++) {
                for (int j = 0; j < qualities.Length; j++) {
                    Quality quality = qualities[i];

                    if (!quality.Name.Equals(QualitySettings.names[i]) || i != quality.Level) {
                        throw new Exception(string.Format("There is no quality {0} with level {1}", quality.Name, quality.Level));
                    }
                }
            }
        }

        public static Quality GetQualityByName(string qualityName) {
            qualityName = qualityName.ToLower();

            for (int i = 0; i < qualities.Length; i++) {
                Quality quality = qualities[i];

                if (quality.Name.ToLower().Equals(qualityName)) {
                    return quality;
                }
            }

            throw new ArgumentException("Quality with name " + qualityName + " was not found.");
        }
    }
}