using Configs;
using Monsters;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Spells
{
    public class SpellController : ITickable
    {
        private readonly SpellFactory _spellFactory;
        private readonly List<Spell> _activeSpells = new();

        [Inject]
        public SpellController(SpellFactory spellFactory)
        {
            _spellFactory = spellFactory;
        }

        public void Tick()
        {
            UpdateSpells();
        }

        public void CreateSpell(SpellConfig config, Vector3 position, Vector3 direction)
        {
            Spell spell = _spellFactory.CreateSpell(config, position, direction);
            if (spell != null)
            {
                spell.CollisionDetected += HandleSpellCollision;
                spell.LifetimeEnded += HandleSpellLifetimeEnded;
                _activeSpells.Add(spell);
            }
        }

        private void UpdateSpells()
        {
            for (int i = _activeSpells.Count - 1; i >= 0; i--)
            {
                Spell spell = _activeSpells[i];
                if (spell == null)
                {
                    _activeSpells.RemoveAt(i);
                    continue;
                }

                spell.UpdateLifetime(Time.deltaTime);
            }
        }

        private void HandleSpellCollision(Spell spell, Collider2D collision)
        {
            if (spell == null) 
                return;

            Monster monster = collision.GetComponentInParent<Monster>();
            if (monster != null)
            {
                float damage = spell.GetDamage();
                monster.TakeDamage(damage);
                DestroySpell(spell);
            }
        }

        private void HandleSpellLifetimeEnded(Spell spell)
        {
            DestroySpell(spell);
        }

        private void DestroySpell(Spell spell)
        {
            if (spell == null || !_activeSpells.Contains(spell)) 
                return;

            spell.CollisionDetected -= HandleSpellCollision;
            spell.LifetimeEnded -= HandleSpellLifetimeEnded;
            _activeSpells.Remove(spell);
            spell.ReturnToPool();
        }
    }
}
