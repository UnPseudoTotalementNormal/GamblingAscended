using System;
using System.Collections.Generic;
using Enums;
using Extensions;
using GameEvents;
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
    
    private GameObject _drawObject;

    private const float OBJECT_SKIN_WIDTH = 0.01f;


    private void Awake()
    {
        _interacterRaycaster = GetComponent<GL_InteracterRaycaster>();
        _tryPickupEvent?.AddListener(OnTryPickup);
        _interactInputEvent?.AddListener(TryDrop);
    }

    private void Update()
    {
        if (_currentHoldable != null)
        {
            DrawPreview(_currentHoldable.GetGameObject());
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

        bool canPlaceObject = true;

        _drawObject.transform.rotation = Quaternion.identity;
        Bounds objectBounds = _drawObject.GetCollidersBounds();
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hitInfo, _dropMaxDistance,
                ~((int)LayerMaskEnum.Character | (int)LayerMaskEnum.Item | (int)LayerMaskEnum.IgnoreRaycast)))
        {
            _drawObject.transform.position = hitInfo.point + hitInfo.normal *
                (Vector3.Dot(hitInfo.normal.Abs(), objectBounds.extents.Abs()));
            if (!hitInfo.collider.gameObject.TryGetComponentInParents(out GL_IPlaceableGround placeableGround))
            {
                canPlaceObject = false;
            }
        }
        else
        {
            _drawObject.transform.position = transform.position + transform.forward * _dropMaxDistance -
                                             (objectBounds.extents.z * transform.forward);
            canPlaceObject = false;
        }
        
        Collider[] overlapColliders = Physics.OverlapBox(_drawObject.transform.position,
            objectBounds.extents - Vector3.one * OBJECT_SKIN_WIDTH, Quaternion.identity,
            ~((int)LayerMaskEnum.Character | (int)LayerMaskEnum.Item | (int)LayerMaskEnum.IgnoreRaycast));


        if (_currentPlacingMaterial)
        {
            _currentPlacingMaterial.color = canPlaceObject ? _canPlaceColor : _cannotPlaceColor;
        }
        
        
        if (!spawnedObject)
        {
            return;
        }
        
        SetPlacingMaterials(_drawObject);

        var colliders = _drawObject.GetComponentsInChildren<Collider>();
        foreach (Collider collider in colliders)
        {
            //collider.enabled = false;
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
        droppedObject.transform.position = _drawObject.transform.position;
        droppedObject.transform.eulerAngles = new Vector3(0, _drawObject.transform.eulerAngles.y);
        
        _currentHoldable.OnDropped();
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
