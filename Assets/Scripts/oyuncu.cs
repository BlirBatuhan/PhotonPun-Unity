using Photon.Pun;
using TMPro;
using UnityEngine;

public class oyuncu : MonoBehaviourPunCallbacks
{
    PhotonView pw;
    // Efekt ��kmas�n� istemedi�in layer'lar (�rn. g�r�nmez collider layer'�)
    public LayerMask ignoreLayers;
    public float menzil;

    [Header("SESLER")]
    public AudioSource[] Sesler;

    [Header("Karakter")]
    public int saglik = 100;

    [Header("Pozisyon Noktalari")]
    public GameObject[] Noktalar;

    [Header("Hareket")]
    public CharacterController controller;
    public float speed = 5f;
    public float gravity = -9.81f;
    public float jumpHeight = 2f;

    Vector3 velocity;
    bool isGrounded;

    // Silah etkisi i�in referans
    public ParticleSystem[] Efektler;  // Efektleri oyuncuya ba�layabilirsin

    public int DarbeGucu = 20;

    void Start()
    {
        pw = GetComponent<PhotonView>();

        if (pw.IsMine)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                transform.position = Noktalar[0].transform.position;
            }
            else
            {
                transform.position = Noktalar[1].transform.position;
            }
            Debug.Log($"Benim karakterim spawn edildi: {PhotonNetwork.LocalPlayer.NickName}");
        }
        else
        {
            Debug.Log($"Ba�ka oyuncunun karakteri g�r�ld�: {pw.Owner.NickName}");
        }
    }

    void Update()
    {
        if (!pw.IsMine) return;
        Hareket();
        Ziplama();
    }

    void Hareket()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move.normalized * speed * Time.deltaTime);
    }

    void Ziplama()
    {
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        if (Input.GetButtonDown("Jump") && controller.isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }

    [PunRPC]
    public void HasarAl(int darbeGucu)
    {
        saglik -= darbeGucu;
        Debug.Log("Hasar ald�m: " + saglik);

        if (saglik <= 0)
        {
            if (pw.IsMine)
                PhotonNetwork.Destroy(gameObject);
        }
    }

    [PunRPC]
    void RPC_AtesEt(Vector3 rayOrigin, Vector3 rayDirection)
    {
        // Ses �al
        Sesler[0].Play();

        // RaycastAll ile t�m �arp��malar� al�yoruz
        RaycastHit[] hits = Physics.RaycastAll(rayOrigin, rayDirection, menzil);

        foreach (var hit in hits)
        {
            // E�er obje, ignoreLayers i�indeyse devam et, efekt verme
            if (((1 << hit.collider.gameObject.layer) & ignoreLayers) != 0)
                continue;

            if (hit.transform.CompareTag("Player"))
            {
                if (hit.transform.GetComponent<PhotonView>().IsMine)
                {
                    hit.transform.GetComponent<oyuncu>().HasarAl(DarbeGucu);
                }

                Instantiate(Efektler[0], hit.point, Quaternion.LookRotation(hit.normal));
            }
            else
            {
                Instantiate(Efektler[1], hit.point, Quaternion.LookRotation(hit.normal));
            }

            // �lk ger�ek objede durmak istiyorsan buraya break koyabilirsin
            break;
        }
    }
}
