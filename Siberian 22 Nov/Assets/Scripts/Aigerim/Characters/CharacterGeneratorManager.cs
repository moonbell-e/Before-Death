using GameControllers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterGeneratorManager : MonoBehaviour
{
    [SerializeField] private CharacterGenerator _characterGenerator;
    [SerializeField] private ScoreCounter _scoreCounter;

    private void Awake()
    {
        _characterGenerator = GetComponent<CharacterGenerator>();
    }

    private void Start()
    {
        _scoreCounter.ScoreCounted += _characterGenerator.InstantiateCharacter;
    }
    

    private void OnDisable()
    {
        _scoreCounter.ScoreCounted -= _characterGenerator.InstantiateCharacter;
    }
}
