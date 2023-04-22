using System.Runtime.Serialization;

namespace Lunarsoft
{
    [System.Serializable]
    [DataContract]
    public class Stat
    {
        [DataMember]
        public float BaseValue;
        [DataMember]
        public float Growth;

        // Default constructor for deserialization
        public Stat() { }

        public Stat(float baseValue, float growth)
        {
            BaseValue = baseValue;
            Growth = growth;
        }

        public float GetValueAtLevel(int level)
        {
            float statIncrease = Growth * (0.65f + 0.035f * level);
            return BaseValue + statIncrease * (level - 1);
        }
    }
}
