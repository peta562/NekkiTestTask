using System;
using UnityEngine;

namespace Monsters
{
    public class Monster : MonoBehaviour
    {
        public event Action<Monster> HealthZero;

        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private Collider2D _collider;

        private float _health;
        private float _damage;
        private float _defense;
        private float _movementSpeed;
        private Transform _target;

        private Action _returnToPool;

        public Transform Target => _target;
        public Vector3 Position => transform.position;
        public float Damage => _damage;

        public void Initialize(float health, float damage, float defense, float movementSpeed, Color color, Transform target)
        {
            _health = health;
            _damage = damage;
            _defense = defense;
            _movementSpeed = movementSpeed;
            _target = target;

            _spriteRenderer.color = color;
        }

        public void SetReturnToPool(Action returnToPool)
        {
            _returnToPool = returnToPool;
        }

        public void ReturnToPool()
        {
            _rigidbody.velocity = Vector2.zero;
            gameObject.SetActive(false);
            _returnToPool?.Invoke();
        }

        public void TakeDamage(float damage)
        {
            float finalDamage = damage * (1f - _defense);
            _health = Mathf.Max(0, _health - finalDamage);

            if (_health <= 0)
            {
                HealthZero?.Invoke(this);
            }
        }

        public void ForceDie()
        {
            _health = 0;
            HealthZero?.Invoke(this);
        }

        public void Move(Vector2 direction)
        {
            _rigidbody.velocity = direction * _movementSpeed;
        }

        public void Rotate(Quaternion rotation)
        {
            transform.rotation = rotation;
        }
    }
}