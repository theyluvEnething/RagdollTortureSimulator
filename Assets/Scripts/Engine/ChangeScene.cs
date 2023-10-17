using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ChangeScene : MonoBehaviour
{
    [Header("REFERENCES")]
    [SerializeField] private Transform _MainRoom;
    [SerializeField] private Transform _ShopRoom;
    [SerializeField] private Transform _SkinRoom;
    [SerializeField] private Transform _Desert;
    [SerializeField] private Transform _Aquarium;
    [SerializeField] private Transform _Items;

    [Header("FORCE-CAMERAS")]
    [SerializeField] private MoveCamera _DesertCamera;
    [SerializeField] private MoveCamera _SkinCamera;

    [Header("SETTINGS")]
    [SerializeField] public SceneState sceneState;
    [SerializeField] public bool inTransition;
    [SerializeField] private float transitionTime = 2f;
    public static ChangeScene Instance;

    public bool coroutineIsRunning = false;
    public bool IsActiveMainRoom = true;
    public bool IsActiveShopRoom = false;
    public bool IsActiveSkinRoom = false;
    public bool isActiveDesert = false;
    public bool isActiveAquarium = false;

    public enum SceneState
    {
        Undefined,
        MainRoomActive,
        ShopRoomActive,
        SkinRoomActive,
        DesertActive,
        AquariumActive,
        WaitForNewScene,
        OnlyExitSign,
    }

    List<GameObject> _ObjectsMainRoom, _ObjectsShopRoom, _ObjectsSkinRoom, _ObjectsDesert, _ObjectsAquarium, _ObjectsItem;

    IEnumerator Start()
    {
        if (!Instance)
            Instance = this;
        sceneState = SceneState.WaitForNewScene;

        _ObjectsMainRoom = GetObjectsIn(_MainRoom);
        _ObjectsShopRoom = GetObjectsIn(_ShopRoom);
        _ObjectsSkinRoom = GetObjectsIn(_SkinRoom);
        _ObjectsDesert = GetObjectsIn(_Desert);
        _ObjectsAquarium = GetObjectsIn(_Aquarium);
        _ObjectsItem = GetObjectsIn(_Items);

        // Wait to ensure everything has loaded properly
        yield return new WaitForSeconds(1f);

        ActivateScene(_ObjectsMainRoom, IsActiveMainRoom);
        ActivateScene(_ObjectsShopRoom, IsActiveShopRoom);
        ActivateScene(_ObjectsDesert, isActiveDesert);
        ActivateScene(_ObjectsAquarium, isActiveAquarium);
        ActivateScene(_ObjectsSkinRoom, IsActiveSkinRoom);
    }

    void LateUpdate()
    {
        if (coroutineIsRunning) return;
        switch (sceneState)
        {
            case SceneState.WaitForNewScene:
                break;

            case SceneState.MainRoomActive:
                coroutineIsRunning = true;
                StartCoroutine(MainRoomBehaviour());
                break;

            case SceneState.ShopRoomActive:
                coroutineIsRunning = true;
                StartCoroutine(ShopRoomBehaviour());
                break;

            case SceneState.DesertActive:
                coroutineIsRunning = true;
                StartCoroutine(DesertBehaviour());
                break;

            case SceneState.SkinRoomActive:
                coroutineIsRunning = true;
                StartCoroutine(SkinRoomBehaviour());
                break;

                //
                //
                //case SceneState.AquariumActive:
                //    coroutineIsRunning = true;
                //    StartCoroutine(AquariumBehaviour());
                //    break;
        }

        switch (GameManager.Instance.ItemState)
        {
            case GameManager.Item.ItemAquarium:
                isActiveAquarium = true;
                ActivateScene(_ObjectsAquarium, true);
                break;

            default:
                ActivateScene(_ObjectsAquarium, false);
                isActiveAquarium = false;
                break;
        }
    }

    private IEnumerator MainRoomBehaviour()
    {
        if (inTransition) yield return null;
        inTransition = true;
        if (!IsActiveMainRoom)
        {
            ActivateScene(_ObjectsMainRoom, true);
            IsActiveMainRoom = true;
        }
        yield return new WaitForSeconds(transitionTime);
        if (IsActiveShopRoom)
        {
            ActivateScene(_ObjectsShopRoom, false);
            IsActiveShopRoom = false;
        }
        if (isActiveDesert)
        {
            ActivateScene(_ObjectsDesert, false);
            isActiveDesert = false;
        }
        coroutineIsRunning = false;
        sceneState = SceneState.WaitForNewScene;
        inTransition = false;
    }
    private IEnumerator ShopRoomBehaviour()
    {
        if (inTransition) yield return null;
        inTransition = true;
        if (!IsActiveShopRoom)
        {
            ActivateScene(_ObjectsShopRoom, true);
            IsActiveShopRoom = true;
        }
        yield return new WaitForSeconds(transitionTime);
        if (IsActiveMainRoom)
        {
            RemoveObjects(_ObjectsItem);
            ActivateScene(_ObjectsMainRoom, false);
            IsActiveMainRoom = false;
        }
        if (isActiveDesert)
        {
            ActivateScene(_ObjectsDesert, false);
            isActiveDesert = false;
        }
        coroutineIsRunning = false;
        sceneState = SceneState.WaitForNewScene;
        inTransition = false;
    }
    private IEnumerator DesertBehaviour()
    {
        if (inTransition) yield return null;
        inTransition = true;
        if (!isActiveDesert)
        {
            ActivateScene(_ObjectsDesert, true);
            isActiveDesert = true;
        }
        yield return new WaitForSeconds(transitionTime);
        if (IsActiveShopRoom)
        {
            ActivateScene(_ObjectsShopRoom, false);
            IsActiveShopRoom = false;
        }
        coroutineIsRunning = false;
        sceneState = SceneState.WaitForNewScene;
        inTransition = false;

        yield return new WaitForSeconds(0.75f);
        sceneState = SceneState.ShopRoomActive;
        _DesertCamera.ForceMove();
    }
    private IEnumerator SkinRoomBehaviour()
    {
        if (inTransition) yield return null;
        inTransition = true;
        if (!IsActiveSkinRoom)
        {
            ActivateScene(_ObjectsSkinRoom, true);
            IsActiveSkinRoom = true;
        }
        yield return new WaitForSeconds(transitionTime);
        if (IsActiveShopRoom)
        {
            ActivateScene(_ObjectsShopRoom, false);
            IsActiveShopRoom = false;
        }
        coroutineIsRunning = false;
        sceneState = SceneState.WaitForNewScene;
        inTransition = false;

        sceneState = SceneState.ShopRoomActive;
        yield return new WaitForSeconds(0.75f);
        _SkinCamera.ForceMove();
    }
    private IEnumerator AquariumBehaviour()
    {
        if (inTransition) yield return null;
        inTransition = true;
        if (!isActiveAquarium)
        {
            ActivateScene(_ObjectsAquarium, true);
            isActiveAquarium = true;
        }
        yield return new WaitForSeconds(transitionTime);
        /*
        if (IsActiveMainRoom)
        {
            ActivateScene(_ObjectsMainRoom, false);
            IsActiveMainRoom = false;
        }
        */

        ChangeObstacleState.Instace.activateObstacles = false;

        coroutineIsRunning = false;
        sceneState = SceneState.WaitForNewScene;
        inTransition = false;
    }
    private List<GameObject> GetObjectsIn(Transform parentTransform)
    {
        List<GameObject> childObjects = new List<GameObject>();
        int childCount = parentTransform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            Transform childTransform = parentTransform.GetChild(i);
            GameObject childObject = childTransform.gameObject;
            childObjects.Add(childObject);
        }
        return childObjects;
    }
    private void ActivateScene(List<GameObject> GameObjects, bool active)
    {
        foreach (GameObject gameObject in GameObjects)
            gameObject.SetActive(active);
    }
    private void RemoveObjects(List<GameObject> GameObjects)
    {
        foreach (GameObject gameObject in GameObjects)
            Destroy(gameObject);
    }
}
