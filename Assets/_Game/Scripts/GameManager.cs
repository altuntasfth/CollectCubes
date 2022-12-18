using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace _Game.Scripts
{
    public class GameManager : MonoBehaviour
    {
        public enum GameMode
        {
            STANDARD,
            AI,
            TIME
        }

        public GameMode gameMode;
        
        public UIManager uiManager;
        public PlayerMechanic player;
        public GameObject confetti;
        public Camera mainCamera;
        public bool isGameStarted;
        public bool isGameOver;
        
        public int levelIndex = 0;
        public int normalizedLevelIndex = 0;

        private void Awake()
        {
            levelIndex = PlayerPrefs.GetInt("LevelIndex", 0);
            normalizedLevelIndex = levelIndex + 1;
            Random.InitState(levelIndex * 5000);
            
            uiManager.Initialize();
            player.Initialize();
        }

        private void Update()
        {
            DebugControl();
        }

        private void DebugControl()
        {
            if (Input.GetKeyDown(KeyCode.D))
            {
                levelIndex = Mathf.Clamp(normalizedLevelIndex, 0, 1000000);
                PlayerPrefs.SetInt("LevelIndex", levelIndex);
                PlayerPrefs.Save();

                DOTween.KillAll();
                DOTween.Clear();
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                levelIndex = Mathf.Clamp(levelIndex - 1, 0, 1000000);
                PlayerPrefs.SetInt("LevelIndex", levelIndex);
                PlayerPrefs.Save();

                DOTween.KillAll();
                DOTween.Clear();
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }

        public void HandleLevelComplete()
        {
            if (isGameOver)
            {
                return;
            }
            
            isGameOver = true;
            
            DOTween.KillAll();
            DOTween.Clear();
            
            uiManager.progressBarFillImage.fillAmount = 1f;
            uiManager.levelCompleteText.text = "LEVEL " + normalizedLevelIndex + "\nCOMPLETED";

            int newMoneyAmount = PlayerPrefs.GetInt("TotalMoneyAmount") + player.collectedVoxelCount;
            PlayerPrefs.SetInt("TotalMoneyAmount", newMoneyAmount);
            PlayerPrefs.SetInt("LevelIndex", levelIndex + 1);
            PlayerPrefs.Save();
            
            confetti.SetActive(true);
            uiManager.levelCompletedScreenUI.gameObject.SetActive(true);
            uiManager.levelCompletedConfetti.SetActive(true);
            DOVirtual.DelayedCall(2f, () =>
            {
                uiManager.levelCompletedScreenUI.DOFade(0f, 0.1f).OnComplete(() =>
                {
                    DOTween.KillAll();
                    DOTween.Clear();

                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                });
            }, false);
        }
    }
}