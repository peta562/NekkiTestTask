using System;
using UnityEngine;

namespace Spells
{
    public class Spell : MonoBehaviour
    {
        public event Action<Spell, Collider2D> CollisionDetected;
        public event Action<Spell> LifetimeEnded;
        
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private Rigidbody2D _rigidbody;
        
        private float _damage;
        private float _speed;
        private float _lifetime;
        private float _timeAlive;
        private Action _returnToPool;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            CollisionDetected?.Invoke(this, collision);
        }

        public void UpdateLifetime(float deltaTime)
        {
            _timeAlive += deltaTime;
            
            if (_timeAlive >= _lifetime)
            {
                LifetimeEnded?.Invoke(this);
            }
        }

        public void Initialize(float damage, float speed, float lifetime, Color color)
        {
            _damage = damage;
            _speed = speed;
            _lifetime = lifetime;
            _timeAlive = 0f;
            
            _spriteRenderer.color = color;
        }

        public void Launch(Vector3 direction)
        {
            transform.up = direction;
            _rigidbody.velocity = direction * _speed;
        }

        public void SetReturnToPool(Action returnToPool)
        {
            _returnToPool = returnToPool;
        }

        public void ReturnToPool()
        {
            _rigidbody.velocity = Vector2.zero;
            _returnToPool?.Invoke();
        }

        public float GetDamage()
        {
            return _damage;
        }
    }
} 