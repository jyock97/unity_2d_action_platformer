using UnityEngine;

/// <summary>
/// Component that manages the Camera Data
/// Data:
///     cameraLimitBounds -> the Bounds where the camera can move in
///     didCameraMoveX -> true if the camera move on the X axis, false otherwise
/// </summary>
public class CameraController : MonoBehaviour
{
    [SerializeField] private float speed;

    private GameObject _player;
    private Camera _camera;
    private GameObject[] _cameraBounds;
    private Bounds _cameraLimitBounds;
    
    private void OnValidate()
    {
        SetCameraBoundPoints();
        CalculateCameraBounds();
    }
    
    private void Start()
    {
        _camera = GetComponent<Camera>();
        _player = GameObject.FindWithTag("Player");

        SetCameraBoundPoints();
    }

    private void Update()
    {
        CalculateCameraBounds();
        Move();
    }

    /// <summary>
    /// Calculate the initial position for the bound points.
    /// </summary>
    private void SetCameraBoundPoints()
    {
        _cameraBounds = GameObject.FindGameObjectsWithTag("CameraBound");
    }

    /// <summary>
    /// Search for all the Camera Bound Objects and calculate a new Bounds
    /// that encapsulate all of them
    /// </summary>
    private void CalculateCameraBounds()
    {
        _cameraLimitBounds = new Bounds();
        _cameraLimitBounds.size = Vector3.one * 2;
        
        foreach (GameObject bound in _cameraBounds)
        {
            _cameraLimitBounds.Encapsulate(bound.transform.position);
        }

        Vector3 newCenter = _cameraLimitBounds.center;
        newCenter.z = transform.position.z;
        _cameraLimitBounds.center = newCenter;
    }

    private void Move()
    {
        Vector3 newPosition = Vector3.Lerp(transform.position, _player.transform.position, speed * Time.deltaTime);
        Vector3 diffPosition = newPosition - transform.position;
        newPosition.z = transform.position.z;
        diffPosition.z = 0;
        
        // Getting all the points from the camera for the newPosition of the camera
        Vector3 p1 = _camera.ViewportToWorldPoint(new Vector3(0, 0, _camera.nearClipPlane)) + diffPosition;
        Vector3 p2 = _camera.ViewportToWorldPoint(new Vector3(0, 1, _camera.nearClipPlane)) + diffPosition;
        Vector3 p3 = _camera.ViewportToWorldPoint(new Vector3(1, 0, _camera.nearClipPlane)) + diffPosition;
        Vector3 p4 = _camera.ViewportToWorldPoint(new Vector3(1, 1, _camera.nearClipPlane)) + diffPosition;

        // Check for each point if it is in the limit Bounds
        bool isP1InCameraBounds = _cameraLimitBounds.Contains(p1);
        bool isP2InCameraBounds = _cameraLimitBounds.Contains(p2);
        bool isP3InCameraBounds = _cameraLimitBounds.Contains(p3);
        bool isP4InCameraBounds = _cameraLimitBounds.Contains(p4);
        
        if (diffPosition.x > 0.001f)
        {
            if (!isP3InCameraBounds && !isP4InCameraBounds)
            {
                // Don't move in X axis if already get to the limit
                newPosition.x = transform.position.x;
            }
        } else if (diffPosition.x < -0.001f)
        {
            // Don't move in X axis if already get to the limit
            if (!isP1InCameraBounds && !isP2InCameraBounds)
            {
                newPosition.x = transform.position.x;
            }
        }
        
        if (diffPosition.y > 0.001f)
        {
            // Don't move in Y axis if already get to the limit
            if (!isP2InCameraBounds && !isP4InCameraBounds)
            {
                newPosition.y = transform.position.y;
            }
        } else if (diffPosition.y < -0.001f)
        {
            // Don't move in Y axis if already get to the limit
            if (!isP1InCameraBounds && !isP3InCameraBounds)
            {
                newPosition.y = transform.position.y;
            }
        }

        Vector2 currentPosition = transform.position;
        currentPosition.y = newPosition.y;
        transform.position = newPosition;
    }
    
    private void OnDrawGizmos()
    {
        CalculateCameraBounds();
        Gizmos.DrawWireCube(_cameraLimitBounds.center, _cameraLimitBounds.size);
    }
}
