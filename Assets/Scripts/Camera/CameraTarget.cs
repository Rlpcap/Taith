using UnityEngine;

public class CameraTarget : MonoBehaviour, IUpdate
{
    public Vector3 CameraForward => Vector3.ProjectOnPlane(transform.forward, Vector3.up).normalized;
    public Vector3 TargetPosition => GetPosition();
    public GameObject character;
    public float heightY = 2f;

    [Header("Settings")]
    public float mouseSensitivity = 1f;
    public float maxCameraDistance = 10f;

    [Header("Collisions")] 
    public LayerMask collisionMask;
    public bool usePercentualCameraDisplacement = true;
    public float fixedDisplacement = 0.5f;
    [Range(0f, 1f)] public float percentualDisplacement = 0.1f;

    private Vector3 _targetPosition = Vector3.zero;

    public static bool isLocked = false;

    void Start()
    {
        UpdateManager.Instance.AddElementUpdate(this);
    }

    public void OnUpdate()
    {
        transform.position = new Vector3(character.transform.position.x, character.transform.position.y + heightY, character.transform.position.z);

    }

    private Vector3 GetPosition()
    {
        var mouseX = Input.GetAxis("Mouse X");
        var mouseY = Input.GetAxis("Mouse Y");
        
        if(!isLocked)
            Rotate(mouseX,mouseY);

        if (Physics.Raycast(transform.position, -transform.forward, out var hit, maxCameraDistance, collisionMask))
        {
            if (usePercentualCameraDisplacement) //¿Uso una distancia porcentual o una fija?
            {
                _targetPosition = Vector3.Lerp(hit.point, transform.position, percentualDisplacement);
            }
            else
            {
                var temp = (transform.position - hit.point).normalized;
                _targetPosition = hit.point + temp * fixedDisplacement;
            }
        }
        else //Si no le pega a nada el raycast, calculo normal
        {
            _targetPosition = transform.position - transform.forward * maxCameraDistance;
        }

        return _targetPosition;
    }


    private void Rotate(float inputX, float inputY)
    {
        var inputWithSensitivity = new Vector3(-inputY * mouseSensitivity, inputX * mouseSensitivity, 0);
        var newRotation = transform.rotation.eulerAngles + inputWithSensitivity;

        var newX = ClampRotation(newRotation.x);
        transform.rotation = Quaternion.Euler(newX, newRotation.y, newRotation.z);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(_targetPosition, 0.2f);
        Gizmos.DrawRay(transform.position, -transform.forward * maxCameraDistance);

        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, CameraForward);
    }

    private float ClampRotation(float f)
    {
        if (f > 360f) //Acerco el valor a entre 0 y 360
        {
            f -= 360f;
        }
        else if (f < 0f)
        {
            f += 360;
        }

        if (f > 80 && f < 150) //Clampeo el valor y snapeo si se pasa
            f = 80;
        else if (f < 300 && f >= 150)
            f = 300f;

        return f;
    }

}
