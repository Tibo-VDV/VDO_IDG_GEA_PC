using UnityEngine;
using UnityEngine.InputSystem;

public class FPSCamRotation : MonoBehaviour
{
    [SerializeField] Transform cameraRef;
    Vector2 mouseDelta;
    [SerializeField] float mouseSensitivity = 1f;
    [SerializeField] Vector2 lookLimits = new Vector2(-90f, 90f);

    float tiltRotation = 0f;
    float yAxisRotation = 0f;

    void Start()
    {
        tiltRotation = cameraRef.rotation.eulerAngles.x;
        yAxisRotation = cameraRef.rotation.eulerAngles.y;
    }
    
    void Update()
    {
        #region MouseRotation

        Vector3 currentRotation = cameraRef.rotation.eulerAngles;
        float x, y;
        x = mouseDelta.x * mouseSensitivity * Time.deltaTime;
        y = mouseDelta.y * mouseSensitivity * Time.deltaTime;

        /*
        transform.Rotate(Vector3.up, x);        yt oplossing heeft problemen
        transform.Rotate(Vector3.left, y);
        */
        //vertical rotation
        tiltRotation -= y; // mouseDelta in Screenspace origin links boven. y as onderaan laten beginnen
        tiltRotation = Mathf.Clamp(tiltRotation, lookLimits.x, lookLimits.y); // we clampen de tiltrotation zodat we niet helemaal naar boven of beneden kunnen kijken.
        currentRotation.x = tiltRotation;
        
        //horizontal rotation
        yAxisRotation += x;
        currentRotation.y = yAxisRotation;
        currentRotation.z = 0; // we zetten de z rotatie op 0 zodat we niet kunnen "rollen" met onze camera.

        cameraRef.rotation = Quaternion.Euler(currentRotation);

        #endregion
    }
    
    void OnLook(InputValue context)
    {
        mouseDelta = context.Get<Vector2>();
    }
      
}
