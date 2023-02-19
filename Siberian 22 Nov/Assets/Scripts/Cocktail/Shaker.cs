using UnityEngine;
using System.Collections.Generic;
using System;
using FMODUnity;
using TMPro;

namespace Cocktails
{
    public class Shaker : MonoBehaviour, IResetable
    {
        public event SendEvent ShakerClicked;

        [SerializeField] private EventReference _shakeSound;
        [SerializeField] private EventReference _icePutSound;
        [SerializeField] private EventReference _icePutInWaterSound;
        [SerializeField] private Additive[] _additiveOnTable;
        [SerializeField] private Additive _ice;
        [SerializeField] private TextMeshProUGUI _warningText;
        [SerializeField] private ParticleSystem[] _alcoholVFX;

        private Animator _animator;
        private CocktailCombinator _combinator;
        private CocktailAdditivesSO _selectedAlcohol;
        private List<CocktailParametersSO> _selectedIngridients;
        private bool _iceAdded;

        public CocktailAdditivesSO SelectedAlcohol => _selectedAlcohol;

        private void Awake()
        {
            _combinator = FindObjectOfType<CocktailCombinator>();
            _animator = GetComponent<Animator>();
            for (int i = 0; i < _additiveOnTable.Length; i++)
            {
                var clickable = _additiveOnTable[i].SelectedAdditive.AddComponent<ClickableObject>();
                clickable.InitializeClick(i, SelectAdditive);
            }
            var ice = _ice.SelectedAdditive.AddComponent<ClickableObject>();
            ice.InitializeClick(-1, SelectAdditive);
            Reset();
        }

        public void Reset()
        {
            _selectedAlcohol = null;
            _selectedIngridients = new List<CocktailParametersSO>();
            _iceAdded = false;
        }

        public void Shake()
        {
            if (_selectedAlcohol == null)
            {
                _warningText.text = "Добавьте алкоголь!";
                Debug.Log("Добавьте алкоголь!");
                return;
            }
            if (_iceAdded == false && _selectedIngridients.Count <= 0)
            {
                _warningText.text = "Не хватает ингредиентов!";
                Debug.Log("Не хватает ингрeдиентов!");
                return;
            }

            if (_combinator.MixCoctail(_iceAdded, _selectedIngridients, _selectedAlcohol) == false) return;
            RuntimeManager.PlayOneShot(_shakeSound);
            ShowAlcoholVFX(_selectedAlcohol);
            _animator.SetTrigger("Play");
            Reset();
        }

        private void SelectAdditive(int index)
        {
            if (index == -1)
            {
                if (_iceAdded == false)
                {
                    _warningText.text = "Добавлен лёд";
                    if (_selectedAlcohol == null)
                        RuntimeManager.PlayOneShot(_icePutSound);
                    else
                        RuntimeManager.PlayOneShot(_icePutInWaterSound);
                    _ice.Animator.SetTrigger("Play");
                    _iceAdded = true;
                    return;
                }
                else
                {
                    _warningText.text = "Вы уже добавили лёд!";
                    return;
                }
            }

            if (_selectedAlcohol != null)
            {
                _warningText.text = "Вы уже налили алкоголь!";
                return;
            }
            _selectedAlcohol = _additiveOnTable[index].Parameters;
            RuntimeManager.PlayOneShot(_additiveOnTable[index].Sound);
            if(_additiveOnTable[index].Animator != null)
                _additiveOnTable[index].Animator.SetTrigger("Play");
            _warningText.text = "Налили алкоголь: " + _selectedAlcohol.Name;
        }

        public void AddIngridient(CocktailParametersSO parameters)
        {
            _warningText.text = "Смешали в шейкере";
            _selectedIngridients.Add(parameters);
        }

        private void ShowAlcoholVFX(CocktailAdditivesSO alcohol)
        {
            switch (alcohol.Name)
            {
                case "Джин":
                    {
                        _alcoholVFX[0].Play();
                        _alcoholVFX[1].Play();
                        break;
                    }
                case "Киски":
                    {
                        _alcoholVFX[2].Play();
                        break;
                    }
                case "Зелёная ведьма":
                    {
                        _alcoholVFX[3].Play();
                        break;
                    }
                case "Айсберг":
                    {
                        _alcoholVFX[4].Play();
                        break;
                    }
            }
        }

        private void OnMouseDown()
        {
            ShakerClicked?.Invoke();
        }
    }

    [Serializable]
    public class Additive
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private EventReference _sound;
        [SerializeField] private CocktailAdditivesSO _parameters;
        [SerializeField] private GameObject _selectedAdditive;

        public Animator Animator => _animator;
        public EventReference Sound => _sound;
        public CocktailAdditivesSO Parameters => _parameters;
        public GameObject SelectedAdditive => _selectedAdditive;
    }
}