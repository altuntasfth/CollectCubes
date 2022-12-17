using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace _Game.Scripts.Pool
{
    public class PoolManager : MonoBehaviour
    {
        public enum PoolType
        {
            Stack,
            LinkedList
        }

        public PoolType poolType;

        // Collection checks will throw errors if we try to release an item that is already in the pool.
        public bool collectionChecks = true;
        public int maxPoolSize = 10;

        public PoolData poolData;

        IObjectPool<GameObject> m_Pool;

        public List<GameObject> poolItems;
        
        
        public static PoolManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                Instance = this;
            }
            
            //GeneratePools();
        }

        private void Update()
        {
            DebugPool();
        }

        private void DebugPool()
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                poolItems.Add(Pool.Get());
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                if (poolItems.Count != 0)
                {
                    m_Pool.Release(poolItems[poolItems.Count-1]);
                    poolItems.Remove(poolItems[poolItems.Count - 1]);
                }
            }
        }

        public IObjectPool<GameObject> Pool
        {
            get
            {
                if (m_Pool == null)
                {
                    if (poolType == PoolType.Stack)
                        m_Pool = new ObjectPool<GameObject>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool,
                            OnDestroyPoolObject, collectionChecks, 10, maxPoolSize);
                    else
                        m_Pool = new LinkedPool<GameObject>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool,
                            OnDestroyPoolObject, collectionChecks, maxPoolSize);
                }

                return m_Pool;
            }
        }

        private GameObject CreatePooledItem()
        {
            var go = Instantiate(poolData.poolInfo.prefab);
            return go;
        }

        // Called when an item is returned to the pool using Release
        private void OnReturnedToPool(GameObject gObject)
        {
            gObject.SetActive(false);
        }

        // Called when an item is taken from the pool using Get
        private void OnTakeFromPool(GameObject gObject)
        {
            gObject.SetActive(true);
        }

        // If the pool capacity is reached then any items returned will be destroyed.
        // We can control what the destroy behavior does, here we destroy the GameObject.
        private void OnDestroyPoolObject(GameObject gObject)
        {
            Destroy(gObject);
        }
    }
}