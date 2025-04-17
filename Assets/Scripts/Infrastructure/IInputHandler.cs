using System;
using UnityEngine;

namespace Infrastructure
{
    public interface IInputHandler
    {
        event Action<Vector2> MoveInput;
        event Action<Vector2> LookInput;
        event Action AttackInput;
        event Action<int> SwitchWeaponInput;
    }
} 