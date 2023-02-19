using UnityEngine;
using GameControllers;
using FMODUnity;
using TMPro;
using System.Collections.Generic;
using UnityEditor;
using System;

namespace Cocktails
{
    public class Cocktail : MonoBehaviour, IResetable
    {
        public event Action<CocktailCombinationSO, CocktailAdditivesSO, ParameterInCocktail[]> OnPouredCocktail;
        [SerializeField] private EventReference _cocktailFinish;
        [SerializeField] private ParameterInCocktail[] _parameterInCocktails;
        [SerializeField] private TextMeshProUGUI _warningText;
        [SerializeField] private Color _failCocktailMaterial;

        private ScoreCounter _scoreCounter;
        private CocktailParametersSO _curGlassParameters;
        private MeshRenderer _curDrinkMesh;
        private CocktailCombinationSO _curCocktailCombination;
        private CocktailAdditivesSO _curAlcohol;
        private CocktailParametersSO _curDecoration;

        public CocktailAdditivesSO CurAlcohol => _curAlcohol;

        private void Awake()
        {
            _scoreCounter = FindObjectOfType<ScoreCounter>();
            Reset();
        }

        //#if UNITY_EDITOR
        //        [ContextMenu("Find Parameters")]
        //#endif
        //private void FindParameters()
        //{
        //    string[] parametrsGUID = AssetDatabase.FindAssets("t:ParameterSO", new[] { "Assets/ScriptableObjects/Parameters" });
        //    List<string> parametersPaths = new List<string>();
        //    foreach (string GUID in parametrsGUID)
        //        parametersPaths.Add(AssetDatabase.GUIDToAssetPath(GUID));
        //    var parameters = new List<ParameterSO>();
        //    foreach (string path in parametersPaths)
        //        parameters.Add(AssetDatabase.LoadAssetAtPath(path, typeof(ParameterSO)) as ParameterSO);

        //    _parameterInCocktails = new ParameterInCocktail[parameters.Count];
        //    for (int i = 0; i < _parameterInCocktails.Length; i++)
        //        _parameterInCocktails[i] = new ParameterInCocktail(parameters[i].name, 0);
        //}

        public void ChangeCocktailParameters(Parameter[] parameters, bool increaseValue)
        {
            if (_curCocktailCombination == null && _curAlcohol != null) return;
            ParameterInCocktail par;
            for (int i = 0; i < parameters.Length; i++)
            {
                par = _parameterInCocktails[i];
                if (increaseValue)
                    par.ChangeParameterValue(par.Parameter.Value + parameters[i].Value);
                else
                    par.ChangeParameterValue(par.Parameter.Value - parameters[i].Value);
            }
        }

        public void Reset()
        {
            if (_curDrinkMesh != null) _curDrinkMesh.enabled = false;
            _curDrinkMesh = null;
            _curAlcohol = null;
            _curCocktailCombination = null;
            _curGlassParameters = null;
            _curDecoration = null;

            for (int i = 0; i < _parameterInCocktails.Length; i++)
                _parameterInCocktails[i].ChangeParameterValue(0);
        }

        public bool SelectGlass(CocktailParametersSO glassParameters, MeshRenderer drinkMesh)
        {
            if (_curAlcohol == null)
            {
                _warningText.text = "Выбран стакан: " + glassParameters.Name;
                if(_curGlassParameters != null) ChangeCocktailParameters(_curGlassParameters.Parameters, false);
                _curGlassParameters = glassParameters;
                _curDrinkMesh = drinkMesh;
                ChangeCocktailParameters(_curGlassParameters.Parameters, true);
                return true;
            }
            return false;
        }

        public bool PourCocktail(CocktailCombinationSO combination, CocktailAdditivesSO alcohol)
        {
            if (_curGlassParameters == null) return false;
            if (combination == null)
            {
                _curDrinkMesh.material.color = _failCocktailMaterial;
                for (int i = 0; i < _parameterInCocktails.Length; i++)
                    _parameterInCocktails[i].ChangeParameterValue(0);

                _warningText.text = "Какая гадость!";
                Debug.Log("Какая гадость!");
            }
            else
            {
                _curDrinkMesh.material.color = combination.Color;
                ChangeCocktailParameters(combination.Parameters.Parameters, true);
                
                _warningText.text = "Коктейль: " + combination.Parameters.Name;
                Debug.Log(combination.name + " " + alcohol.name);
            }
            _curDrinkMesh.enabled = true;
            _curCocktailCombination = combination;
            _curAlcohol = alcohol;
            ChangeCocktailParameters(_curAlcohol.Parameters.ToArray(), true);
            OnPouredCocktail?.Invoke(combination, alcohol, _parameterInCocktails);
            return true;
        }

        public void SetDecoration(CocktailParametersSO decoration)
        {
            if (_curDecoration != null) ChangeCocktailParameters(_curDecoration.Parameters, false);
            _curDecoration = decoration;
            if (decoration == null) return;
            ChangeCocktailParameters(_curDecoration.Parameters, true);
        }

        public bool GiveCocktail()
        {
            if (_curAlcohol == null)
            {
                _warningText.text = "Вы не налили коктейль!";
                Debug.Log("Вы не налили коктейль!");
                return false;
            }

            Parameter[] result = new Parameter[_parameterInCocktails.Length];
            for (int i = 0; i < result.Length; i++)
                result[i] = new Parameter(_parameterInCocktails[i].Parameter.Name, _parameterInCocktails[i].Parameter.Value);

            //for (int i = 0; i < result.Length; i++)
            //{
            //    if (_curCocktailCombination == null)
            //        result[i] = new Parameter(_curGlassParameters.Parameters[i].Name, 0);
            //    else if (_curDecoration == null)
            //        result[i] = new Parameter(_curGlassParameters.Parameters[i].Name, _curGlassParameters.Parameters[i].Value +
            //            _curCocktailCombination.Parameters.Parameters[i].Value + _curAlcohol.Parameters[i].Value);
            //    else
            //        result[i] = new Parameter(_curGlassParameters.Parameters[i].Name, _curDecoration.Parameters[i].Value +
            //            _curCocktailCombination.Parameters.Parameters[i].Value + _curGlassParameters.Parameters[i].Value
            //            + _curAlcohol.Parameters[i].Value);
            //}

            _scoreCounter.CountScore(result, _curAlcohol);
            RuntimeManager.PlayOneShot(_cocktailFinish);
            Reset();
            return true;
        }
    }

    [Serializable]
    public class ParameterInCocktail
    {
        [SerializeField] private TextMeshProUGUI _valueText;
        [SerializeField] private Parameter _parameter;

        public ParameterInCocktail(string name, int value)
        {
            _parameter = new Parameter(name, value);
        }

        public void ShowParameter()
        {
            _valueText.text = _parameter.Value.ToString();
        }

        public void ChangeParameterValue(int value)
        {
            _parameter = new Parameter(Parameter.Name, value);
            ShowParameter();
        }

        public Parameter Parameter => _parameter;
        public TextMeshProUGUI ValueText => _valueText;
    }
}