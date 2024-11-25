using System;
using System.Collections.Generic;
using Enums;
using Extensions;
using GameEvents;
using GameEvents.Enum;
using Interactables;
using Interactables.ObjectHolding_Placing;
using Unity.Mathematics;
using UnityEngine;

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
    private bool _canPlaceObject;
    private bool _isNightTime;

    private const float OBJECT_SKIN_WIDTH = 0.01f;


    private void Awake()
    {
        _interacterRaycaster = GetComponent<GL_InteracterRaycaster>();
        _tryPickupEvent?.AddListener(OnTryPickup);
        _interactInputEvent?.AddListener(TryDrop);
        _tryPlaceInputEvent?.AddListener(TryPlace);
        GameEventEnum.OnDayEnded.AddListener((eventInfo) => { _isNightTime = true; });
        GameEventEnum.OnNightEnded.AddListener((eventInfo) => { _isNightTime = false; });
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
            Debug.Log(":)");
            ignoreLayer |= (int)LayerMaskEnum.Path;
        }
        Collider[] overlapColliders = Physics.OverlapBox(worldObjectBounds.center,
            localObjectBounds.extents - Vector3.one * OBJECT_SKIN_WIDTH, Quaternion.identity,
            ~ignoreLayer);

        if (overlapColliders.Length > 0)
        {
            _canPlaceObject = false;
        }

        if (_currentPlacingMaterial)
        {
            _currentPlacingMaterial.color = _canPlaceObject ? _canPlaceColor : _cannotPlaceColor;
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
        
        _drawObject.SetActive(true);
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
        
        var currentPlaceable = _currentHoldable.GetPlaceable();
        currentPlaceable.Place(_drawObject.transform.position, _placeRotation);
        if (currentPlaceable.DestroyItemOnPlaced)
        {
            Destroy(_currentHoldable.GetGameObject());
            Reset();
        }

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
