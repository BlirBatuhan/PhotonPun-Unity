using Photon.Pun;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class oyuncu : MonoBehaviour
{
    PhotonView pw;
    public int saglýk;
    public GameObject[] Noktalar;
    int hedefOyuncu;
    void Start()
    {
        pw = GetComponent<PhotonView>();
        if(pw.IsMine)
        {
            GetComponent<Renderer>().material.color = Color.yellow;
        }

        if(PhotonNetwork.IsMasterClient)
        {
            transform.position = Noktalar[0].transform.position;
            GameObject.FindWithTag("Oyuncu1_isim").GetComponent<TextMeshProUGUI>().text = saglýk.ToString();
            hedefOyuncu = 1;
        }
        else
        {
            transform.position = Noktalar[1 ].transform.position;
            GameObject.FindWithTag("Oyuncu2_isim").GetComponent<TextMeshProUGUI>().text = saglýk.ToString();
            hedefOyuncu = 0;
        }
       
    }

    // Update is called once per frame
    void Update()
    {
        if (pw.IsMine)
        {
            Hareket();
            Zýpla();
            
        }
        
    }

   /* void AtesEt()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit))
            {
                if (hit.collider.CompareTag("Dusman"))
                {
                    hit.collider.GetComponent<PhotonView>().RPC("CanAzalt", RpcTarget.All, hedefOyuncu);
                }
            }
        }
    }*/

    void Hareket()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * Input.GetAxis("Vertical") * 20f);
        transform.Translate(Vector3.right * Time.deltaTime * Input.GetAxis("Horizontal") * 20f);
    }

    void Zýpla()
    { 
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GetComponent<Rigidbody>().AddForce(Vector3.up * 5, ForceMode.Impulse);
        }
    }

    [PunRPC]
    public void HasarAl(int darbegucu)
    {
        saglýk -= darbegucu;
        Debug.Log("Hasar aldým: " + saglýk);
        if (saglýk <= 0)
        {
            PhotonNetwork.Destroy(gameObject);
        }
       
    }

    void CanAzalt()
    {
        saglýk--;

            GameObject.FindWithTag("SunucuYonetimi").GetComponent<sunucuyonetim>().SkoruGuncelle(hedefOyuncu, saglýk);
    
    }
}
