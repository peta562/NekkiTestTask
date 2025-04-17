using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "SceneConfig", menuName = "Configs/SceneConfig")]
    public class SceneConfig : ScriptableObject
    {
        public int MaxMonstersOnScene = 10;
        public float MonsterSpawnDistance = 15f;
        public float MinTimeBetweenSpawns = 1f;
        public float MaxTimeBetweenSpawns = 3f;
    }
} 