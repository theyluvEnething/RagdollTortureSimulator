using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.Rendering.PostProcessing.HistogramMonitor;
using Debug = UnityEngine.Debug;

public class InputManager : MonoBehaviour
{
    // DRAGGING
    [Header("REFERENCES")]
    [SerializeField] private Transform _PlayerObject;

    [Header("ATTRIBUTES")]
    [SerializeField] private float ragdoll_distance = 0.1f;
    [SerializeField] private float ragdoll_spring = 210;
    [SerializeField] private float ragdoll_damper = 1f;
    [SerializeField] private float ragdoll_drag = 0f;
    [SerializeField] private float ragdoll_angularDrag = 0.3f;

    [Space(10)]
    [SerializeField] private float rigidbody_distance = 0.2f;
    [SerializeField] private float rigidbody_spring = 50.0f;
    [SerializeField] private float rigidbody_damper = 5.0f;
    [SerializeField] private float rigidbody_drag = 10.0f;
    [SerializeField] private float rigidbody_angularDrag = 5.0f;

    [Header("ITEMS")]
    [SerializeField] private GameObject _Missile;
    [SerializeField] private GameObject _Rifle;
    [SerializeField] private GameObject _ExpoRifle;
    [SerializeField] private GameObject _Gun;
    [SerializeField] private GameObject _Bazooka;
    [SerializeField] private GameObject _Chainsaw;
    [SerializeField] private GameObject _DiamondAxe;
    [SerializeField] private GameObject _Truck;
    [SerializeField] public GameObject _Dynamite;
    [SerializeField] private GameObject _Flamethrower;
    [SerializeField] private GameObject _Aquarium;

    [Header("ITEM-PREFABS")]
    [SerializeField] private GameObject _MissilePrefab;

    [Header("TUTORIALS")]
    [SerializeField] private GameObject _SwipeTutorial;
    [SerializeField] private GameObject _GroundClickTutorial;
    [SerializeField] private GameObject _MoveAroundTutorial;
    [SerializeField] private GameObject _ClickTutorial;
    [SerializeField] private GameObject _DragTutorial;
    [SerializeField] private GameObject _ButtonTutorial;

    [Header("DEBUG")]
    [SerializeField] private Transform _selectedObject;
    [SerializeField] private Rigidbody _dragRigidbody;
    [SerializeField] public static bool isDragged;
    [SerializeField] public GameObject _draggedObject;
    [SerializeField] private float noInputTimer;
    [SerializeField] public float stopTutorialTimer;

    //[Header("INSTANCE")]
    [SerializeField] public static InputManager Instance { get; private set; }

    private RagdollEnabler ragdollEnabler;
    private RagdollAnimator ragdollAnimator;
    private List<GameObject> placedObjects = new List<GameObject>();    
    private float placeTimer;
    private Camera Camera;
    private Vector3 inputPosition;
    private int groundLayerMask, backgroundLayerMask, dragLayerMask, shopLayerMask;
    private SpringJoint springJoint;
    
    private bool rifleOut, expoRifleOut, gunOut, bazookaOut, chainsawOut, diamondAxeOut, truckOut, missileOut, 
    dynamiteOut, flamethrowerOut, aquariumOut;
    
    private bool stopDrag;

    private const int MAX_OBJECTS = 20;

