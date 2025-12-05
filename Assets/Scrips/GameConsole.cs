using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameConsole : MonoBehaviour
{
    [Header("Referencias UI")]
    // Asegúrate de arrastrar el TextMeshPro (UI) aquí
    public TMP_Text textoConsola; 
    // Asegúrate de arrastrar el objeto Scroll View aquí
    public ScrollRect scrollRect; 

    [Header("Ajustes")]
    public int maxLineas = 50;

    private Queue<string> colaMensajes = new Queue<string>();

    // LOGS DE UNITY: Suscripción y Desuscripción

    // Se llama cuando el script se activa (al inicio)
    private void OnEnable()
    {
        // Suscribir el método HandleLog para capturar todos los logs de Unity
        Application.logMessageReceived += HandleLog;
    }

    // Se llama cuando el script se desactiva o se destruye
    private void OnDisable()
    {
        // Desuscribir es fundamental para evitar errores
        Application.logMessageReceived -= HandleLog;
    }

    // Manejador del Log: Redirige los logs de Unity a la consola del juego
    void HandleLog(string logString, string stackTrace, LogType type)
    {
        string tipoMensaje = "sistema"; // Color blanco por defecto

        switch (type)
        {
            case LogType.Error:
            case LogType.Exception:
                // Usamos 'enemigo' (rojo) para errores
                tipoMensaje = "enemigo"; 
                break;
            case LogType.Warning:
                // Usamos 'jugador' (verde) para advertencias
                tipoMensaje = "jugador"; 
                break;
            case LogType.Log:
            default:
                // Logs normales
                break;
        }
        
        // Formatear el mensaje, incluyendo el tipo de log de Unity
        Escribir($"[{type.ToString()}] {logString}", tipoMensaje);
    }

    // MÉTODO PÚBLICO → Escribir mensaje en la consola
    public void Escribir(string mensaje, string tipo = "sistema")
    {
        // Aplicar color según el tipo
        switch (tipo.ToLower())
        {
            case "jugador":
                mensaje = $"<color=#00FF00>{mensaje}</color>";
                break;

            case "enemigo":
                mensaje = $"<color=#FF4444>{mensaje}</color>";
                break;

            default:    // sistema
                mensaje = $"<color=#FFFFFF>{mensaje}</color>";
                break;
        }

        // Mantener máximo de líneas
        if (colaMensajes.Count >= maxLineas)
            colaMensajes.Dequeue();

        colaMensajes.Enqueue(mensaje);

        ActualizarTexto();
    }

    // Actualizar el texto mostrado en la consola
    private void ActualizarTexto()
    {
        // Esta comprobación es la que generaba tu error original
        if (textoConsola == null)
        {
            Debug.LogError("GameConsole: Falta asignar el campo 'textoConsola' en el Inspector.");
            return;
        }

        // Construir texto completo a partir de la cola de mensajes
        textoConsola.text = string.Join("\n", colaMensajes);

        // Forzar actualización de UI para asegurar el desplazamiento
        Canvas.ForceUpdateCanvases();

        // Auto scroll hacia abajo
        if (scrollRect != null)
            scrollRect.verticalNormalizedPosition = 0f;
    }

    // Mensaje inicial opcional
    private void Start()
    {
        Escribir("Se inicio la partida.", "sistema");
    }
}