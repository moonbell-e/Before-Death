using UnityEngine;
using TMPro;
using FMODUnity;

namespace GameControllers
{
    public delegate void SendInt(int value);

    public class ScoreCounter : MonoBehaviour
    {
        public event SendInt ScoreChanged;
        public event SendEvent ScoreCounted;

        [SerializeField] private bool _endlessLevel;
        [SerializeField] private TextMeshProUGUI _scoreText;
        [SerializeField] private TextMeshProUGUI _warningText;

        private Parameter[] _characterParameters;
        private CocktailAdditivesSO _characterAlcohol;
        private float _score;
        private EventReference _curLeaveSound;

        public int Score => Mathf.RoundToInt(_score);


        private void Awake()
        {
            _score = 0;
        }

        public void SetCharacterParameters(Parameter[] parameters, CocktailAdditivesSO alcohol, EventReference leaveSound)
        {
            _characterParameters = parameters;
            _characterAlcohol = alcohol;
            _curLeaveSound = leaveSound;
        }

        public void SetCharacterParameters(Parameter[] parameters, CocktailAdditivesSO alcohol)
        {
            _characterParameters = parameters;
            _characterAlcohol = alcohol;
        }

        public void CountScore(Parameter[] parameters, CocktailAdditivesSO alcohol)
        {
            int result = 0;
            for (int i = 0; i < _characterParameters.Length; i++)
            {
                if (_characterParameters[i].Value <= parameters[i].Value)
                    result += _characterParameters[i].Value - (parameters[i].Value - _characterParameters[i].Value);
                else
                    result += parameters[i].Value - (_characterParameters[i].Value - parameters[i].Value);
                Debug.Log(parameters[i].Name + " " + _characterParameters[i].Value + " " + parameters[i].Value + " = " + result);
            }

            if (_characterAlcohol == alcohol)
            {
                if (result > 0)
                    result *= 2;
                else
                    result = 0;
            }

            if(_endlessLevel) 
                ScoreChanged?.Invoke(result * 4);
            else
                _score += result * 10;

            if (_score < 0) 
                _score = 0;
            Debug.Log(_score + " Алкоголь: " + (_characterAlcohol == alcohol).ToString());
            _scoreText.text = _score.ToString();
            if (_curLeaveSound.IsNull == false)
            {
                Debug.Log("Sound");
                RuntimeManager.PlayOneShot(_curLeaveSound);
            }
            ScoreCounted?.Invoke();
        }

        public void ChangeScore(int value)
        {
            if (_endlessLevel)
            {
                ScoreChanged?.Invoke(value);
                return;
            }

            if (_score > 0) _warningText.text = "Штраф за растрату алкоголя! " + value;
            _score += value;
            if (_score < 0) _score = 0;
            _scoreText.text = _score.ToString();
        }

        private void Update()
        {
            if (_endlessLevel == false) return;

            _score += Time.deltaTime;
            _scoreText.text = Mathf.RoundToInt(_score).ToString();
        }
    }
}