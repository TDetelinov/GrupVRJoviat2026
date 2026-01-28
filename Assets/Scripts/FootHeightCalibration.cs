using System.Collections;
using UnityEngine;
using TMPro;

public class FootHeightCalibration : MonoBehaviour
{
    [Header("Referencias")]
    public Transform leftController;
    public Transform rightController;

    public GameObject cubePrefab;
    public TextMeshPro countdownText;

    [Header("Configuración")]
    public float countdownTime = 5f;
    public float groundY = 0f;

    private bool calibrationDone = false;

    private GameObject leftCube;
    private GameObject rightCube;

    private float leftFootHeight;
    private float rightFootHeight;

    void OnTriggerEnter(Collider other)
    {
        if (calibrationDone) return;

        if (other.CompareTag("Player"))
        {
            StartCoroutine(CalibrationRoutine());
        }
    }

    IEnumerator CalibrationRoutine()
    {
        float time = countdownTime;
        countdownText.gameObject.SetActive(true);

        while (time > 0)
        {
            countdownText.text = Mathf.Ceil(time).ToString();
            yield return new WaitForSeconds(1f);
            time--;
        }

        countdownText.text = "0";
        yield return new WaitForSeconds(0.2f);
        countdownText.gameObject.SetActive(false);

        CalculateHeights();
        SpawnReferenceCubes();

        calibrationDone = true;
    }

    void CalculateHeights()
    {
        leftFootHeight = leftController.position.y - groundY;
        rightFootHeight = rightController.position.y - groundY;

        Debug.Log($"Altura pie izquierdo: {leftFootHeight:F3}");
        Debug.Log($"Altura pie derecho: {rightFootHeight:F3}");
    }

    void SpawnReferenceCubes()
    {
        leftCube = Instantiate(cubePrefab);
        rightCube = Instantiate(cubePrefab);

        leftCube.name = "LeftFootReference";
        rightCube.name = "RightFootReference";

        // Colocamos a la altura calculada
        leftCube.transform.position = new Vector3(
            leftController.position.x,
            leftFootHeight,
            leftController.position.z
        );

        rightCube.transform.position = new Vector3(
            rightController.position.x,
            rightFootHeight,
            rightController.position.z
        );

        // Añadimos el script de seguimiento
        leftCube.AddComponent<FollowXZ>().target = leftController;
        rightCube.AddComponent<FollowXZ>().target = rightController;
    }
}
