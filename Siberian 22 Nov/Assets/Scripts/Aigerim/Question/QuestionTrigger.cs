using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionTrigger : MonoBehaviour
{
    public delegate void OnCharacterTriggered();
    public event OnCharacterTriggered OnEventCharacterTriggered;

    public void Func(OnCharacterTriggered onCharacterTriggered)
    {
        OnEventCharacterTriggered += onCharacterTriggered;
    }

    private void OnMouseDown()
    {
        if (this.GetComponent<Collider>() != null)
        {
            Debug.Log("onmousedown");
            OnEventCharacterTriggered?.Invoke();
        }
    }
}
