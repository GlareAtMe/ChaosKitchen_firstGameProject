using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour, IKitchenObjectParrent  
{
    public static Player Instance { get; private set; }

    public event EventHandler OnPickupKitchenObject;
    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangedEventArgs : EventArgs {
        public BaseCounter selectedClearCounter;
    }

    [SerializeField] private GameInput gameInput;
    [SerializeField] private LayerMask countersLayerMask;
    [SerializeField] private Transform kitchenObjectHoldPoint;

    private bool isWalking;
    private float moveSpeed = 6f;
    private Vector3 lastInteractDiractions;
    private BaseCounter selectedCounter;
    private KitchenObject kitchenObj;


    private void Awake() {

        if (Instance != null) {
            Debug.LogError("There is more then one player instance");
        }

        Instance = this;
    }

    private void Start() {
        gameInput.OnInteractAction += GameInput_OnInteractAction;
        gameInput.OnAlternatInteractionAction += GameInput_OnAlternatInteractionAction;
    }

    private void GameInput_OnAlternatInteractionAction(object sender, EventArgs e) {
        if (!GameManager.Instance.IsGamePlaying()) return;

        if (selectedCounter != null) {
            selectedCounter.InteractAlternate(this);
        }
    }

    private void GameInput_OnInteractAction(object sender, EventArgs e) {
        if(!GameManager.Instance.IsGamePlaying()) return;


        if (selectedCounter != null) { 
            selectedCounter.Interact(this);
        }
    }
    private void Update() {
        HandleMovement();
        HandleInteractions();
    }
    public bool IsWalking() {
        return isWalking;
    }

    private void HandleInteractions() {
        float interactDistance = 2f;
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();

        Vector3 moveDiraction = new Vector3(inputVector.x, 0f, inputVector.y);
        if (moveDiraction != Vector3.zero) {
            lastInteractDiractions = moveDiraction;
        }
        if (Physics.Raycast(transform.position, lastInteractDiractions, out RaycastHit raycastHit, interactDistance, countersLayerMask)) {
            if (raycastHit.transform.TryGetComponent(out BaseCounter baseCounter)) {
                //has clear counter 
                if (baseCounter != selectedCounter) {
                    SetSelectedCounter(baseCounter);
                }
            } else {
                SetSelectedCounter(null);
            }
        } else {
            SetSelectedCounter(null);
        };
    }

    private void HandleMovement() {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();

        Vector3 moveDiraction = new Vector3(inputVector.x, 0f, inputVector.y);

        float playerRadius = .7f;
        float playerHight = 2f;
        float moveDistance = moveSpeed * Time.deltaTime;

        //lazy way is :  bool canMove = ! Physics.Raycast(transform.position, moveDiraction, playerSize);
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHight, playerRadius, moveDiraction, moveDistance);

        if (!canMove) {
            //Cannot move toward moveDiraction

            //attempt only x movement 
            Vector3 moveDiractionX = new Vector3(moveDiraction.x, 0, 0).normalized;
            canMove = (moveDiraction.x < -.5f || moveDiraction.x > .5f)  && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHight, playerRadius, moveDiractionX, moveDistance);

            if (canMove) {
                //can move only on x
                moveDiraction = moveDiractionX;
            } else {
                //cannot move only on X

                //attempt only Z movement
                Vector3 moveDiractionZ = new Vector3(0, 0, moveDiraction.z).normalized;
                canMove = (moveDiraction.z < -.5f || moveDiraction.z > .5f) && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHight, playerRadius, moveDiractionZ, moveDistance);

                if (canMove) {
                    //can only move on z
                    moveDiraction = moveDiractionZ;
                } else {
                    //cannot move any diraction 
                }

            }
        }

        if (canMove) {
            transform.position += moveDiraction * moveDistance;
        }
        isWalking = moveDiraction != Vector3.zero;

        float rotateSpeed = 7f;
        transform.forward = Vector3.Slerp(transform.forward, moveDiraction, Time.deltaTime * rotateSpeed);
    }

    private void SetSelectedCounter(BaseCounter selectedCounter) { 
        this.selectedCounter = selectedCounter;

        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs {
            selectedClearCounter = selectedCounter
        });
    }
    public Transform GetKitchenObjectFollowTransform() {
        return kitchenObjectHoldPoint;
    }

    public void SetKitchenObject(KitchenObject kitchenObj) {
        this.kitchenObj = kitchenObj;

        if (kitchenObj != null) {
            OnPickupKitchenObject?.Invoke(this, EventArgs.Empty);
        }
    }

    public KitchenObject GetKitchenObject() {
        return kitchenObj;
    }

    public void ClearKitchenObject() {
        kitchenObj = null;
    }

    public bool HasKitchenObject() {
        return kitchenObj != null;
    }
}
