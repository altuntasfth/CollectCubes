using System;
using _Game.Scripts.Level;
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

        public LevelManager levelManager;
        public UIManager uiManager;
        public PlayerMechanic player;
        public Camera mainCamera;
        public bool isGameStarted;
        public bool isGameOver;
        
        public int levelIndex = 0;
        public int normalizedLevelIndex = 0;
        public int levelModeIndex;

        private void Awake()
        {
            Random.InitState(levelIndex * 5000);
            levelIndex = PlayerPrefs.GetInt("LevelIndex", 0);
            normalizedLevelIndex = levelIndex + 1;

            levelModeIndex = PlayerPrefs.GetInt("LevelModeIndex", 0);

            if (levelModeIndex == 0)
            {
                gameMode = GameMode.STANDARD;
            }
            else if (levelModeIndex == 1)
            {
                gameMode = GameMode.TIME;
            }
            else if (levelModeIndex == 2)
            {
                gameMode = GameMode.AI;
            }
            
            levelManager.Initialize();
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
            
            player.rb.velocity = Vector3.zero;
            
            DOTween.KillAll();
            DOTween.Clear();
            
            uiManager.progressBarFillImage.fillAmount = 1f;
            uiManager.levelCompleteText.text = "LEVEL " + normalizedLevelIndex + "\nCOMPLETED";

            int newMoneyAmount = PlayerPrefs.GetInt("TotalMoneyAmount") + player.collectedVoxelCount;
            PlayerPrefs.SetInt("TotalMoneyAmount", newMoneyAmount);
            PlayerPrefs.SetInt("LevelIndex", levelIndex + 1);
            PlayerPrefs.Save();
            
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