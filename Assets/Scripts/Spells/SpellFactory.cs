using System.Collections.Generic;
using Configs;
using Infrastructure;
using UnityEngine;
using Zenject;

namespace Spells
{
    public class SpellFactory
    {
        private readonly DiContainer _container;
        private readonly Dictionary<string, ObjectPool<Spell>> _pools = new();

        [Inject]
        public SpellFactory(DiContainer container)
        {
            _container = container;
        }

        public void Preload(SpellConfig config, int count)
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

        public Spell CreateSpell(SpellConfig config, Vector3 position, Vector3 direction)
        {
            var prefab = config.Prefab;
            if (prefab == null)
            {
                Debug.LogError("Spell prefab is null! Assign a prefab in SpellConfig.");
                return null;
            }

            if (!_pools.TryGetValue(config.Name, out var pool))
            {
                pool = CreatePool(config.Name, config.Prefab, 5);
                _pools[config.Name] = pool;
            }

            Spell spell = pool.Get();
            spell.transform.position = position;
            spell.Initialize(config.Damage, config.Speed, config.Lifetime, config.Color);
            spell.Launch(direction);
            
            spell.SetReturnToPool(() => pool.Return(spell));

            return spell;
        }

        private ObjectPool<Spell> CreatePool(string name, GameObject prefab, int initialSize)
        {
            return new ObjectPool<Spell>(
                createFunc: () => _container.InstantiatePrefabForComponent<Spell>(prefab),
                initialSize: initialSize,
                canGrow: true,
                new GameObject($"{name}(Pool)").transform
            );
        }
    }
}