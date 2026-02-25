using System.Collections.Generic;
using UnityEngine;

public class ContadorSecuencia : MonoBehaviour
{
    [Header("Secuencia objetivo")]
    [SerializeField] private List<int> secuenciaObjetivo = new List<int> { 1, 2, 3, 4 };

    [Header("Acción al completar secuencia")]
    [SerializeField] private Color colorAlCompletar = Color.green;

    [Header("Depuración")]
    [SerializeField] private bool mostrarLogs = true;

    private readonly List<int> secuenciaLeida = new List<int>();
    public IReadOnlyList<int> SecuenciaLeida => secuenciaLeida;
    public bool SecuenciaCompletada { get; private set; }

    private void OnTriggerEnter(Collider other)
    {
        RegistrarToken(other.gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        RegistrarToken(collision.gameObject);
    }

    private void RegistrarToken(GameObject token)
    {
        if (SecuenciaCompletada)
        {
            return;
        }

        if (!TokenSequenceUtils.TryGetTokenNumber(token, out int numero))
        {
            return;
        }

        secuenciaLeida.Add(numero);

        if (mostrarLogs)
        {
            Debug.Log($"[ContadorSecuencia] Secuencia leída: {string.Join(", ", secuenciaLeida)}");
        }

        if (TokenSequenceUtils.SequenceMatchesTail(secuenciaLeida, secuenciaObjetivo))
        {
            SecuenciaCompletada = true;
            TokenSequenceUtils.PaintAllTokens(colorAlCompletar);

            if (mostrarLogs)
            {
                Debug.Log("[ContadorSecuencia] Secuencia objetivo completada. Se colorean todos los tokens.");
            }
        }
    }
}
