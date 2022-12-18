using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _Game.Scripts
{
    public class PlayerMechanic : MonoBehaviour
    {
        public float velocityMultiplier = 1500f;
        public Rigidbody rb;
        public int collectedVoxelCount;
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
            
            /*Vector2 localPoint = Vector2.zero;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(inputManager.draggingPlane, eventData.position, eventData.pressEventCamera, out localPoint);
            dragStartPosition = (localPoint.x) * Vector3.right + (localPoint.y) * Vector3.forward;*/
            
            /*RaycastHit hit;
            Ray ray = gameManager.mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 1000f, 1 << LayerMask.NameToLayer("Ground")))
            {
                dragStartPosition = hit.point;
            }*/
        }

        private void HandleOnPointerDrag(PointerEventData eventData)
        {
            if (!gameManager.isGameStarted || gameManager.isGameOver)
            {
                return;
            }
            
            Vector3 inputViewport = gameManager.mainCamera.ScreenToViewportPoint(eventData.position) - Vector3.one * 0.5f;
            dragEndPosition = inputViewport.x * Vector3.right + inputViewport.y * Vector3.forward;
            
            // Vector2 localPoint = Vector2.zero;
            // RectTransformUtility.ScreenPointToLocalPointInRectangle(inputManager.draggingPlane, eventData.position, eventData.pressEventCamera, out localPoint);
            // dragEndPosition = (localPoint.x) * Vector3.right + (localPoint.y) * Vector3.forward;
            
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
            

            Debug.DrawRay(dragStartPosition, dragDirection, Color.red);

            /*RaycastHit hit;
            Ray ray = gameManager.mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 1000f, 1 << LayerMask.NameToLayer("Ground")))
            {
                dragEndPosition = hit.point;
            }

            Vector3 dragDistance = (dragEndPosition - dragStartPosition);
            Vector3 dragDirection = dragDistance;
            float dragMagnitude = dragDistance.magnitude;

            Debug.Log(dragMagnitude);

            transform.forward = dragDirection;


            Debug.DrawRay(dragStartPosition, dragDirection, Color.red);

            if (Time.frameCount % 20 == 0)
            {
                
            }*/

            /*var delta = dragEndPosition - dragStartPosition;
            var move = new Vector3(delta.x, 0, delta.y);*/

            // GetComponent<Rigidbody>().velocity = move * 10 * Time.fixedDeltaTime;
            //
            // transform.forward = move;
            // transform.forward = Vector3.Lerp(transform.forward, move, 10 * Time.fixedDeltaTime);

            /*Vector3 fixedTouchPosition = touchPosition.x * Vector3.right + touchPosition.y * Vector3.forward;
            Vector3 inputViewport = gameManager.mainCamera.ScreenToViewportPoint(fixedTouchPosition) - Vector3.one * 0.5f;
            Debug.Log(inputViewport);
            Vector3 direction = (lastInputViewport - inputViewport).normalized;
            transform.forward = transform.position + fixedTouchPosition;*/

            /*Debug.Log(touchPosition);
            Vector3 inputViewport = gameManager.mainCamera.ScreenToViewportPoint(touchPosition) - Vector3.one * 0.5f;       
            lastPosition = transform.position;
            float deltaInputX = (inputViewport.x - lastInputX) * dragSensivity * Screen.width / Screen.height;
            float xPos = Mathf.Clamp(deltaInputX + lastPosition.x, -10, 10);
        
            transform.Rotate(new Vector3(0, xPos, 0) * rotateSpeed * Time.deltaTime, Space.World);*/
        }

        private void HandleOnPointerEnd(PointerEventData eventData)
        {
            rb.velocity = Vector3.zero;
        }
    }
}