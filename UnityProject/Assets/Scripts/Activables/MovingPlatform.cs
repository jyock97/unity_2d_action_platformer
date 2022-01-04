using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : Activable
{
    [SerializeField] private float movementWaitTime;
    [SerializeField] private float movementSpeed;
    [SerializeField] private bool shouldMove;
    [Header("Debug")]
    [SerializeField] private bool calculatePivots;

    
    private List<Vector2> _pivotPositions;
    private Vector2 _currentPivot;
    private int _currentPivotIndex;
    private float _currentWaitTime;

    private void OnValidate()
    {
        if (calculatePivots || _pivotPositions == null)
        {
            CalculatePivots();
            calculatePivots = false;
        }
    }

    private void Start()
    {
        CalculatePivots();
    }

    private void Update()
    {
        if (!shouldMove)
        {
            return;
        }
        
        if (Vector2.Distance(transform.position, _currentPivot) > 0.1f)
        {
            if (Time.time > _currentWaitTime)
            {
                transform.position = Vector2.MoveTowards(transform.position, _currentPivot, movementSpeed * Time.deltaTime);
            }
        }
        else
        {
            _currentWaitTime = Time.time + movementWaitTime;
            _currentPivotIndex = (_currentPivotIndex + 1) % _pivotPositions.Count;
            _currentPivot = _pivotPositions[_currentPivotIndex];
        }

#if UNITY_EDITOR
        if (calculatePivots)
        {
            CalculatePivots();
            calculatePivots = false;
        }
#endif
    }

    public override void Activate()
    {
        shouldMove = true;
    }

    private void CalculatePivots()
    {
        Transform pivots = transform.Find("Pivots");

        if (pivots != null)
        {
            _pivotPositions = new List<Vector2>();
            foreach (Transform t in transform.Find("Pivots"))
            {
                _pivotPositions.Add(t.position);
            }

            _currentPivotIndex = 0;
            _currentPivot = _pivotPositions[_currentPivotIndex];
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag(TagsLayers.PlayerTag))
        {
            other.transform.SetParent(transform);
        }
    }
    
    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag(TagsLayers.PlayerTag))
        {
            other.transform.SetParent(null);
        }
    }

    private void OnDrawGizmos()
    {
        Color color = Color.green;
        color.a = 0.5f;
        Gizmos.color = color;
        
        Vector2 prevPivot = _pivotPositions[0];
        Gizmos.DrawSphere(prevPivot, 0.2f);
        for (int i = 1; i < _pivotPositions.Count; i++)
        {
            Gizmos.DrawSphere(_pivotPositions[i], 0.2f);
            Gizmos.DrawLine(prevPivot, _pivotPositions[i]);
            prevPivot = _pivotPositions[i];
        }
        Gizmos.DrawSphere(_currentPivot, 0.3f);
    }
}
