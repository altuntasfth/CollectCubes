using System;
using _Game.Scripts.Level;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace _Game.Scripts
{
    public class UIManager : MonoBehaviour
    {
        public LevelManager levelManager;
        public PlayerMechanic player;
        public float levelProgressRatio;
        private GameManager gameManager;
        private bool isInitialized;
        public bool isSettingUIEnabled;
        private float timer;
        
        [Space(10)]
        [Header("Texts")]
        [SerializeField] private TextMeshProUGUI progressBarLevelStartText;
        [SerializeField] private TextMeshProUGUI progressBarLevelEndText;
        public TextMeshProUGUI gameplayMoneyBarText; 
        public TextMeshProUGUI aiMoneyBarText; 
        [SerializeField] private TextMeshProUGUI timerText; 
        public TextMeshProUGUI levelCompleteText;
        
        [Space(10)]
        [Header("Sprites")]
        [SerializeField] private Sprite vibrationOnImage;
        [SerializeField] private Sprite vibrationOffImage;
        [SerializeField] private Sprite vibrationButtonOnImage;
        [SerializeField] private Sprite vibrationButtonOffImage;
        [SerializeField] private bool vibrationState;
        
        [Space(10)]
        [Header("Images")]
        [SerializeField] private Image vibrationImage;
        [SerializeField] private Image vibrationButtonImage;
        public Image progressBarFillImage;
        
        [Space(10)]
        [Header("Buttons")]
        [SerializeField] private Button resetButton;
        [SerializeField] private Button nextLevelButton;
        [SerializeField] private Button settingButton;
        [SerializeField] private Button vibrationButton;
        
        [Space(10)]
        [Header("UIs")]
        [SerializeField] private GameObject settingUI;
        [SerializeField] private GameObject timerUI;
        [SerializeField] private GameObject standardGameModeUI;
        [SerializeField] private GameObject timerGameModeUI;
        [SerializeField] private GameObject aiGameModeUI;
        [SerializeField] private GameObject standardTutorialUI;
        [SerializeField] private GameObject timerTutorialUI;
        [SerializeField] private GameObject aiTutorialUI;
        [SerializeField] private GameObject settingBackgroundUI;
        [SerializeField] private GameObject moneyBarUI;
        public CanvasGroup levelCompletedScreenUI;
        public GameObject levelCompletedConfetti;

        public int totalMoneyAmount;

        public void Initialize()
        {
            gameManager = FindObjectOfType<GameManager>();
            
            resetButton.gameObject.SetActive(false);
            settingUI.SetActive(false);
            nextLevelButton.gameObject.SetActive(false);
            
            totalMoneyAmount = PlayerPrefs.GetInt("TotalMoneyAmount", 0);
            gameplayMoneyBarText.text = totalMoneyAmount.ToString();
            
            settingButton.transform.DOScale(Vector3.one, 1f).OnComplete(() =>
            {
                settingButton.enabled = true;
            });
            
            switch (gameManager.gameMode)
            {
                case GameManager.GameMode.STANDARD:
                    InitializeStandardGameMode();
                    break;
                case GameManager.GameMode.TIME:
                    InitializeTimeGameMode();
                    break;
                case GameManager.GameMode.AI:
                    InitializeAIGameMode();
                    break;
                case GameManager.GameMode.AIObstacle:
                    InitializeAIGameMode();
                    break;
            }

            timer = levelManager.timer;

            isInitialized = true;
        }

        private void Update()
        {
            if (!isInitialized || !gameManager.isGameStarted || gameManager.isGameOver || levelManager == null || player == null)
            {
                return;
            }

            switch (gameManager.gameMode)
            {
                case GameManager.GameMode.STANDARD:
                    UpdateStandardGameMode();
                    break;
                case GameManager.GameMode.TIME:
                    UpdateTimeGameMode();
                    break;
                case GameManager.GameMode.AI:
                    UpdateAIGameMode();
                    break;
                case GameManager.GameMode.AIObstacle:
                    UpdateAIGameMode();
                    break;
            }
        }
        
        public void StartGame()
        {
            resetButton.gameObject.SetActive(true);
            
            standardTutorialUI.SetActive(false);
            timerTutorialUI.SetActive(false);
            aiTutorialUI.SetActive(false);
            
            DOVirtual.DelayedCall(2f, () =>
            {
                nextLevelButton.enabled = false;
                nextLevelButton.gameObject.SetActive(true);
                nextLevelButton.transform.DOPunchRotation(new Vector3(30, 0, 15), 1f, 5, 50).OnComplete(() =>
                {
                    nextLevelButton.enabled = true;
                });
            });

            settingButton.enabled = false;
            settingButton.transform.DOScale(Vector3.zero, 1f);

            if (gameManager.gameMode == GameManager.GameMode.TIME)
            {
                levelManager.StartToSpawnVoxels(1);
            }
            else if (gameManager.gameMode == GameManager.GameMode.AI || gameManager.gameMode == GameManager.GameMode.AIObstacle)
            {
                levelManager.StartToSpawnVoxels(3);
            }
        }

        #region INITIALIZEGAMEMODE

        private void InitializeStandardGameMode()
        {
            timerUI.SetActive(false);
            
            progressBarLevelStartText.text = (gameManager.normalizedLevelIndex).ToString();
            progressBarLevelEndText.text = (gameManager.normalizedLevelIndex + 1).ToString();
            progressBarFillImage.fillAmount = 0f;
            levelProgressRatio = 0f;
            
            standardGameModeUI.SetActive(true);
        }

        private void InitializeTimeGameMode()
        {
            gameplayMoneyBarText.text = "0";
            timerUI.SetActive(true);
            
            float milliSeconds = (timer % 1) * 100;
            timerText.text = string.Format("{0:00}:{1:00}", timer, milliSeconds);
            
            timerGameModeUI.SetActive(true);
        }

        private void InitializeAIGameMode()
        {
            gameplayMoneyBarText.text = "0";
            aiMoneyBarText.text = "0";
            moneyBarUI.GetComponent<RectTransform>().localPosition = new Vector3(-106, -220, 0);
            timerUI.SetActive(true);
            
            float milliSeconds = (timer % 1) * 100;
            timerText.text = string.Format("{0:00}:{1:00}", timer, milliSeconds);
            
            aiGameModeUI.SetActive(true);
        }

        #endregion

        #region UPDATEGAMEMODE

        private void UpdateStandardGameMode()
        {
            progressBarFillImage.fillAmount = Mathf.Lerp(progressBarFillImage.fillAmount, (float)player.collectedVoxelCount / (float)levelManager.voxelCount, Time.smoothDeltaTime * 5f);
        }

        private void UpdateTimeGameMode()
        {
            TimeCounter();

            if (timer <= 0.001f)
            {
                timerText.text = "00:00";
                gameManager.HandleLevelComplete();
            }
        }

        private void UpdateAIGameMode()
        {
            TimeCounter();
            
            if (timer <= 0.001f)
            {
                timerText.text = "00:00";
                gameManager.HandleLevelComplete();
            }
        }

        private void TimeCounter()
        {
            float milliSeconds = (timer % 1) * 100;
            timerText.text = string.Format("{0:00}:{1:00}", timer, milliSeconds);
                
            timer -= Time.deltaTime;
            if (timer <= 0.01f)
            {
                timer = 0f;
            }
        }

        #endregion

        #region BUTTONACTIONS

        private void ClearAndReload()
        {
            DOTween.KillAll();
            DOTween.Clear();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void StandardGameModeButton()
        {
            PlayerPrefs.SetInt("LevelModeIndex", 0);
            PlayerPrefs.Save();
            
            ClearAndReload();
        }
        
        public void TimerGameModeButton()
        {
            PlayerPrefs.SetInt("LevelModeIndex", 1);
            PlayerPrefs.Save();
            
            ClearAndReload();
        }
        
        public void AIGameModeButton()
        {
            PlayerPrefs.SetInt("LevelModeIndex", 2);
            PlayerPrefs.Save();
            
            ClearAndReload();
        }
        
        public void AIWithObstacleGameModeButton()
        {
            PlayerPrefs.SetInt("LevelModeIndex", 3);
            PlayerPrefs.Save();
            
            ClearAndReload();
        }
        
        public void ClearPlayerPrefsButton()
        {
            PlayerPrefs.DeleteAll();
            
            ClearAndReload();
        }

        public void SettingButton()
        {
            isSettingUIEnabled = true;
            settingUI.SetActive(true);
            settingUI.GetComponent<Image>().DOFade(0.6f, 1f);
            settingBackgroundUI.transform.DOScale(Vector3.one, 1f);
        }

        public void CancelButton()
        {
            settingUI.GetComponent<Image>().DOFade(0f, 1f);
            settingBackgroundUI.transform.DOScale(Vector3.zero, 1f).OnComplete(() =>
            {
                settingUI.SetActive(false);
                isSettingUIEnabled = false;
            });
        }

        public void VibrationButton()
        {
            if (vibrationState == false)
            {
                vibrationImage.sprite = vibrationOnImage;
                vibrationButtonImage.sprite = vibrationButtonOnImage;
                vibrationState = true;
                return;
            }

            if (vibrationState == true)
            {
                vibrationImage.sprite = vibrationOffImage;
                vibrationButtonImage.sprite = vibrationButtonOffImage;
                vibrationState = false;
            }
        }

        public void ResetButton()
        {
            DOTween.KillAll();
            DOTween.Clear();
            
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void NextLevelButton()
        {
            PlayerPrefs.SetInt("LevelModeIndex", 0);
            PlayerPrefs.Save();
            
            nextLevelButton.enabled = false;
            gameManager.HandleLevelComplete();
        }

        #endregion
    }
}