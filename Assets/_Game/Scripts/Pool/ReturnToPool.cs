using UnityEngine;
using UnityEngine.Pool;

namespace _Game.Scripts.Pool
{
    public class ReturnToPool : MonoBehaviour
    {
        public IObjectPool<GameObject> pool;
    }
}