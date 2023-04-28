using UnityEngine;

namespace Lunarsoft
{
    [CreateAssetMenu(fileName = "New Character", menuName = "Lunarsoft/CharacterStats")]
    public class CharacterStats : ScriptableObject
    {
        public string Name { get; set; }

        public int level;
        public int experience;

        public Stat Health = new Stat();
        public Stat HealthRegeneration = new Stat();

        public Stat AttackDamage = new Stat();
        public Stat AttackSpeed = new Stat();

        public Stat Armor = new Stat();
        public Stat ArmorPenetration = new Stat();

        public Stat MagicResistance = new Stat();
        public Stat MagicAttack = new Stat();

        public Stat Mana = new Stat();
        public Stat ManaRegeneration = new Stat();

        public Stat Speed = new Stat();

        public Stat Range = new Stat();

        public Stat GoldGeneration = new Stat();

        public Stat CriticalStrikeChance = new Stat();
        public Stat CriticalStrikeDamage = new Stat();

        public Stat LifeSteal = new Stat();

        public void AddExperience(int experiencePoints)
        {
            experience += experiencePoints;
            CheckForLevelUp();
        }

        private void CheckForLevelUp()
        {
            while (experience >= ExperienceToNextLevel(level))
            {
                level++;
            }
        }

        private int ExperienceToNextLevel(int currentLevel)
        {
            int baseExperience;
            int experienceIncrement;
            int levelOffset;

            if (currentLevel >= 1 && currentLevel < 30)
            {
                baseExperience = 144;
                experienceIncrement = 8;
                levelOffset = currentLevel - 1;
            }
            else if (currentLevel >= 30 && currentLevel < 50)
            {
                baseExperience = 2688;
                experienceIncrement = 16;
                levelOffset = currentLevel - 30;
            }
            else
            {
                baseExperience = 2592;
                experienceIncrement = 96;
                levelOffset = currentLevel - 50;
            }

            return baseExperience + experienceIncrement * levelOffset;
        }
    }
}
