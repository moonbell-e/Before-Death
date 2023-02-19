using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Book;
using FMODUnity;
using TMPro;
using Cocktails;
using GameControllers;
using System;

public class TutorialManager : MonoBehaviour, IResetable
{
    [TextArea(7, 15)]
    [SerializeField] private List<string> _tutorialText;
    [SerializeField] private TextMeshProUGUI _dialogText;
    [SerializeField] private EventReference _succesSound;
    [SerializeField] private BookSetuper _book;
    [SerializeField] private Cocktail _cocktail;
    [SerializeField] private ResultCocktail[] _resultCocktails;
    [SerializeField] private ScoreCounter _scoreCounter;
    [SerializeField] private List<Parameter> _parameters;
    [SerializeField] private CocktailAdditivesSO _cocktailAdditivesSO;
    [SerializeField] private Sink _sink;
    [SerializeField] private GameObject _pointer;
    [SerializeField] private GameObject _menuButton;

    private void Start()
    {
        _scoreCounter.SetCharacterParameters(_parameters.ToArray(), _cocktailAdditivesSO);
    }

    private void OnEnable()
    {
        _book.OnEventOpenedBook += ShowFirstSentence;
        _cocktail.OnPouredCocktail += ShowSecondSentence;
        _cocktail.OnPouredCocktail += ShowFifthSentence;
    }


    private void ShowFirstSentence()
    {
        _dialogText.text = _tutorialText[0];
        _book.OnEventOpenedBook -= ShowFirstSentence;
        RuntimeManager.PlayOneShot(_succesSound);
    }

    private void ShowSecondSentence(CocktailCombinationSO cocktail, CocktailAdditivesSO alcohol, ParameterInCocktail[] parameters)
    {
        if (_resultCocktails[0].ReadyCocktail == cocktail && _dialogText.text == _tutorialText[0])
        {
            _dialogText.text = _tutorialText[1];
            RuntimeManager.PlayOneShot(_succesSound);
        }
    }

    private void ShowThirdSentence()
    {
        if (_dialogText.text == _tutorialText[1])
        {
            _dialogText.text = _tutorialText[2];
            RuntimeManager.PlayOneShot(_succesSound);
        }
    }

    private void OnDisable()
    {
        _book.OnEventOpenedBook -= ShowFirstSentence;
        _cocktail.OnPouredCocktail -= ShowSecondSentence;
        _cocktail.OnPouredCocktail -= ShowFifthSentence;
    }

    public void Reset()
    {
        ShowThirdSentence();
        RuntimeManager.PlayOneShot(_succesSound);
        _pointer.SetActive(true);
    }


    public void ShowFourthSentence()
    {
        if (_dialogText.text == _tutorialText[2])
        {
            _dialogText.text = _tutorialText[3];
            RuntimeManager.PlayOneShot(_succesSound);
        }
    }

    private void ShowFifthSentence(CocktailCombinationSO cocktail, CocktailAdditivesSO alcohol, ParameterInCocktail[] parameters)
    {
        if (_dialogText.text == _tutorialText[3] && alcohol == _cocktailAdditivesSO && parameters[0].Parameter.Value == _parameters[0].Value
            && parameters[1].Parameter.Value == _parameters[1].Value && parameters[2].Parameter.Value == _parameters[2].Value
            && parameters[3].Parameter.Value == _parameters[3].Value)
        {

            _dialogText.text = _tutorialText[4];
            _menuButton.SetActive(true);
            RuntimeManager.PlayOneShot(_succesSound);
        }
    }

    [Serializable]
    public class ResultCocktail
    {
        [SerializeField] private CocktailCombinationSO _readyCocktail;
        public CocktailCombinationSO ReadyCocktail => _readyCocktail;

        [SerializeField] private CocktailParametersSO _glassSelected;
        public CocktailParametersSO GlassSelected => _glassSelected;
    }



}
