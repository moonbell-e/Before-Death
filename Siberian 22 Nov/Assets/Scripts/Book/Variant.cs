using TMPro;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

namespace Book
{
    public class Variant : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private string _description;
        [SerializeField] private TextMeshProUGUI _descriptionTMPro;
        [SerializeField] private List<ParameterValueDisplay> _parametersValueDislays;

        private void Start()
        {
            _button.onClick.AddListener(DisplayVariant);
            foreach(var display in _parametersValueDislays)
                _button.onClick.AddListener(display.DisplayParameters);
        }

        public void SetupVariant(string name, string descriptionText, Parameter[] parameters, 
            ParameterValueText[] parameterValueTexts, TextMeshProUGUI discription)
        {
            GetComponentInChildren<TextMeshProUGUI>().text = name;
            _description = descriptionText;
            _descriptionTMPro = discription;

            SetParametersValueDisplays(parameters, parameterValueTexts);
        }
        
        private void SetParametersValueDisplays(Parameter[] parameters, ParameterValueText[] parameterValueTexts)
        {
            _parametersValueDislays = new List<ParameterValueDisplay>();
            foreach (var parameter in parameters)
            {
                ParameterValueText valueText = null;
                foreach(var parameterValue in parameterValueTexts)
                    if(parameterValue.Name == parameter.Name)
                    {
                        valueText = parameterValue;
                        break;
                    }
                if(valueText == null)
                {
                    Debug.LogError("Error! Отсутвует указанный параметр" + parameter.Name);
                    return;
                }

                var display = new ParameterValueDisplay(parameter.Value, valueText.Text);
                _parametersValueDislays.Add(display);
            }
        }

        public void SetVariantState(bool state)
        {
            gameObject.SetActive(state);
        }

        public void DisplayVariant()
        {
            _descriptionTMPro.text = _description;
        }
    }

    [Serializable]
    public class ParameterValueDisplay
    {
        [SerializeField] private int _value;
        [SerializeField] private TextMeshProUGUI _text;

        public ParameterValueDisplay(int value, TextMeshProUGUI text)
        {
            _value = value;
            _text = text;
        }

        public void DisplayParameters()
        {
            _text.text = _value.ToString();
        }
    }
}