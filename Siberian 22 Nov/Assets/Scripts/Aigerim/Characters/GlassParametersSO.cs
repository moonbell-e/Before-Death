using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace Cocktails
{
    [CreateAssetMenu(fileName = "NewGlass", menuName = "ScriptableObject/Glass")]
    public class GlassParametersSO : ScriptableObject
    {
        [SerializeField] private List<ParameterSO> _parametrs;
    }
}
