using System;
using Infrastructure;
using Monsters;
using UnityEngine;
using Zenject;

namespace Player
{
    public class WizardController : ITickable
    {
        public event Action Died;
        
        private readonly WizardModel _wizardModel;
        private readonly WizardView _wizardView;
        private readonly IInputHandler _inputHandler;

        private Vector3 _moveDirection;
        private Vector3 _lookDirection;

        [Inject]
        public WizardController(
            WizardModel wizardModel,
            WizardView wizardView,
            IInputHandler inputHandler)
        {
            _wizardModel = wizardModel;
            _wizardView = wizardView;
            _inputHandler = inputHandler; ;
        }

        public void Initialize(Vector2 spawnPosition)
        {
            _wizardView.transform.position = spawnPosition;
            
            _wizardModel.Died += OnWizardDied;
            _inputHandler.MoveInput += HandleMoveInput;
            _inputHandler.LookInput += HandleLookInput;
            _inputHandler.AttackInput += HandleAttackInput;
            _inputHandler.SwitchWeaponInput += HandleSwitchWeaponInput;
            _wizardView.CollisionDetected += HandleCollision;
        }

        public void Deinitialize()
        {
            _wizardModel.Died -= OnWizardDied;
            _inputHandler.MoveInput -= HandleMoveInput;
            _inputHandler.LookInput -= HandleLookInput;
            _inputHandler.AttackInput -= HandleAttackInput;
            _inputHandler.SwitchWeaponInput -= HandleSwitchWeaponInput;
            _wizardView.CollisionDetected -= HandleCollision;
        }

        public void Tick()
        {
            if (_wizardModel.IsDead)
            {
                return;
            }

            UpdateMovement();
            UpdateRotation();
        }

        private void UpdateMovement()
        {
            Vector3 velocity = _moveDirection * _wizardModel.MovementSpeed;
            _wizardView.SetVelocity(velocity);
        }

        private void UpdateRotation()
        {
            if (_lookDirection == Vector3.zero)
            {
                return;
            }

            float angle = Mathf.Atan2(_lookDirection.y, _lookDirection.x) * Mathf.Rad2Deg - 90;
            Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
            _wizardView.SetRotation(targetRotation, _wizardModel.RotationSpeed);
        }

        private void HandleMoveInput(Vector2 direction)
        {
            _moveDirection = new Vector3(direction.x, direction.y, 0);
        }

        private void HandleLookInput(Vector2 direction)
        {
            _lookDirection = new Vector3(direction.x, direction.y, 0);
        }

        private void HandleAttackInput()
        {
            Vector3 spawnPosition = _wizardView.SpellSpawnPoint.position;
            Vector3 direction = _lookDirection.normalized;
            _wizardModel.Attack(spawnPosition, direction);
        }

        private void HandleSwitchWeaponInput(int direction)
        {
            _wizardModel.SwitchWeapon(direction);
        }

        private void OnWizardDied()
        {
            _wizardView.PlayDeathAnimation();
            Died?.Invoke();
        }
        
        private void HandleCollision(WizardView wizardView, Collider2D collision)
        {
            Monster monster = collision.GetComponent<Monster>();
            
            if (monster != null && !_wizardModel.IsDead)
            {
                _wizardModel.TakeDamage(monster.Damage);
                monster.ForceDie();
            }
        }
    }
} 