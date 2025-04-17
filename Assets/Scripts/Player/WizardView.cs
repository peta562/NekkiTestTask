using System;
using UnityEngine;

namespace Player
{
    public class WizardView : MonoBehaviour
    {
        public event Action<WizardView, Collider2D> CollisionDetected;
        
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private Transform _spellSpawnPoint;

        public Transform SpellSpawnPoint => _spellSpawnPoint;

        public void SetVelocity(Vector3 velocity)
        {
            _rigidbody.velocity = new Vector2(velocity.x, velocity.y);
        }

        public void SetRotation(Quaternion targetRotation, float rotationSpeed)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }

        public void PlayDeathAnimation()
        {
            gameObject.SetActive(false);
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            CollisionDetected?.Invoke(this, collider);
        }
    }
} 