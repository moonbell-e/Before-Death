using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

namespace GameControllers
{
    public class EndMenu : MonoBehaviour
    {
        private const string EndlessGameLevel = "EndlessLevel";
        private const string MainMenuLevel = "Menu";

        [SerializeField] private GameObject _body;
        [SerializeField] private TextMeshProUGUI _finalScoreText;

        private ScoreCounter _scoreCounter;
        private EndGameTimer _endGameTimer;

        private void Awake()
        {
            _endGameTimer = FindObjectOfType<EndGameTimer>();
            _endGameTimer.TimesOut += DisplayEndGameWindow;
            _body.SetActive(false);
            _scoreCounter = FindObjectOfType<ScoreCounter>();
        }

        private void DisplayEndGameWindow()
        {
            _body.SetActive(true);
            _finalScoreText.text = _scoreCounter.Score.ToString();
            Physics.queriesHitTriggers = false;
            Time.timeScale = 0;
        }

        public void ToMainMenuScene()
        {
            Time.timeScale = 1;
            Physics.queriesHitTriggers = true;
            SceneManager.LoadScene(MainMenuLevel);
        }

        public void ReloadLevel()
        {
            Time.timeScale = 1;
            Physics.queriesHitTriggers = true;
            SceneManager.LoadScene(EndlessGameLevel);
        }

        private void OnDisable()
        {
            _endGameTimer.TimesOut -= DisplayEndGameWindow;
        }
    }
}