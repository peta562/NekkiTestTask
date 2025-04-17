using System;
using Infrastructure;
using UnityEngine;
using Zenject;

public class DesktopInputHandler : ITickable, IInputHandler
{
    public event Action<Vector2> MoveInput;
    public event Action<Vector2> LookInput;
    public event Action AttackInput;
    public event Action<int> SwitchWeaponInput;

    public void Tick()
    {
        ProcessDirectionalInput();
        ProcessAttackInput();
        ProcessWeaponSwitchInput();
    }

    private void ProcessDirectionalInput()
    {
        float horizontal = 0f;
        float vertical = 0f;
        
        if (Input.GetKey(KeyCode.UpArrow))
            vertical += 1f;
        if (Input.GetKey(KeyCode.DownArrow))
            vertical -= 1f;
        if (Input.GetKey(KeyCode.LeftArrow))
            horizontal -= 1f;
        if (Input.GetKey(KeyCode.RightArrow))
            horizontal += 1f;
        
        
        Vector2 direction = new Vector2(horizontal, vertical).normalized;
        
        MoveInput?.Invoke(direction);

        if (horizontal != 0f || vertical != 0f)
        {
            LookInput?.Invoke(direction);
        }
    }

    private void ProcessAttackInput()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            AttackInput?.Invoke();
        }
    }

    private void ProcessWeaponSwitchInput()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SwitchWeaponInput?.Invoke(-1);
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            SwitchWeaponInput?.Invoke(1);
        }
    }
} 