using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Monsters
{
    public class MonsterController : ITickable
    {
        private readonly List<Monster> _activeMonsters = new();

        public int ActiveMonstersCount => _activeMonsters.Count;

        public void Tick()
        {
            for (int i = _activeMonsters.Count - 1; i >= 0; i--)
            {
                Monster monster = _activeMonsters[i];
                if (monster == null)
                {
                    _activeMonsters.RemoveAt(i);
                    continue;
                }

                UpdateMonsterMovement(monster);
            }
        }

        public void Deinitialize()
        {
            for (var i = _activeMonsters.Count - 1; i >= 0; i--)
            {
                var monster = _activeMonsters[i];
                monster.ForceDie();
            }

            _activeMonsters.Clear();
        }

        public void RegisterMonster(Monster monster)
        {
            if (monster == null || _activeMonsters.Contains(monster)) 
                return;

            _activeMonsters.Add(monster);
            monster.HealthZero += HandleMonsterHealthZero;
        }

        private void UnregisterMonster(Monster monster)
        {
            if (monster == null || !_activeMonsters.Contains(monster)) 
                return;

            monster.HealthZero -= HandleMonsterHealthZero;
            _activeMonsters.Remove(monster);
        }

        private void UpdateMonsterMovement(Monster monster)
        {
            Transform target = monster.Target;
            if (target == null) return;

            Vector3 monsterPosition = monster.Position;
            Vector2 direction = (target.position - monsterPosition).normalized;

            monster.Move(direction);

            if (direction != Vector2.zero)
            {
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
                monster.Rotate(Quaternion.Euler(0, 0, angle));
            }
        }

        private void HandleMonsterHealthZero(Monster monster)
        {
            UnregisterMonster(monster);
            monster.ReturnToPool();
        }
    }
}