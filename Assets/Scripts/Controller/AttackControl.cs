using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AttackControl : MonoBehaviour
{
    [SerializeField] private Swan s;

    public void Initialize(InputAction attackAction)
    {
        attackAction.performed += AttackAction_performed;
        attackAction.Enable();

    }

    private void AttackAction_performed(InputAction.CallbackContext obj)
    {
        //Debug.Log("Attack");
        s.attack("punch");
    }
}
