using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public sealed class PlayerShipPresentation : MonoBehaviour
{
    [SerializeField, Min(1f)] private float turnSpeed = 12f;
    [SerializeField, Min(0f)] private float movementThreshold = 0.0025f;

    private Transform shipVisual;
    private TrailRenderer engineTrail;
    private Vector3 previousPosition;
    private float facingAngle;

    private void Awake()
    {
        SpriteRenderer rootRenderer = GetComponent<SpriteRenderer>();
        var visualRoot = new GameObject("Ship Visual");
        visualRoot.transform.SetParent(transform, false);
        shipVisual = visualRoot.transform;

        var visualRenderer = visualRoot.AddComponent<SpriteRenderer>();
        visualRenderer.sprite = rootRenderer.sprite;
        visualRenderer.color = Color.white;
        visualRenderer.sortingLayerID = rootRenderer.sortingLayerID;
        visualRenderer.sortingOrder = rootRenderer.sortingOrder;
        rootRenderer.enabled = false;

        var engineRoot = new GameObject("Engine Trail");
        engineRoot.transform.SetParent(shipVisual, false);
        engineRoot.transform.localPosition = new Vector3(0f, -0.34f, 0f);
        engineTrail = engineRoot.AddComponent<TrailRenderer>();
        engineTrail.sharedMaterial = ArenaPresentation.TrailMaterial;
        engineTrail.time = .18f;
        engineTrail.minVertexDistance = .04f;
        engineTrail.startWidth = .16f;
        engineTrail.endWidth = 0f;
        engineTrail.startColor = new Color(.2f, .8f, 1f, .85f);
        engineTrail.endColor = new Color(.1f, .45f, 1f, 0f);
        engineTrail.sortingOrder = visualRenderer.sortingOrder - 1;
        engineTrail.emitting = false;

        previousPosition = transform.position;
    }

    private void LateUpdate()
    {
        Vector3 movement = transform.position - previousPosition;
        previousPosition = transform.position;
        bool moving = movement.sqrMagnitude > movementThreshold * movementThreshold;

        if (moving)
        {
            facingAngle = Mathf.Atan2(movement.y, movement.x) *
                Mathf.Rad2Deg - 90f;
        }

        Quaternion targetRotation = Quaternion.Euler(0f, 0f, facingAngle);
        shipVisual.rotation = Quaternion.Slerp(
            shipVisual.rotation,
            targetRotation,
            1f - Mathf.Exp(-turnSpeed * Time.unscaledDeltaTime));
        engineTrail.emitting = moving && Time.timeScale > 0f;
    }
}
