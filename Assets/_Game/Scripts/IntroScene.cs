using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Game.Scripts
{
    public class IntroScene : MonoBehaviour
    {
        public float switchDelay = 1f;

        public void Awake()
        {
            DOVirtual.DelayedCall(switchDelay, () =>
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            });
        }
    }
}