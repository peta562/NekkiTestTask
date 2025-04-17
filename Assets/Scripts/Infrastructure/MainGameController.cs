using Cysharp.Threading.Tasks;
using Monsters;
using Player;
using Spells;
using System;
using System.Threading;
using Game_Field;
using UI;
using UnityEngine;
using Zenject;

namespace Infrastructure
{
    public class MainGameController
    {
        private readonly WizardController _wizardController;
        private readonly MonsterSpawner _monsterSpawner;
        private readonly GameUI _gameUI;
        private readonly GameField _gameField;
        private readonly ConfigProvider _configProvider;
        private readonly SpellFactory _spellFactory;
        private readonly MonsterFactory _monsterFactory;
        private readonly MonsterController _monsterController;

        private CancellationTokenSource _gameSessionCts;
        private GameState _currentGameState = GameState.None;

        [Inject]
        public MainGameController(
            WizardController wizardController,
            MonsterSpawner monsterSpawner,
            GameUI gameUI,
            GameField gameField,
            ConfigProvider configProvider,
            SpellFactory spellFactory,
            MonsterFactory monsterFactory,
            MonsterController monsterController)
        {
            _wizardController = wizardController;
            _monsterSpawner = monsterSpawner;
            _gameUI = gameUI;
            _gameField = gameField;
            _configProvider = configProvider;
            _spellFactory = spellFactory;
            _monsterFactory = monsterFactory;
            _monsterController = monsterController;
        }

        public async UniTaskVoid StartGameAsync()
        {
            if (_currentGameState == GameState.Playing)
            {
                return;
            }

            _gameSessionCts?.Cancel();
            _gameSessionCts = new CancellationTokenSource();
            
            try
            {
                await InitializeGameAsync(_gameSessionCts.Token);
            }
            catch (OperationCanceledException)
            {
                Debug.Log("Game session was canceled");
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error in game session: {ex.Message}");
            }
        }

        public void PauseGame()
        {
            if (_currentGameState != GameState.Playing)
            {
                return;
            }
            
            _currentGameState = GameState.Paused;
            Time.timeScale = 0f;
        }

        public void ResumeGame()
        {
            if (_currentGameState != GameState.Paused)
            {
                return;
            }
            
            _currentGameState = GameState.Playing;
            Time.timeScale = 1f;
        }

        private void StopGame()
        {
            _wizardController.Died -= StopGame;
            
            _gameSessionCts?.Cancel();
            _currentGameState = GameState.Ended;
            
            _monsterSpawner.StopSpawning();
            _monsterController.Deinitialize();
            _wizardController.Deinitialize();
            _gameUI.Deinitialize();
            
            Time.timeScale = 1f;
        }

        private async UniTask InitializeGameAsync(CancellationToken cancellationToken)
        {
            _currentGameState = GameState.Initializing;
            _wizardController.Died += StopGame;

            foreach (var spellConfig in _configProvider.SpellsConfig.AvailableSpells)
            {
                _spellFactory.Preload(spellConfig, 10);
            }

            foreach (var monsterConfig in _configProvider.MonstersConfig.AvailableMonsters)
            {
                _monsterFactory.Preload(monsterConfig, 5);
            }

            _gameField.Initialize();
            _wizardController.Initialize(_gameField.SpawnPoint.position);
            _gameUI.Initialize();

            await UniTask.Delay(TimeSpan.FromSeconds(0.1f), cancellationToken: cancellationToken);

            _monsterSpawner.StartSpawningAsync().Forget();
            
            _currentGameState = GameState.Playing;
            Time.timeScale = 1f;
        }
    }
} 