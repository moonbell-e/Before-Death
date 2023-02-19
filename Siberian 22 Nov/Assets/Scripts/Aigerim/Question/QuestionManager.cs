using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestionManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _showingText;
    [SerializeField] private GameObject _questionPanel;
    
    public void ShowQuestion(string questionText)
    {
        _questionPanel.SetActive(true);
        _showingText.text = questionText;
    }
    
}
