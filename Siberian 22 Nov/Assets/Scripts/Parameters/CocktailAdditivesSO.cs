using UnityEngine;
using System.Collections.Generic;
using FMODUnity;

[CreateAssetMenu (fileName = "NewAdditive", menuName = "ScriptableObject/CocktailAdditive")]
public class CocktailAdditivesSO : ScriptableObject, IRequiredParameters
{
    [SerializeField] EventReference _sound;
    [SerializeField] private string _name;
    [SerializeField] [TextArea] string _description;
    [SerializeField] private List<Parameter> _parameters;


    public EventReference Sound => _sound;
    public string Name => _name;
    public string Description => _description;
    public List<Parameter> Parameters => _parameters;

    public void Setup(List<ParameterSO> parameters)
    {
        _parameters = new List<Parameter>();
        foreach (ParameterSO param in parameters)
            _parameters.Add(new Parameter(param.name, 0));
    }
}
