using UnityEngine;

public class DoorControl : MonoBehaviour
{

    public Transform[] doors;           // Kap�n�n kendisi
    public Vector3 openPosition1;
    public Vector3 openPosition2;  // A��k konum (lokal pozisyon fark�)
    public float openSpeed = 2f;     // A��lma h�z�
    private Vector3 closedPosition1;
    private Vector3 closedPosition2;// Kapal� pozisyon
    private bool isOpening = false;  // Kap� a��l�yor mu

    AudioSource SesCal; // Kap� sesi

    void Start()
    {
        SesCal = doors[0].GetComponent<AudioSource>();
        closedPosition1 = doors[0].localPosition;
        closedPosition2 = doors[1].localPosition;
    }

    void Update()
    {
        if (isOpening)
        {
            doors[0].localPosition = Vector3.Lerp(doors[0].localPosition, openPosition1, Time.deltaTime * openSpeed);
            doors[1].localPosition = Vector3.Lerp(doors[1].localPosition, openPosition2, Time.deltaTime * openSpeed);
        }
        else
        {
            doors[0].localPosition = Vector3.Lerp(doors[0].localPosition, closedPosition1, Time.deltaTime * openSpeed);
            doors[1].localPosition = Vector3.Lerp(doors[1].localPosition, closedPosition2, Time.deltaTime * openSpeed);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Oyuncu etiketi
        {
            isOpening = true;
            if (!SesCal.isPlaying) // Ses �alm�yorsa
            {
                SesCal.Play(); // Kap� sesi �al
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isOpening = false;
            SesCal.Play();
        }
    }
}
