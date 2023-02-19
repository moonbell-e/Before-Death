using UnityEngine;
using TMPro;
using System.Collections.Generic;

namespace Book
{
    public class Section : MonoBehaviour
    {
        [SerializeField] protected GameObject _variantPrefab;
        [SerializeField] protected List<Variant> _variants;

        public void CloseSection()
        {
            foreach (var variant in _variants)
                variant.SetVariantState(false);
        }

        public void OpenSection()
        {
            foreach(var variant in _variants)
                variant.SetVariantState(true);
        }

        public void SetupVariants(List<CocktailParametersSO> parameters, ParameterValueText[] parametersValueText,
            TextMeshProUGUI discription, Transform content)
        {
            _variants = new List<Variant>();

            foreach(var parameter in parameters)
            {
                if (parameter == null) continue;
                var variant = Instantiate(_variantPrefab, content).GetComponent<Variant>();
                variant.SetupVariant(parameter.Name, parameter.Description, parameter.Parameters, parametersValueText, discription);
                _variants.Add(variant);
            }

            CloseSection();
        }

        public void SetupVariants(List<CocktailAdditivesSO> parameters, ParameterValueText[] parametersValueText,
            TextMeshProUGUI discription, Transform content)
        {
            _variants = new List<Variant>();

            foreach (var parameter in parameters)
            {
                if (parameter.Name == "Ë¸ä") continue;
                var variant = Instantiate(_variantPrefab, content).GetComponent<Variant>();
                variant.SetupVariant(parameter.Name, parameter.Description, parameter.Parameters.ToArray(), parametersValueText, discription);
                _variants.Add(variant);
            }

            CloseSection();
        }
    }
}