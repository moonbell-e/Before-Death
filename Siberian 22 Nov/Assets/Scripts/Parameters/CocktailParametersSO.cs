using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "NewCocktailParameter", menuName = "ScriptableObject/CocktailParameter")]
public class CocktailParametersSO : ScriptableObject, IRequiredParameters
{
    [SerializeField] private string _name;
    [SerializeField] [TextArea] private string _description;
    [SerializeField] private Parameter[] _parameters;


    public string Name => _name;
    public Parameter[] Parameters => _parameters;
    public string Description => _description;

    public void Setup(List<ParameterSO> parameters)
    {
        List<Parameter> param = new List<Parameter>();
        foreach (ParameterSO p in parameters)
            param.Add(new Parameter(p.name, 0));
        _parameters = param.ToArray();
    }
}
