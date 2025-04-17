using System.Collections.Generic;
using Configs;
using Infrastructure;
using UnityEngine;
using Zenject;

namespace Monsters
{
    public class MonsterFactory
    {
        private readonly DiContainer _container;
        private readonly Player.WizardView _wizardView;
        private readonly Dictionary<string, ObjectPool<Monster>> _pools = new();

        [Inject]
        public MonsterFactory(DiContainer container, Player.WizardView wizardView)
        {
            _container = container;
            _wizardView = wizardView;
        }

        public void Preload(MonsterConfig config, int count)
        {
            if (!_pools.ContainsKey(config.Name))
            {
                _pools[config.Name] = CreatePool(config.Name, config.Prefab, count);
            }
            else
            {
                _pools[config.Name].Preload(count);
            }
        }

        public Monster CreateMonster(MonsterConfig config, Vector3 position)
        {
            GameObject prefab = config.Prefab;
            if (prefab == null)
            {
                Debug.LogError("Monster prefab is null! Make sure to assign a prefab in MonsterConfig.");
                return null;
            }

            if (!_pools.TryGetValue(config.Name, out var pool))
            {
                pool = CreatePool(config.Name, prefab, 5);
                _pools[config.Name] = pool;
            }

            Monster monster = pool.Get();
            monster.transform.position = position;

            monster.Initialize(
                config.Health,
                config.Damage,
                config.Defense,
                config.MovementSpeed,
                config.Color,
                _wizardView.transform
            );

            monster.SetReturnToPool(() => pool.Return(monster));

            return monster;
        }

        private ObjectPool<Monster> CreatePool(string name, GameObject prefab, int initialSize)
        {
            return new ObjectPool<Monster>(
                createFunc: () => _container.InstantiatePrefabForComponent<Monster>(prefab),
                initialSize: initialSize,
                canGrow: true,
                new GameObject($"{name}(Pool)").transform
            );
        }
    }
}
