using FMODUnity;
using GameControllers;
using System.Collections;
using UnityEngine;


public class CharacterInfo : MonoBehaviour
{
    [SerializeField] private QuestionTrigger _questionTrigger;
    [SerializeField] private QuestionManager _questionManager;
    [SerializeField] private string _questionText;
    [SerializeField] private Parameter[] _allParamsScore;
    [SerializeField] private CocktailAdditivesSO _alcohol;

    private ScoreCounter _scoreCounter;

    public QuestionTrigger QuestionTrigger => _questionTrigger;
    public string QuestionText => _questionText;


    private void Awake()
    {
        _scoreCounter = FindObjectOfType<ScoreCounter>();
    }

    public void OnEventQuestionTriggered()
    {
        _questionManager.ShowQuestion(_questionText);
    }

    public void SetCharacterInfo(QuestionTrigger questionTrigger, string textQuestion, Parameter[] parameterScore,
        CocktailAdditivesSO alcohol, EventReference leaveSound)
    {
        _questionTrigger = questionTrigger;
        _questionText = textQuestion;
        _allParamsScore = parameterScore;
        _alcohol = alcohol;
        _scoreCounter.SetCharacterParameters(_allParamsScore, _alcohol, leaveSound);
        _questionTrigger.OnEventCharacterTriggered += OnEventQuestionTriggered;
    }

    private void OnDisable()
    {
        _questionTrigger.OnEventCharacterTriggered -= OnEventQuestionTriggered;
    }
}
