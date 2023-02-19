using UnityEngine;
using System;
using FMODUnity;

namespace Cocktails
{
    public class GlassSelector : MonoBehaviour, IResetable
    {
        public event SendEvent GlassChanged;

        [SerializeField] private EventReference _putGlass;
        [SerializeField] private Glass[] _glasses;
        
        private Cocktail _cocktail;
        private GameObject _selectedGlass;
        private MeshRenderer _selectedDrink;

        private void Awake()
        {
            _cocktail = GetComponent<Cocktail>();

            for (int i = 0; i < _glasses.Length; i++)
            {
                var glass = _glasses[i].Selectable;
                var clickable = glass.AddComponent<ClickableObject>();
                clickable.InitializeClick(i, SelectGlass);
            }
            foreach (var glass in _glasses)
            {
                glass.OnTable.SetActive(false);
                glass.Drink.enabled = false;
            }
            Reset();
        }

        public void Reset()
        {
            if(_selectedGlass != null)
            {
                _selectedGlass.SetActive(false);
                _selectedDrink.enabled = false;
            }
            _selectedGlass = null;
            _selectedDrink = null;
        }

        private void SelectGlass(int index)
        {
            var glass = _glasses[index];
            if (_cocktail.SelectGlass(glass.Parameters, glass.Drink))
            {
                if(_selectedGlass != null)
                {
                    GlassChanged?.Invoke();
                    _selectedGlass.SetActive(false);
                    _selectedDrink.enabled = false;
                }
                glass.OnTable.SetActive(true);
                _selectedGlass = glass.OnTable;
                _selectedDrink = glass.Drink;
                RuntimeManager.PlayOneShot(_putGlass);
            }
            else Debug.Log("Опустошите текущую ёмкость!");
        }
    }

    [Serializable]
    public class Glass
    {
        [SerializeField] private CocktailParametersSO _parameters;
        [SerializeField] private GameObject _glassOnTable;
        [SerializeField] private MeshRenderer _drink;
        [SerializeField] private GameObject _selectableGlasses;

        public CocktailParametersSO Parameters => _parameters;
        public GameObject OnTable => _glassOnTable;
        public MeshRenderer Drink => _drink;
        public GameObject Selectable => _selectableGlasses;
    }
}