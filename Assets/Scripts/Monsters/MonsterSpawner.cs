using Configs;
using Cysharp.Threading.Tasks;
using Infrastructure;
using System;
using UnityEngine;
using Zenject;

namespace Monsters
{
    public class MonsterSpawner
    {
        private readonly ConfigProvider _configProvider;
        private readonly MonsterFactory _monsterFactory;
        private readonly MonsterController _monsterController;
        
        private bool _isSpawning;

        [Inject]
        public MonsterSpawner(
            ConfigProvider configProvider, 
            MonsterFactory monsterFactory,
            MonsterController monsterController)
        {
            _configProvider = configProvider;
            _monsterFactory = monsterFactory;
            _monsterController = monsterController;
        }

        public async UniTask StartSpawningAsync()
        {
            _isSpawning = true;
            
            while (_isSpawning)
            {
                if (_monsterController.ActiveMonstersCount < _configProvider.SceneConfig.MaxMonstersOnScene)
                {
                    SpawnMonster();
                }
                
                float delay = UnityEngine.Random.Range(
                    _configProvider.SceneConfig.MinTimeBetweenSpawns,
                    _configProvider.SceneConfig.MaxTimeBetweenSpawns
                );
                
                await UniTask.Delay(TimeSpan.FromSeconds(delay));
            }
        }

        public void StopSpawning()
        {
            _isSpawning = false;
        }

        private void SpawnMonster()
        {
            MonsterConfig[] availableMonsters = _configProvider.MonstersConfig.AvailableMonsters;
            if (availableMonsters.Length == 0)
            {
                return;
            }
            
            Vector3 spawnPosition = GetRandomSpawnPosition();
            MonsterConfig randomConfig = availableMonsters[UnityEngine.Random.Range(0, availableMonsters.Length)];
            
            Monster monster = _monsterFactory.CreateMonster(randomConfig, spawnPosition);
            if (monster != null)
            {
                _monsterController.RegisterMonster(monster);
            }
        }

        private Vector3 GetRandomSpawnPosition()
        {
            float angle = UnityEngine.Random.Range(0f, Mathf.PI * 2);
            float distance = _configProvider.SceneConfig.MonsterSpawnDistance;
            
            Vector2 spawnOffset = new Vector2(
                Mathf.Cos(angle) * distance,
                Mathf.Sin(angle) * distance
            );
            
            return spawnOffset;
        }
    }
} 