using UnityEngine;
using System;
using FMODUnity;

namespace Cocktails
{
    public delegate void SendIngridient(CocktailParametersSO Parameter, GameObject Prefab, EventReference Sound, GameObject Particle);

    public class IngridientSelector : MonoBehaviour
    {
        public event SendIngridient IngridientSelected;

        [SerializeField] private Ingridient[] _ingridients;

        private void Awake()
        {
            for (int i = 0; i < _ingridients.Length; i++)
            {
                var clickable = _ingridients[i].IngridientOnTable.AddComponent<ClickableObject>();
                clickable.InitializeClick(i, SelectIngridient);
                _ingridients[i].ParticleSystem.SetActive(false);
            }
        }

        private void SelectIngridient(int index)
        {
            var ingr = _ingridients[index];
            IngridientSelected?.Invoke(ingr.Parameters, ingr.Prefab, ingr.Sound, ingr.ParticleSystem);
        }
    }

    [Serializable]
    public class Ingridient
    {
        [SerializeField] private EventReference _sound;
        [SerializeField] private GameObject _particleSystem;
        [SerializeField] private GameObject _prefab;
        [SerializeField] private CocktailParametersSO _parameters;
        [SerializeField] private GameObject _ingridientOnTable;

        public EventReference Sound => _sound;
        public GameObject ParticleSystem => _particleSystem;
        public GameObject Prefab => _prefab;
        public CocktailParametersSO Parameters => _parameters;
        public GameObject IngridientOnTable => _ingridientOnTable;
    }
}