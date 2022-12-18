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
        private float timer = 10f;
        
        [Space(10)]
        [Header("Texts")]
        [SerializeField] private TextMeshProUGUI progressBarLevelStartText;
        [SerializeField] private TextMeshProUGUI progressBarLevelEndText;
        [SerializeField] private TextMeshProUGUI tutorialText; 
        public TextMeshProUGUI gameplayMoneyBarText; 
        [SerializeField] private TextMeshProUGUI bonusLevelMoneyBarText; 
        [SerializeField] private TextMeshProUGUI bonusLevelTimerText; 
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
        [SerializeField] private GameObject bonusLevelUI;
        [SerializeField] private GameObject gameplayUI;
        [SerializeField] private GameObject settingBackgroundUI;
        public CanvasGroup levelCompletedScreenUI;
        public GameObject levelCompletedConfetti;

        public int totalMoneyAmount;

        public void Initialize()
        {
            gameManager = FindObjectOfType<GameManager>();
            
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
            }

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
            }
        }
        
        public void StartGame()
        {
            resetButton.gameObject.SetActive(true);
            tutorialText.gameObject.SetActive(false);
            
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
        }

        #region INITIALIZEGAMEMODE

        private void InitializeStandardGameMode()
        {
            totalMoneyAmount = PlayerPrefs.GetInt("TotalMoneyAmount", 0);
            gameplayMoneyBarText.text = totalMoneyAmount.ToString();
            gameplayUI.SetActive(true);
            progressBarLevelStartText.text = (gameManager.normalizedLevelIndex).ToString();
            progressBarLevelEndText.text = (gameManager.normalizedLevelIndex + 1).ToString();
            progressBarFillImage.fillAmount = 0f;
            levelProgressRatio = 0f;
            
            resetButton.gameObject.SetActive(false);
            settingUI.SetActive(false);
            
            nextLevelButton.gameObject.SetActive(false);
            
            settingButton.transform.DOScale(Vector3.one, 1f).OnComplete(() =>
            {
                settingButton.enabled = true;
            });
        }

        private void InitializeTimeGameMode()
        {
            bonusLevelUI.SetActive(true);
            bonusLevelMoneyBarText.text = "0";
            float milliSeconds = (timer % 1) * 100;
            bonusLevelTimerText.text = string.Format("{0:00}:{1:00}", timer, milliSeconds);
        }

        private void InitializeAIGameMode()
        {
            
        }

        #endregion

        #region UPDATEGAMEMODE

        private void UpdateStandardGameMode()
        {
            progressBarFillImage.fillAmount = Mathf.Lerp(progressBarFillImage.fillAmount, (float)player.collectedVoxelCount / (float)levelManager.voxelCount, Time.smoothDeltaTime * 5f);
        }

        private void UpdateTimeGameMode()
        {
            float milliSeconds = (timer % 1) * 100;
            bonusLevelTimerText.text = string.Format("{0:00}:{1:00}", timer, milliSeconds);
                
            timer -= Time.deltaTime;
            if (timer <= 0.01f)
            {
                timer = 0f;
            }
        }

        private void UpdateAIGameMode()
        {
            
        }

        #endregion

        #region BUTTONACTIONS

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
            nextLevelButton.enabled = false;
            gameManager.HandleLevelComplete();
        }

        #endregion
    }
}