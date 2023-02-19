using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using FMODUnity;
using TMPro;

namespace Cocktails
{
    public class CocktailCombinator : MonoBehaviour
    {
        [SerializeField] private EventReference _failCocktailSound;
        [SerializeField] private List<CocktailCombinationSO> _combinations;
        [SerializeField] private TextMeshProUGUI _warningText;
        private Cocktail _cocktail;

        private void Awake()
        {
            _cocktail = FindObjectOfType<Cocktail>();
        }

//#if UNITY_EDITOR
//        [ContextMenu("Find Combinations")]
//#endif
        //private void FindCombinations()
        //{
        //    string[] combinationsGUID = AssetDatabase.FindAssets("t:CocktailCombinationSO", new[] { "Assets/ScriptableObjects/CocktailCombinations" });
        //    List<string> combinationsPaths = new List<string>();
        //    foreach (string GUID in combinationsGUID)
        //        combinationsPaths.Add(AssetDatabase.GUIDToAssetPath(GUID));
        //    _combinations = new List<CocktailCombinationSO>();
        //    foreach (string path in combinationsPaths)
        //        _combinations.Add(AssetDatabase.LoadAssetAtPath(path, typeof(CocktailCombinationSO)) as CocktailCombinationSO);
        //}

        public bool MixCoctail(bool ice, List<CocktailParametersSO> ingridients, CocktailAdditivesSO alcohol)
        {
            bool res;
            CocktailCombinationSO result = null;
            if (_cocktail.CurAlcohol != null)
            {
                _warningText.text = "Коктейль уже налит!";
                Debug.Log("Коктейль уже налит!");
                return false;
            }
            if (ingridients.Count <= 0)
            {
                res = _cocktail.PourCocktail(result, alcohol);
                if (res == false)
                    _warningText.text = "Выберите ёмкость для коктейля!";
                return res;
            }
            
            Debug.Log(ice);
            foreach (CocktailParametersSO ingr in ingridients)
                Debug.Log(ingr.name);
            
            List<CocktailParametersSO> ingridientsCheck = new List<CocktailParametersSO>();
            int ingridientsCount = 0;
            foreach (CocktailCombinationSO combination in _combinations)
            {
                if (combination.Ice != ice) continue;

                ingridientsCheck = new List<CocktailParametersSO>();
                foreach (CocktailParametersSO ingridient in ingridients)
                        ingridientsCheck.Add(ingridient);

                ingridientsCount = 0;
                foreach (CocktailParametersSO ingridient in combination.Ingridients)
                    if (ingridientsCheck.Contains(ingridient))
                    {
                        ingridientsCount++;
                        ingridientsCheck.Remove(ingridient);
                    }

                if (combination.Ingridients.Length == ingridientsCount)
                {
                    if (ingridientsCheck.Count <= combination.AnyIngridientCount)
                    {
                        result = combination;
                        break;
                    }
                }
            }
            res = _cocktail.PourCocktail(result, alcohol);
            if (res == false)
                _warningText.text = "Выберите ёмкость для коктейля!";
            if (result == null && res != false)
                RuntimeManager.PlayOneShot(_failCocktailSound);
            else
                RuntimeManager.PlayOneShot(alcohol.Sound);
            return res;
        }
    }
}