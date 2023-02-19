using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewObject", menuName = "ScriptableObject/Object")]
public class ObjectParametersSO : ScriptableObject, IRequiredParameters
{
    [SerializeField] private Parameter[] _parameters;

    public Parameter[] Parameters => _parameters;

    public void Setup(List<ParameterSO> parameters)
    {
        List<Parameter> param = new List<Parameter>();
        foreach (ParameterSO p in parameters)
            param.Add(new Parameter(p.name, 0));
        _parameters = param.ToArray();
    }
}
