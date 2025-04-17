using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "MonstersConfig", menuName = "Configs/MonstersConfig")]
    public class MonstersConfig : ScriptableObject
    {
        public MonsterConfig[] AvailableMonsters;
    }
} 