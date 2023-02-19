using UnityEngine;
using TMPro;
using System;
using UnityEditor;
using System.Collections.Generic;
using Cocktails;

namespace Book
{
    public class BookSetuper : MonoBehaviour
    {
        public delegate void OpenedBook();
        public event OpenedBook OnEventOpenedBook;

        [SerializeField] private GameObject _bookBody;
        [SerializeField] private ParameterValueText[] _parametersValueText;
        [SerializeField] private TextMeshProUGUI _discription;
        [SerializeField] private Transform _variantKeeper;
        [SerializeField] private SectionSetup[] _sectionsSetups;
        [SerializeField] private CocktailSectionSetup _cocktailSectionSetup;

        private void Awake()
        {
            CloseBook();
        }

    //#if UNITY_EDITOR
    //    [ContextMenu ("SetupSections")]
    //#endif
    //    private void SetupSections()
    //    {
    //        string[] parametrsGUID = AssetDatabase.FindAssets("t:" + _cocktailSectionSetup.Type, new[] { _cocktailSectionSetup.Path });
    //        List<string> parametersPaths = new List<string>();
    //        foreach (string GUID in parametrsGUID)
    //            parametersPaths.Add(AssetDatabase.GUIDToAssetPath(GUID));
    //        var parameters = new List<CocktailCombinationSO>();
    //        foreach (string path in parametersPaths)
    //            parameters.Add(AssetDatabase.LoadAssetAtPath(path, typeof(CocktailCombinationSO)) as CocktailCombinationSO);

    //        _cocktailSectionSetup.Section.SetupVariants(parameters, _parametersValueText, _discription, _variantKeeper);

    //        foreach(var section in _sectionsSetups)
    //        {
    //            parametrsGUID = AssetDatabase.FindAssets("t:"+section.Type, new[] { section.Path });
    //            parametersPaths = new List<string>();
    //            foreach (string GUID in parametrsGUID)
    //                parametersPaths.Add(AssetDatabase.GUIDToAssetPath(GUID));

    //            if (section.Type == "CocktailParametersSO")
    //            {
    //                var param = new List<CocktailParametersSO>();
    //                foreach (string path in parametersPaths)
    //                    param.Add(AssetDatabase.LoadAssetAtPath(path, typeof(CocktailParametersSO)) as CocktailParametersSO);
    //                section.Section.SetupVariants(param, _parametersValueText, _discription, _variantKeeper);
    //            }
    //            else
    //            {
    //                var param = new List<CocktailAdditivesSO>();
    //                foreach (string path in parametersPaths)
    //                    param.Add(AssetDatabase.LoadAssetAtPath(path, typeof(CocktailAdditivesSO)) as CocktailAdditivesSO);
    //                section.Section.SetupVariants(param, _parametersValueText, _discription, _variantKeeper);
    //            }
    //        }
    //    }

        public void OpenBook()
        {
            _bookBody.SetActive(true);
            Time.timeScale = 0.2f;
            Physics.queriesHitTriggers = false;
            OnEventOpenedBook?.Invoke();
        }

        public void CloseBook()
        {
            _bookBody.SetActive(false);
            Time.timeScale = 1f;
            Physics.queriesHitTriggers = true;
        }
    }

    [Serializable]
    public class ParameterValueText
    {
        [SerializeField] private string _name;
        [SerializeField] private TextMeshProUGUI _valueText;

        public string Name => _name;
        public TextMeshProUGUI Text => _valueText;
    }

    [Serializable]
    public class SectionSetup
    {
        [SerializeField] private Section _section;
        [SerializeField] private string _type;
        [SerializeField] private string _path;

        public Section Section => _section;
        public string Type => _type;
        public string Path => _path;
    }

    [Serializable]
    public class CocktailSectionSetup
    {
        [SerializeField] private CocktailSection _section;
        [SerializeField] private string _type;
        [SerializeField] private string _path;

        public CocktailSection Section => _section;
        public string Type => _type;
        public string Path => _path;
    }
}