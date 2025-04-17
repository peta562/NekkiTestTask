using Configs;
using UnityEngine;
using Zenject;

namespace Infrastructure
{
    public class ConfigProvider
    {
        public SceneConfig SceneConfig { get; }
        public WizardConfig WizardConfig { get; }
        public SpellsConfig SpellsConfig { get; }
        public MonstersConfig MonstersConfig { get; }
        
        public ConfigProvider(
            SceneConfig sceneConfig,
            WizardConfig wizardConfig,
            SpellsConfig spellsConfig,
            MonstersConfig monstersConfig)
        {
            SceneConfig = sceneConfig;
            WizardConfig = wizardConfig;
            SpellsConfig = spellsConfig;
            MonstersConfig = monstersConfig;
        }
    }
} 