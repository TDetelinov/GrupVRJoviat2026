using System.Collections.Generic;
using UnityEngine;

public class ContadorCrearSecuencia : MonoBehaviour
{
    [Header("Secuencia creada desde el Inspector")]
    [SerializeField] private List<int> numerosConfigurados = new List<int>();

    [Header("Comportamiento de parada")]
    [SerializeField] private int limiteNumero = 100;
    [SerializeField] private Color colorAlParar = Color.red;

    [Header("Depuración")]
    [SerializeField] private bool mostrarLogs = true;

    private readonly List<int> secuenciaCreada = new List<int>();
    public IReadOnlyList<int> SecuenciaCreada => secuenciaCreada;
    public bool EstaDetenido { get; private set; }

    private void Awake()
    {
        RecrearSecuenciaDesdeInspector();
    }

    [ContextMenu("Recrear secuencia desde inspector")]
    public void RecrearSecuenciaDesdeInspector()
    {
        secuenciaCreada.Clear();
        secuenciaCreada.AddRange(numerosConfigurados);

        if (mostrarLogs)
        {
            Debug.Log($"[ContadorCrearSecuencia] Secuencia creada: {string.Join(", ", secuenciaCreada)}");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        RevisarToken(other.gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        RevisarToken(collision.gameObject);
    }

    private void RevisarToken(GameObject token)
    {
        if (EstaDetenido)
        {
            return;
        }

        if (!TokenSequenceUtils.TryGetTokenNumber(token, out int numero))
        {
            return;
        }

        if (numero > limiteNumero)
        {
            EstaDetenido = true;
            TokenSequenceUtils.PaintAllTokens(colorAlParar);

            if (mostrarLogs)
            {
                Debug.Log($"[ContadorCrearSecuencia] Se detiene porque el token {token.name} tiene número {numero} (> {limiteNumero}).");
            }
        }
    }
}
