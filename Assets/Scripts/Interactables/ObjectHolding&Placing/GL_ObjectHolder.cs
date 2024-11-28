using System;
using System.Collections.Generic;
using Character.Enemy;
using Enums;
using Extensions;
using GameEvents;
using GameEvents.Enum;
using Interactables;
using Interactables.ObjectHolding_Placing;
using Towers;
using Towers.Interface;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(GL_InteracterRaycaster))]
public class GL_ObjectHolder : MonoBehaviour
{
    private GL_InteracterRaycaster _interacterRaycaster;
    private GL_IHoldable _currentHoldable;
    
    [SerializeField] private GameEvent<GameEventInfo> _tryPickupEvent;
    [SerializeField] private GameEvent<GameEventInfo> _interactInputEvent;
    [SerializeField] private GameEvent<GameEventInfo> _tryPlaceInputEvent;

    [SerializeField] private Material _placingMaterial;
    [SerializeField] private float _dropMaxDistance = 2;

    private Material _currentPlacingMaterial;
    [SerializeField] private Color _canPlaceColor;
    [SerializeField] private Color _cannotPlaceColor;

    [SerializeField] private Vector3 _placeRotation;
    
    private GameObject _drawObject;
    private NavMeshObstacle _drawObjectObstacle;
    private GameObject _drawObjectRangePreview;
    [SerializeField] private Mesh _previewMesh;
    private bool _canPlaceObject;
    private bool _isNightTime;

    private const float OBJECT_SKIN_WIDTH = 0.01f;

    private bool _canTracePath;
    


    private void Awake()
    {
        _interacterRaycaster = GetComponent<GL_InteracterRaycaster>();
        _tryPickupEvent?.AddListener(OnTryPickup);
        _interactInputEvent?.AddListener(TryDrop);
        _tryPlaceInputEvent?.AddListener(TryPlace);
        GameEventEnum.OnDayEnded.AddListener((eventInfo) => { _isNightTime = true; });
        GameEventEnum.OnNightEnded.AddListener((eventInfo) => { _isNightTime = false; });
        GameEventEnum.AnswerCanPathTrace.AddListener(OnCanPathTraceAnswer);
        GameEventEnum.AnswerCanPathTrace.AddListener(OnAnswerCanPathTrace);
    }

    private void OnCanPathTraceAnswer(GameEventInfo eventInfo)
    {
        if (!gameObject.HasGameID(eventInfo.Ids) || !eventInfo.TryTo(out GameEventBool gameEventBool))
        {
            return;
        }
        
        _canTracePath = gameEventBool.Value;
    }

    private void Update()
    {
        if (_currentHoldable != null && _currentHoldable.IsPlaceable())
        {
            DrawPreview(_currentHoldable.GetPlaceable().PlaceableObject);
        }
    }

