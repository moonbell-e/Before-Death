using UnityEngine;
using System;

[CreateAssetMenu (fileName = "NewParameter", menuName = "ScriptableObject/Parameter")]
public class ParameterSO : ScriptableObject
{

}

[Serializable]
public class Parameter
{
    [SerializeField] private string _name;
    [SerializeField] private int _value;

    public string Name => _name;
    public int Value => _value;

    public Parameter(string name, int value)
    {
        _name = name;
        _value = value;
    }
}