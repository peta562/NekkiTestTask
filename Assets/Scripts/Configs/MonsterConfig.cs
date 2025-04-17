using UnityEngine;

namespace Configs
{
    [System.Serializable]
    public class MonsterConfig
    {
        public string Name;
        public GameObject Prefab;
        public float Health;
        public float Damage;
        [Range(0f, 1f)]
        public float Defense;
        public float MovementSpeed;
        public Color Color = Color.red;
    }
} 