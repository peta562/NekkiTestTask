using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "SpellsConfig", menuName = "Configs/SpellsConfig")]
    public class SpellsConfig : ScriptableObject
    {
        public SpellConfig[] AvailableSpells;
    }
} 