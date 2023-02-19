using UnityEngine;
using System.Linq;
using GameControllers;
using TMPro;

public delegate void SendEvent();

namespace Cocktails
{
    public class Sink : MonoBehaviour
    {
        [SerializeField] private int _sinkPenalty;

        private event SendEvent _restart;
        private Cocktail _cocktail;
        private Shaker _shaker;
        private ScoreCounter _scoreCounter;
        [SerializeField] private TextMeshProUGUI _warningText;

        private void Awake()
        {
            _cocktail = FindObjectOfType<Cocktail>();
            _shaker = FindObjectOfType<Shaker>();
            _scoreCounter = FindObjectOfType<ScoreCounter>();
            _scoreCounter.ScoreCounted += ResetTable;
            var resetableObjects = FindObjectsOfType<MonoBehaviour>().OfType<IResetable>().ToArray();
            foreach (var resetable in resetableObjects)
                _restart += resetable.Reset;
        }
        
        private void OnMouseDown()
        {
            if (_cocktail.CurAlcohol != null || _shaker.SelectedAlcohol != null)
            {
                Debug.Log("Штраф за растрату алкоголя!" + _sinkPenalty);
                _scoreCounter.ChangeScore(_sinkPenalty);
            }
                
            ResetTable();
        }

        private void ResetTable()
        {
            _restart?.Invoke();
        }

        private void OnDisable()
        {
            _scoreCounter.ScoreCounted -= ResetTable;
            var resetableObjects = FindObjectsOfType<MonoBehaviour>().OfType<IResetable>().ToArray();
            foreach (var resetable in resetableObjects)
                _restart -= resetable.Reset;
        }
    }
    
    public interface IResetable
    {
        void Reset();
    }
}