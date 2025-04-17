using Configs;
using Game_Field;
using Monsters;
using Player;
using Spells;
using UI;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Infrastructure
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private SceneConfig _sceneConfig;
        [SerializeField] private WizardConfig _wizardConfig;
        [SerializeField] private SpellsConfig _spellsConfig;
        [SerializeField] private MonstersConfig _monstersConfig;
        [SerializeField] private WizardView _wizardViewPrefab;
        [SerializeField] private GameField _gameFieldPrefab;
        [SerializeField] private GameUI _gameUI;

        public override void InstallBindings()
        {
            BindConfigs();
            BindPlayer();
            BindSpells();
            BindMonsters();
            BindUI();
            BindUtils();
            BindMainController();

            Container.BindInterfacesAndSelfTo<GameBootstrapper>().AsSingle();
        }

        private void BindConfigs()
        {
            Container.Bind<SceneConfig>().FromInstance(_sceneConfig).AsSingle();
            Container.Bind<WizardConfig>().FromInstance(_wizardConfig).AsSingle();
            Container.Bind<SpellsConfig>().FromInstance(_spellsConfig).AsSingle();
            Container.Bind<MonstersConfig>().FromInstance(_monstersConfig).AsSingle();
            Container.Bind<ConfigProvider>().AsSingle();
        }

        private void BindPlayer()
        {
            Container.Bind<WizardView>().FromComponentInNewPrefab(_wizardViewPrefab).AsSingle();
            Container.BindInterfacesAndSelfTo<WizardController>().AsSingle();
            Container.Bind<WizardModel>().AsSingle();
            
            Container.BindInterfacesAndSelfTo<DesktopInputHandler>().AsSingle();
        }

        private void BindSpells()
        {
            Container.Bind<SpellFactory>().AsSingle();
            Container.BindInterfacesAndSelfTo<SpellController>().AsSingle();
        }

        private void BindMonsters()
        {
            Container.Bind<MonsterFactory>().AsSingle();
            Container.BindInterfacesAndSelfTo<MonsterController>().AsSingle();
            Container.Bind<MonsterSpawner>().AsSingle();
        }
        
        private void BindUI()
        {
            Container.Bind<GameUI>().FromInstance(_gameUI).AsSingle();
        }
        
        private void BindUtils()
        {
            Container.Bind<GameField>().FromComponentInNewPrefab(_gameFieldPrefab).UnderTransform(transform).AsSingle();
        }
        
        private void BindMainController()
        {
            Container.Bind<MainGameController>().AsSingle();
        }
    }
} 