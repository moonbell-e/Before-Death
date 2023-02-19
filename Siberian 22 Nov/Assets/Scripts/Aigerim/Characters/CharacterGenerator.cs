using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using GameControllers;
using FMODUnity;

public class CharacterGenerator : MonoBehaviour
{
    [SerializeField] private List<Character> _listCharacters;
    [SerializeField] private CharacterInfo _characterInfo;

    [SerializeField] private List<GameObject> _characterGameObjects;

    private void Awake()
    {
        _characterInfo = GetComponent<CharacterInfo>();
    }

    private void Start()
    {
        InstantiateCharacter();
    }

    public void InstantiateCharacter()
    {
        if (_characterGameObjects.Count != 0)
        {
            DestroyPreviousCharacter();
        }

        Character character = _listCharacters[UnityEngine.Random.Range(0, _listCharacters.Count)];

        RuntimeManager.PlayOneShot(character.ComeSound);
        Preset preset = character.GetRandomPreset();
        Object object1 = character.GetRandomObjects();
        Object object2 = character.GetRandomObjects();
        Question question = character.GetRandomQuestion();

        if (preset != null)
        {
            _characterGameObjects.Add(Instantiate(preset.GO));
        }

        if (object1 != object2)
        {
            _characterGameObjects.Add(Instantiate(object1.GO, object1.Point.position, Quaternion.identity));
            _characterGameObjects.Add(Instantiate(object2.GO, object2.Point.position, Quaternion.identity));
        }
        else
        {
            object1 = character.GetRandomObjects(0, 2);
            object2 = character.GetRandomObjects(2, 3);
            _characterGameObjects.Add(Instantiate(object1.GO, object1.Point.position, Quaternion.identity));
            _characterGameObjects.Add(Instantiate(object2.GO, object2.Point.position, Quaternion.identity));
        }

        SetAllInfo(preset, object1, object2, question, character.LeaveSound);
    }

    private void SetAllInfo(Preset preset, Object object1, Object object2, Question question, EventReference leaveSound)
    {
        _characterInfo.SetCharacterInfo(_characterGameObjects[0].GetComponent<QuestionTrigger>(),
         question.Description, CountParameterScore(preset, object1, object2), question.Alcohol, leaveSound);
    }

    private Parameter[] CountParameterScore(Preset preset, Object object1, Object object2)
    {
        Parameter[] result = new Parameter[preset.PresetParameter.Parameters.Length];

        for (int i = 0; i < result.Length; i++)
        {
            result[i] = new Parameter(preset.PresetParameter.Parameters[i].Name, preset.PresetParameter.Parameters[i].Value + object1.ObjectParameter.Parameters[i].Value + object2.ObjectParameter.Parameters[i].Value);
        }

        return result;
    }

    private void DestroyPreviousCharacter()
    {
        foreach (GameObject GO in _characterGameObjects)
        {
            Destroy(GO);
        }
        _characterGameObjects.Clear();
    }
}

[Serializable]
public class Character
{
    [SerializeField] private string _name;
    [SerializeField] private EventReference _come;
    [SerializeField] private EventReference _leave;
    [SerializeField] private List<Preset> _presets;
    [SerializeField] private List<Object> _objects;
    [SerializeField] private List<Question> _questions;

    public EventReference ComeSound => _come;
    public EventReference LeaveSound => _leave;

    public Preset GetRandomPreset()
    {
        if (_presets.Count == 0) return null;

        return _presets[UnityEngine.Random.Range(0, _presets.Count)];
    }

    public Object GetRandomObjects()
    {
        if (_objects.Count == 0) return null;

        return _objects[UnityEngine.Random.Range(0, _objects.Count)];
    }
       
    public Object GetRandomObjects(int index, int indexEnd)
    {
        if (_objects.Count == 0) return null;

        return _objects[UnityEngine.Random.Range(index, indexEnd)];
    }

    public Question GetRandomQuestion()
    {
        if (_questions.Count == 0) return null;

        return _questions[UnityEngine.Random.Range(0, _questions.Count)];
    }
}



