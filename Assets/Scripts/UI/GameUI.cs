using Configs;
using Infrastructure;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI
{
    public class GameUI : MonoBehaviour
    {
        [SerializeField] private Slider _healthSlider;
        [SerializeField] private TextMeshProUGUI _currentSpellText;
        [SerializeField] private TextMeshProUGUI _healthText;
        
        private WizardModel _wizardModel;
        private ConfigProvider _configProvider;

        [Inject]
        public void Construct(WizardModel wizardModel, ConfigProvider configProvider)
        {
            _wizardModel = wizardModel;
            _configProvider = configProvider;
        }
        
        public void Initialize()
        {
            _wizardModel.HealthChanged += UpdateHealth;
            _wizardModel.ActiveSpellChanged += UpdateCurrentSpell;
            
            UpdateHealth(_wizardModel.Health);
            UpdateCurrentSpell(_wizardModel.ActiveSpellIndex);
        }

        public void Deinitialize()
        {
            _wizardModel.HealthChanged -= UpdateHealth;
            _wizardModel.ActiveSpellChanged -= UpdateCurrentSpell;
        }

        private void UpdateHealth(float health)
        {
            _healthSlider.maxValue = _configProvider.WizardConfig.Health;
            _healthSlider.value = health;
            
            _healthText.text = $"Health: {health:F0}";
        }

        private void UpdateCurrentSpell(int spellIndex)
        {
            if (_configProvider.SpellsConfig.AvailableSpells.Length > 0)
            {
                SpellConfig spellConfig = _configProvider.SpellsConfig.AvailableSpells[spellIndex];
                _currentSpellText.text = $"Spell: {spellConfig.Name} (Damage: {spellConfig.Damage})";
            }
        }
    }
} 