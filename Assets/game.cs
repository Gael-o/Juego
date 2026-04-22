using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using TMPro; // CAMBIO: Necesario para usar TextMeshPro

public class game : MonoBehaviour {
    public int puntos = 0;
    public string idUsuario = "1";
    public TextMeshProUGUI textoPuntos; // CAMBIO: Esta es la referencia al texto en pantalla

    void Update() {
        float mov = Input.GetAxis("Horizontal") * Time.deltaTime * 5;
        transform.Translate(mov, 0, 0);

        if (transform.position.x > 3f) { 
            puntos++; 
            transform.position = Vector3.zero;
            // CAMBIO: Actualizamos el texto en pantalla
            if(textoPuntos != null) textoPuntos.text = "Puntos: " + puntos; 
            Debug.Log("Puntos: " + puntos);
        } 
        else if (transform.position.x < -3f) {
            puntos--;
            transform.position = Vector3.zero;
            // CAMBIO: Actualizamos el texto en pantalla
            if(textoPuntos != null) textoPuntos.text = "Puntos: " + puntos;
            Debug.Log("Puntos: " + puntos);
        }

        if (Input.GetKeyDown(KeyCode.Space)) {
            StartCoroutine(EnviarDatos());
        }
    }

    IEnumerator EnviarDatos() {
        string json = "{\"userId\":" + idUsuario + ", \"score\":" + puntos + "}";
        using (UnityWebRequest www = new UnityWebRequest("http://localhost:8080/update-score", "POST")) {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");
            yield return www.SendWebRequest();
            if (www.result == UnityWebRequest.Result.Success) Debug.Log("¡Base de Datos Actualizada!");
            else Debug.Log("Error: " + www.error);
        }
    }
}