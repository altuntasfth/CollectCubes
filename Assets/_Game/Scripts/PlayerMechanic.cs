using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _Game.Scripts
{
    public class PlayerMechanic : CharacterController
    {
        private GameManager gameManager;
        private UIManager uiManager;
        private InputManager inputManager;

        public Vector3 dragStartPosition;
        public Vector3 dragEndPosition;
        public Vector3 dragPosition;
        private float dragMagnitude;

        private void OnEnable()
        {
            InputManager.Instance.PointerDown += HandleOnPointerDown;
            InputManager.Instance.PointerDrag += HandleOnPointerDrag;
            InputManager.Instance.PointerEnd += HandleOnPointerEnd;
        }

        private void OnDisable()
        {
            InputManager.Instance.PointerDown -= HandleOnPointerDown;
            InputManager.Instance.PointerDrag += HandleOnPointerDrag;
            InputManager.Instance.PointerEnd -= HandleOnPointerEnd;
        }

        public void Initialize()
        {
            characterType = CharacterType.PLAYER;
            gameManager = FindObjectOfType<GameManager>();
            inputManager = FindObjectOfType<InputManager>();
            uiManager = FindObjectOfType<UIManager>();
        }

        private void Update()
        {
            if (gameManager == null || uiManager == null || inputManager == null)
            {
                return;
            }
            
            if (!gameManager.isGameStarted || gameManager.isGameOver)
            {
                return;
            }

            if (rb.velocity.magnitude < 10)
            {
                dragStartPosition = dragEndPosition;
            }
        }

        private void HandleOnPointerDown(PointerEventData eventData)
        {
            if (EventSystem.current.currentSelectedGameObject != null)
            {
                string eventName = EventSystem.current.currentSelectedGameObject.name;
                if (eventName.Contains("Setting"))
                {
                    Debug.Log(true);
                    return;
                }
            }

            if (uiManager.isSettingUIEnabled)
            {
                return;
            }

            if (gameManager.isGameStarted == false)
            {
                uiManager.StartGame();
                gameManager.isGameStarted = true;
            }
            
            Vector3 inputViewport = gameManager.mainCamera.ScreenToViewportPoint(eventData.position) - Vector3.one * 0.5f;
            dragStartPosition = inputViewport.x * Vector3.right + inputViewport.y * Vector3.forward;
        }

        private void HandleOnPointerDrag(PointerEventData eventData)
        {
            if (!gameManager.isGameStarted || gameManager.isGameOver)
            {
                return;
            }
            
            Vector3 inputViewport = gameManager.mainCamera.ScreenToViewportPoint(eventData.position) - Vector3.one * 0.5f;
            dragEndPosition = inputViewport.x * Vector3.right + inputViewport.y * Vector3.forward;

            Vector3 dragDistance = (dragEndPosition - dragStartPosition);
            Vector3 dragDirection = dragDistance.normalized;
            float dragMagnitude = dragDistance.magnitude;

            if (eventData.dragging)
            {
                rb.velocity = dragDirection * velocityMultiplier * Time.fixedDeltaTime;
            }

            if (dragDirection != Vector3.zero)
            {
                transform.forward = dragDirection;
            }
        }

        private void HandleOnPointerEnd(PointerEventData eventData)
        {
            rb.velocity = Vector3.zero;
        }
    }
}