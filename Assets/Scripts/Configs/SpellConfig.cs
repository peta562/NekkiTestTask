using UnityEngine;

namespace Configs
{
    [System.Serializable]
    public class SpellConfig
    {
        public string Name;
        public GameObject Prefab;
        public float Damage;
        public float Speed;
        public float Lifetime;
        public Color Color = Color.blue;
    }
} 