    private void DrawPreview(GameObject previewedObject)
    {
        bool spawnedObject = false;
        if (!_drawObject)
        {
            spawnedObject = true;
            _drawObject = Instantiate(previewedObject);
            var components = _drawObject.GetComponentsInChildren<Component>();
            foreach (var component in components)
            {
                if (component is Renderer or Collider or Transform or MeshFilter)
                {
                    continue;
                }
                Destroy(component);
            }
        }

        _canPlaceObject = true;

        _drawObject.transform.eulerAngles = _placeRotation;
        _drawObject.transform.position = Vector3.zero;
        Bounds localObjectBounds = _drawObject.GetCollidersBounds();
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hitInfo, _dropMaxDistance,
                ~((int)LayerMaskEnum.Character | (int)LayerMaskEnum.Item | (int)LayerMaskEnum.Path |
                  (int)LayerMaskEnum.IgnoreRaycast)))
        {
            _drawObject.transform.position = hitInfo.point + hitInfo.normal *
                Vector3.Dot(hitInfo.normal.Abs(), localObjectBounds.extents.Abs()) - localObjectBounds.center;
            if (!hitInfo.collider.gameObject.TryGetComponentInParents(out GL_IPlaceableGround placeableGround))
            {
                _canPlaceObject = false;
            }
        }
        else
        {
            Vector3 itemPoint = transform.forward * _dropMaxDistance;
            _drawObject.transform.position = transform.position + itemPoint - localObjectBounds.center;
            _canPlaceObject = false;
        }
        Bounds worldObjectBounds = _drawObject.GetCollidersBounds();
        int ignoreLayer = (int)LayerMaskEnum.IgnoreRaycast;
        if (_isNightTime)
        {
            ignoreLayer |= (int)LayerMaskEnum.Path;
        }
        Collider[] overlapColliders = Physics.OverlapBox(worldObjectBounds.center,
            localObjectBounds.extents - Vector3.one * OBJECT_SKIN_WIDTH, Quaternion.identity,
            ~ignoreLayer);

        if (overlapColliders.Length > 0)
        {
            _canPlaceObject = false;
        }

        if (_canPlaceObject)
        {
            GameEventEnum.AskCanPathTrace.Invoke(new GameEventInfo { Ids = new[] { gameObject.GetGameID() }});
            _canPlaceObject = _canTracePath;
        }
        
        if (_currentPlacingMaterial)
        {
            ChangePlacingMaterialColor(hitInfo.collider);
        }
        
        
        if (!spawnedObject)
        {
            return;
        }
        
        SetPlacingMaterials(_drawObject);

        var colliders = _drawObject.GetComponentsInChildren<Collider>();
        foreach (Collider collider in colliders)
        {
            collider.enabled = false;
        }
        
        _drawObjectObstacle = _drawObject.AddComponent<NavMeshObstacle>();
        _drawObjectObstacle.shape = NavMeshObstacleShape.Box;
        _drawObjectObstacle.carving = true;
        _drawObjectObstacle.carveOnlyStationary = false;
        _drawObjectObstacle.center = localObjectBounds.center;
        _drawObjectObstacle.size = localObjectBounds.extents * 2 + Vector3.one * OBJECT_SKIN_WIDTH;

        GL_IPlaceable placeableObject = _currentHoldable.GetPlaceable();
        if (placeableObject is GL_TowerPlaceable towerPlaceable)
        {
            GL_TowerInfo towerInfo = towerPlaceable.TowerInfo;
            _drawObjectRangePreview = new GameObject("RangePreview");
            _drawObjectRangePreview.transform.SetParent(_drawObject.transform);
            _drawObjectRangePreview.AddComponent<MeshFilter>().mesh = _previewMesh;
            _drawObjectRangePreview.AddComponent<MeshRenderer>().material = _currentPlacingMaterial;
            _drawObjectRangePreview.transform.localPosition = new Vector3(0, -localObjectBounds.extents.y + localObjectBounds.center.y);
            _drawObjectRangePreview.transform.localScale = new Vector3(towerInfo.AttackRadius * 2, 0.5f, towerInfo.AttackRadius * 2);
        }
        
        
        _drawObject.SetActive(true);
    }

    private void ChangePlacingMaterialColor(bool isOnGround)
    {
        _currentPlacingMaterial.color = _canPlaceObject ? _canPlaceColor : _cannotPlaceColor;
        if (_drawObjectRangePreview)
        {
            _drawObjectRangePreview.SetActive(isOnGround);
        }
    }

    private void SetPlacingMaterials(GameObject objectToChange)
    {
        var renderers = objectToChange.GetComponentsInChildren<Renderer>();
        _currentPlacingMaterial = new(_placingMaterial);
        foreach (Renderer renderer in renderers)
        {
            List<Material> newMaterials = new List<Material>();
            for (int i = 0; i < renderer.materials.Length; i++)
            {
                newMaterials.Add(_currentPlacingMaterial);
            }
            renderer.SetMaterials(newMaterials);
        }
    }

    private void TryPlace(GameEventInfo eventInfo)
    {
        if (_currentHoldable == null || !_canPlaceObject)
        {
            return;
        }
        
        GameEventEnum.AskCanPathTrace.Invoke(new GameEventInfo { Ids = new[] { gameObject.GetGameID() }});
        
        if (!_canTracePath)
        {
            return;
        }

        _drawObjectObstacle.enabled = false;
        
        var currentPlaceable = _currentHoldable.GetPlaceable();
        currentPlaceable.Place(_drawObject.transform.position, _placeRotation);
        if (currentPlaceable.DestroyItemOnPlaced)
        {
            Destroy(_currentHoldable.GetGameObject());
            Reset();
        }

        _drawObjectObstacle.enabled = true;
    }

    private void OnAnswerCanPathTrace(GameEventInfo eventInfo)
    {
        if (!gameObject.HasGameID(eventInfo.Ids) || !eventInfo.TryTo(out GameEventBool gameEventBool))
        {
            return;
        }

        _canTracePath = gameEventBool.Value;
    }

    public void OnTryPickup(GameEventInfo eventInfo)
    {
        if (!gameObject.HasGameID(eventInfo.Ids) || !eventInfo.TryTo(out GameEventGameObject gameEventGameObject))
        {
            return;
        }
        
        Pickup(gameEventGameObject.Value);
    }
    
    public void Pickup(GameObject pickedObject)
    {
        if (!pickedObject.TryGetComponent(out GL_IHoldable holdable))
        {
            return;
        }
        
        _currentHoldable = holdable;
        _currentHoldable.OnPickup();
        
        _interacterRaycaster.DisableComponent();
    }

    public void Drop()
    {
        if (_currentHoldable == null)
        {
            return;
        }
        
        var droppedObject = _currentHoldable.GetGameObject();
        Bounds objectBounds = droppedObject.GetCollidersBounds();
        droppedObject.transform.position = transform.position;
        droppedObject.transform.eulerAngles = new Vector3(0, transform.eulerAngles.y);
        
        _currentHoldable.OnDropped();
        Reset();
    }

    private void Reset()
    {
        Timer.Timer.NewTimer(0, () => { _interacterRaycaster.EnableComponent(); });
        _currentHoldable = null;
        Destroy(_drawObject);
    }

    private void TryDrop(GameEventInfo eventInfo)
    {
        if (!gameObject.HasGameID(eventInfo.Ids) || _currentHoldable == null)
        {
            return;
        }
        
        Drop();
    }
}
