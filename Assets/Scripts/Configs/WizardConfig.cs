using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "WizardConfig", menuName = "Configs/WizardConfig")]
    public class WizardConfig : ScriptableObject
    {
        public float Health = 100f;
        [Range(0f, 1f)]
        public float Defense = 0.5f;
        public float MovementSpeed = 5f;
        public float RotationSpeed = 180f;
    }
} 