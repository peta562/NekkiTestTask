using Configs;
using Infrastructure;
using Spells;
using System;
using UnityEngine;
using Zenject;

namespace Player
{
    public class WizardModel
    {
        public event Action<float> HealthChanged;
        public event Action<int> ActiveSpellChanged;
        public event Action Died;

        private readonly ConfigProvider _configProvider;
        private readonly SpellController _spellController;

        private float _health;
        private float _defense;
        private float _movementSpeed;
        private float _rotationSpeed;
        private int _activeWeaponIndex;

        public float Health => _health;
        public float MovementSpeed => _movementSpeed;
        public float RotationSpeed => _rotationSpeed;
        public int ActiveSpellIndex => _activeWeaponIndex;
        public bool IsDead => _health <= 0;

        [Inject]
        public WizardModel(ConfigProvider configProvider, SpellController spellController)
        {
            _configProvider = configProvider;
            _spellController = spellController;
            
            InitializeStats();
        }

        private void InitializeStats()
        {
            WizardConfig config = _configProvider.WizardConfig;
            
            _health = config.Health;
            _defense = config.Defense;
            _movementSpeed = config.MovementSpeed;
            _rotationSpeed = config.RotationSpeed;
            _activeWeaponIndex = 0;
        }

        public void TakeDamage(float damage)
        {
            if (IsDead)
            {
                return;
            }

            float finalDamage = damage * (1f - _defense);
            _health = Mathf.Max(0, _health - finalDamage);
            
            HealthChanged?.Invoke(_health);
            
            if (_health <= 0)
            {
                Died?.Invoke();
            }
        }

        public void SwitchWeapon(int direction)
        {
            int spellCount = _configProvider.SpellsConfig.AvailableSpells.Length;
            if (spellCount == 0)
            {
                return;
            }

            _activeWeaponIndex = (_activeWeaponIndex + direction + spellCount) % spellCount;
            ActiveSpellChanged?.Invoke(_activeWeaponIndex);
        }

        public void Attack(Vector3 position, Vector3 direction)
        {
            if (IsDead)
            {
                return;
            }

            int spellCount = _configProvider.SpellsConfig.AvailableSpells.Length;
            if (spellCount == 0)
            {
                return;
            }

            SpellConfig spellConfig = _configProvider.SpellsConfig.AvailableSpells[_activeWeaponIndex];
            _spellController.CreateSpell(spellConfig, position, direction);
        }
    }
} 