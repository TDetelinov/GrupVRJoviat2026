using UnityEngine;

public class CubeEditManager : MonoBehaviour
{
    [Header("Referencias de mandos")]
    public Transform leftController;
    public Transform rightController;

    [Header("Parámetros de edición")]
    public float scaleSpeed = 0.5f;
    public float rotationSpeed = 90f;

    private GameObject currentCube;
    private bool editMode = false;

    void Update()
    {
        HandleCreateAndEditToggle();
        
        if (!editMode || currentCube == null)
            return;

        HandleScaling();
        HandleRotation();
        HandleReposition();
    }

    // ---------------------------------
    // BOTÓN A → CREAR / SALIR DE EDICIÓN
    // ---------------------------------
    void HandleCreateAndEditToggle()
    {
        if (OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.RTouch))
        {
            if (!editMode)
            {
                CreateCube();
                editMode = true;
                Debug.Log("🟢 MODO EDICIÓN ACTIVADO");
            }
            else
            {
                editMode = false;
                Debug.Log("🔴 MODO EDICIÓN DESACTIVADO");
            }
        }
    }

    void CreateCube()
    {
        currentCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        currentCube.transform.position = rightController.position + rightController.forward * 0.3f;
        currentCube.transform.localScale = Vector3.one * 0.1f;
    }

    // --------------------
    // JOYSTICK DERECHO → ESCALA
    // --------------------
    void HandleScaling()
    {
        Vector2 scaleInput = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);

        if (Mathf.Abs(scaleInput.y) > 0.1f)
        {
            float scaleAmount = scaleInput.y * scaleSpeed * Time.deltaTime;
            currentCube.transform.localScale += Vector3.one * scaleAmount;
        }
    }

    // --------------------
    // JOYSTICK IZQUIERDO → ROTACIÓN
    // --------------------
    void HandleRotation()
    {
        Vector2 rotateInput = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);

        if (rotateInput.magnitude < 0.2f)
            return;

        float absX = Mathf.Abs(rotateInput.x);
        float absY = Mathf.Abs(rotateInput.y);

        if (absX > absY)
        {
            // Rotación horizontal (Yaw)
            currentCube.transform.Rotate(Vector3.up, rotateInput.x * rotationSpeed * Time.deltaTime, Space.World);
        }
        else
        {
            // Rotación vertical (Pitch)
            currentCube.transform.Rotate(Vector3.right, -rotateInput.y * rotationSpeed * Time.deltaTime, Space.World);
        }
    }

    // --------------------
    // BOTÓN B → REPOSICIONAR
    // --------------------
    void HandleReposition()
    {
        if (OVRInput.GetDown(OVRInput.Button.Two, OVRInput.Controller.RTouch))
        {
            currentCube.transform.position = leftController.position;
            Debug.Log("📍 Cubo reposicionado en el mando izquierdo");
        }
    }
}