    void Start()
    {
        if (!Instance) Instance = this;
        Camera = DataManager.Instance.MainCamera;
        ragdollAnimator = DataManager.Instance.RagdollAnimator;
        ragdollEnabler = DataManager.Instance.RagdollEnabler;
        groundLayerMask = 1 << LayerMask.NameToLayer("Ground");
        shopLayerMask = 1 << LayerMask.NameToLayer("Shop");
        dragLayerMask = 1 << LayerMask.NameToLayer("Item") | 1 << LayerMask.NameToLayer("Movable"); ;
        backgroundLayerMask = 1 << LayerMask.NameToLayer("Background") | 1 << LayerMask.NameToLayer("Ground");
    }
    void Update()
    {
        inputPosition = Input.mousePosition;
        if (Input.GetKeyDown("1")) GameManager.Instance.ItemState = GameManager.Item.Empty;
        if (Input.GetKeyDown("2")) GameManager.Instance.ItemState = GameManager.Item.ItemMissile;
        if (Input.GetKeyDown("3")) GameManager.Instance.ItemState = GameManager.Item.ItemRifle;
        if (Input.GetKeyDown("4")) GameManager.Instance.ItemState = GameManager.Item.ItemExpoRifle;
        if (Input.GetKeyDown("5")) GameManager.Instance.ItemState = GameManager.Item.ItemGun;
        if (Input.GetKeyDown("6")) GameManager.Instance.ItemState = GameManager.Item.ItemBazooka;
        if (Input.GetKeyDown("7")) GameManager.Instance.ItemState = GameManager.Item.ItemChainsaw;
        if (Input.GetKeyDown("8")) GameManager.Instance.ItemState = GameManager.Item.ItemDiamondAxe;
        if (Input.GetKeyDown("9")) GameManager.Instance.ItemState = GameManager.Item.ItemTruck;
        if (Input.GetKeyDown("0")) GameManager.Instance.ItemState = GameManager.Item.ItemDynamite;
        if (Input.GetKeyDown("q")) GameManager.Instance.ItemState = GameManager.Item.ItemFlamethrower;
        if (Input.GetKeyDown("w")) GameManager.Instance.ItemState = GameManager.Item.ItemAquarium;
        if (isDragged && !Input.GetMouseButton(0) && !Input.GetMouseButtonDown(0)) isDragged = false;
        rifleOut = false;   expoRifleOut = false;   gunOut = false;   chainsawOut = false;   bazookaOut = false;
        diamondAxeOut = false;  truckOut = false;   missileOut = false;   dynamiteOut = false; flamethrowerOut = false;
        aquariumOut = false;

        switch (GameManager.Instance.ItemState)
        {
            case GameManager.Item.Empty:
                EmptyBehaviour();
                break;
            case GameManager.Item.ItemMissile:
                missileOut = true;
                MissileBehaviour();
                break;
            case GameManager.Item.ItemRifle:
                rifleOut = true;
                RifleBehaviour();
                break;
            case GameManager.Item.ItemExpoRifle:
                expoRifleOut = true;
                ExpoRifleBehaviour();
                break;
            case GameManager.Item.ItemGun:
                gunOut = true;
                GunBehaviour();
                break;
            case GameManager.Item.ItemBazooka:
                bazookaOut = true;
                BazookaBehaviour();
                break;
            case GameManager.Item.ItemChainsaw:
                chainsawOut = true;
                ChainsawBehaviour();
                break;
            case GameManager.Item.ItemDiamondAxe:
                diamondAxeOut = true;
                DiamondAxeBehaviour();
                break;
            case GameManager.Item.ItemTruck:
                truckOut = true;
                TruckBehaviour();
                break;            
            case GameManager.Item.ItemDynamite:
                dynamiteOut = true;
                DynamiteBehaviour();
                break;            
            case GameManager.Item.ItemFlamethrower:
                flamethrowerOut = true;
                FlamethrowerBehaviour();
                break;
            case GameManager.Item.ItemAquarium:
                aquariumOut = true;
                AquariumBehaviour();
                break;
        }

        if (!rifleOut && _Rifle.activeSelf) RifleBehaviour();
        if (!expoRifleOut && _ExpoRifle.activeSelf) ExpoRifleBehaviour();
        if (!gunOut && _Gun.activeSelf) GunBehaviour();
        if (!bazookaOut && _Bazooka.activeSelf) BazookaBehaviour();
        if (!chainsawOut && _Chainsaw.activeSelf) ChainsawBehaviour();
        if (!diamondAxeOut && _DiamondAxe.activeSelf) DiamondAxeBehaviour();
        if (!truckOut && _Truck.activeSelf) TruckBehaviour();
        if (!missileOut && _Missile.activeSelf) MissileBehaviour();
        if (!dynamiteOut && _Dynamite.activeSelf) DynamiteBehaviour();
        if (!flamethrowerOut && _Flamethrower.activeSelf) FlamethrowerBehaviour();
        if (!aquariumOut && _Aquarium.activeSelf) AquariumBehaviour();

        ShowInputTutorial();
    }
    void FixedUpdate()
    {
        placeTimer -= Time.deltaTime;

        for (int i = 0; i < placedObjects.Count; i++)
        {
            if (placedObjects[i] == null)
            {
                placedObjects.RemoveAt(i);
            }
        }
    }
    private void ShowInputTutorial()
    {
        stopTutorialTimer -= Time.deltaTime;
        if (!ChangeScene.Instance.IsActiveMainRoom || stopTutorialTimer > 0 || ChangeScene.Instance.isActiveAquarium)
            return;

        bool swipeAnimationPlaying = false;
        bool groundClickAnimationPlaying = false;
        bool moveAroundAnimationPlaying = false;
        bool clickAnimationPlaying = false;
        bool dragAnimationPlaying = false;
        bool buttonAnimationPlaying = false;

        if (!Input.GetMouseButton(0) && !Input.GetMouseButtonDown(0) && !Input.GetMouseButtonUp(0))
            noInputTimer += Time.deltaTime;
        else
            noInputTimer = 0f;

        if (noInputTimer > 4f)
        {
            switch (GameManager.Instance.ItemState)
            {
                case GameManager.Item.Empty:
                    dragAnimationPlaying = true;
                    _DragTutorial.SetActive(true);
                    break;
                case GameManager.Item.ItemChainsaw:
                    dragAnimationPlaying = true;
                    _DragTutorial.SetActive(true);
                    break;    
                    
                case GameManager.Item.ItemDiamondAxe:
                    dragAnimationPlaying = true;
                    _DragTutorial.SetActive(true);
                    break;
                case GameManager.Item.ItemDynamite:
                    swipeAnimationPlaying = true;
                    _SwipeTutorial.SetActive(true);
                    break;           
                case GameManager.Item.ItemMissile:
                    groundClickAnimationPlaying = true;
                    _GroundClickTutorial.SetActive(true);
                    break;   
                case GameManager.Item.ItemRifle:
                    moveAroundAnimationPlaying = true;
                    _MoveAroundTutorial.SetActive(true);
                    break;               
                case GameManager.Item.ItemExpoRifle:
                    moveAroundAnimationPlaying = true;
                    _MoveAroundTutorial.SetActive(true);
                    break;      
                case GameManager.Item.ItemBazooka:
                    moveAroundAnimationPlaying = true;
                    _MoveAroundTutorial.SetActive(true);
                    break;
                case GameManager.Item.ItemFlamethrower:
                    moveAroundAnimationPlaying = true;
                    _MoveAroundTutorial.SetActive(true);
                    break;
                case GameManager.Item.ItemGun:
                    clickAnimationPlaying = true;
                    _ClickTutorial.SetActive(true);
                    break;
                case GameManager.Item.ItemTruck:
                    buttonAnimationPlaying = true;
                    _ButtonTutorial.SetActive(true);
                    break;     
                case GameManager.Item.ItemAquarium:
                    buttonAnimationPlaying = true;
                    _ButtonTutorial.SetActive(true);
                    break;     
            }
        }

        if (!swipeAnimationPlaying)
            _SwipeTutorial.SetActive(false);     
        if (!groundClickAnimationPlaying)
            _GroundClickTutorial.SetActive(false);
        if (!moveAroundAnimationPlaying)
            _MoveAroundTutorial.SetActive(false);
        if (!clickAnimationPlaying)
            _ClickTutorial.SetActive(false);    
        if (!dragAnimationPlaying)
            _DragTutorial.SetActive(false);       
        if (!buttonAnimationPlaying)
            _ButtonTutorial.SetActive(false);

    }
    private void EmptyBehaviour()
    {
        if (!Input.GetMouseButton(0) && !Input.GetMouseButtonDown(0))
            return;
        DragBehaviour();
    }
    private void MissileBehaviour()
    {
        GeneralBehaviour(missileOut, _Missile);

        if (!Input.GetMouseButton(0) && !Input.GetMouseButtonDown(0))
            return;

        int drag = DragBehaviour();

        if (drag == 1)
        {
            return;
        }

        if (!Physics.Raycast(Camera.ScreenPointToRay(inputPosition), out RaycastHit hit, Mathf.Infinity, groundLayerMask))
        {
            return;
        }

        if (placeTimer <= 0)
        {
            placeTimer = 0.2f;
            Vector3 offset = new Vector3(0f, 2f, 0f);
            placedObjects.Add(Instantiate(_MissilePrefab, hit.point + offset, _MissilePrefab.transform.rotation));

            if (placedObjects.Count > MAX_OBJECTS)
            {
                Destroy(placedObjects[0]);
                placedObjects.RemoveAt(0);
            }
        }
    }
    private void RifleBehaviour()
    {
        GeneralBehaviour(rifleOut, _Rifle);
    }
    private void ExpoRifleBehaviour()
    {
        GeneralBehaviour(expoRifleOut, _ExpoRifle);
    }
    private void GunBehaviour()
    {
        GeneralBehaviour(gunOut, _Gun);
    }
    private void BazookaBehaviour()
    {
        GeneralBehaviour(bazookaOut, _Bazooka);
    }
    private void ChainsawBehaviour()
    {
        GeneralBehaviour(chainsawOut, _Chainsaw);
        DragBehaviour();
    }
    private void DiamondAxeBehaviour()
    {
        GeneralBehaviour(diamondAxeOut, _DiamondAxe);
        DragBehaviour();
    }
    private void TruckBehaviour()
    {
        GeneralBehaviour(truckOut, _Truck);
    }
    private void DynamiteBehaviour()
    {
        GeneralBehaviour(dynamiteOut, _Dynamite);
    }
    private void FlamethrowerBehaviour()
    {
        GeneralBehaviour(flamethrowerOut, _Flamethrower);
    }
    private void AquariumBehaviour()
    {
        DragBehaviour();
        GeneralBehaviour(aquariumOut, _Aquarium);
    }
    protected void GeneralBehaviour(bool isOut, GameObject _Item)
    {
        if (isOut && !_Item.activeSelf)
        {
            _Item.SetActive(true);
        }
        else if (!isOut && _Item.activeSelf)
        {
            _Item.SetActive(false);
        }
    }
    protected int DragBehaviour()
    {
        if (Physics.Raycast(Camera.ScreenPointToRay(inputPosition), out RaycastHit hit, Mathf.Infinity, dragLayerMask))
        {
            // Make Character ragdoll
            if (hit.transform.CompareTag("EnableRagdoll") && Input.GetMouseButtonDown(0))
            {
                ragdollAnimator.PushRagdoll();
                DragBehaviour();
                return 0;
            }
            if (Input.GetMouseButtonUp(0))
            {
                if (_draggedObject.CompareTag("Item"))
                {
                    Item selectedItem = _draggedObject.GetComponent<Item>();
                    selectedItem.isDragged = false;
                }
                _draggedObject = null;
                _selectedObject = null;
                _dragRigidbody = null;
                isDragged = false;

            }
            if (!isDragged && Input.GetMouseButtonDown(0))
            {
                _selectedObject = hit.transform;
                _draggedObject = _selectedObject.gameObject;
                _dragRigidbody = hit.collider.attachedRigidbody;
                isDragged = true;

                if (_draggedObject.CompareTag("Player"))
                {
                    _selectedObject = _PlayerObject;
                    _draggedObject = _selectedObject.gameObject;
                    _dragRigidbody = _PlayerObject.GetComponent<Rigidbody>();
                }
                if (hit.transform.CompareTag("Item"))
                {
                    Item selectedItem = hit.transform.GetComponent<Item>();
                    selectedItem.isDragged = true;
                }
            }
        }
        if (isDragged)
        {
            if (_selectedObject.CompareTag("Player"))
                DragRagdoll();
            else if (_selectedObject.CompareTag("Movable") || _selectedObject.CompareTag("Item"))
                DragRigidbody();
            return 1;
        }
        return 0;
    }
    protected void DragRagdoll()
    {
        if (!Physics.Raycast(Camera.ScreenPointToRay(inputPosition), out RaycastHit mouse, Mathf.Infinity, dragLayerMask))
            return;

        if (!_dragRigidbody) return;


        // Correct z-Position
        Vector3 changeZ = mouse.point;
        changeZ.z = DataManager.Instance.PlayerTransform.position.z;
        mouse.point = changeZ;
        
        Debug.Log(changeZ);
        Debug.Log(mouse.point);

        if (!springJoint)
        {
            GameObject go = new GameObject("Rigidbody dragger");
            Rigidbody body = go.AddComponent<Rigidbody>();
            body.isKinematic = true;
            springJoint = go.AddComponent<SpringJoint>();
        }

        springJoint.transform.position = mouse.point;

        springJoint.spring = ragdoll_spring;
        springJoint.damper = ragdoll_damper;
        springJoint.maxDistance = ragdoll_distance;
        if (_dragRigidbody) springJoint.connectedBody = _dragRigidbody;
        springJoint.connectedBody.drag = ragdoll_drag;
        springJoint.connectedBody.angularDrag = ragdoll_angularDrag;

        StartCoroutine(MoveObject(mouse.distance));
    }
    protected void DragRigidbody()
    {
        if (!Physics.Raycast(Camera.ScreenPointToRay(inputPosition), out RaycastHit mouse, Mathf.Infinity, dragLayerMask))
            return;

        
        if (!springJoint)
        {
            GameObject go = new GameObject("Rigidbody dragger");
            Rigidbody body = go.AddComponent<Rigidbody>();
            body.isKinematic = true;
            springJoint = go.AddComponent<SpringJoint>();
        }

        springJoint.transform.position = mouse.point;

        springJoint.spring = rigidbody_spring;
        springJoint.damper = rigidbody_damper;
        springJoint.maxDistance = rigidbody_distance;
        if (_dragRigidbody) springJoint.connectedBody = _dragRigidbody;
        springJoint.connectedBody.drag = rigidbody_drag;
        springJoint.connectedBody.angularDrag = rigidbody_angularDrag;


        StartCoroutine(MoveObject(mouse.distance + 3f));
        
    }
    protected IEnumerator MoveObject(float distance)
    {
        while (Input.GetMouseButton(0) && !stopDrag)
        {
            Ray ray = Camera.ScreenPointToRay(inputPosition);
            springJoint.transform.position = ray.GetPoint(distance);
            yield return null;
        }

        if (stopDrag) stopDrag = false;

        if (springJoint.connectedBody)
        {
            springJoint.connectedBody.drag = 0;
            springJoint.connectedBody.angularDrag = 0.05f;
            springJoint.connectedBody = null;
        }
    }
    public void InterruptDrag()
    {
        if (_draggedObject && _draggedObject.CompareTag("Item"))
        {
            if (!_draggedObject.GetComponent<Item>())
                return;
            Item selectedItem = _draggedObject.GetComponent<Item>();
            selectedItem.isDragged = false;
        }
        _draggedObject = null;
        _selectedObject = null;
        _dragRigidbody = null;
        isDragged = false;
        stopDrag = true;
    }
}
