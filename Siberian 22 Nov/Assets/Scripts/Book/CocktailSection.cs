using UnityEngine;
using Cocktails;
using TMPro;
using System.Collections.Generic;

namespace Book
{
    public class CocktailSection : Section
    {
        public void SetupVariants(List<CocktailCombinationSO> combinations, ParameterValueText[] parametersValueText,
            TextMeshProUGUI discription, Transform content)
        {
            _variants = new List<Variant>();

            foreach (var combination in combinations)
            {
                var variant = Instantiate(_variantPrefab, content).GetComponent<Variant>();
                variant.SetupVariant(combination.Parameters.Name, combination.Parameters.Description,
                    combination.Parameters.Parameters, parametersValueText, discription);
                _variants.Add(variant);
            }

            CloseSection();
        }
    }
}