using System;
using UnityEngine;


public class CharacterContainer : MonoBehaviour
{

}

[Serializable]
public class Preset
{
    [SerializeField] private GameObject _preset;
    [SerializeField] private PresetsParametersSO _parameter;

    public GameObject GO => _preset;
    public PresetsParametersSO PresetParameter => _parameter;
}

[Serializable]
public class Object
{
    [SerializeField] private GameObject _object;
    [SerializeField] private ObjectParametersSO _parameter;
    [SerializeField] private Transform _point;

    public GameObject GO => _object;

    public ObjectParametersSO ObjectParameter => _parameter;
    public Transform Point => _point;
}

[Serializable]
public class Question
{
    [SerializeField][TextArea(5, 10)] private string _question;
    [SerializeField] private CocktailAdditivesSO _alcohol;

    public string Description => _question;
    public CocktailAdditivesSO Alcohol => _alcohol;
}

