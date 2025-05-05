using Photon.Pun;
using UnityEngine;

public class oyuncu : MonoBehaviour
{
    PhotonView pw;
    public int sagl�k;
    public GameObject[] Noktalar;
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
        }
        else
        {
            transform.position = Noktalar[1 ].transform.position;
        }
       
    }

    // Update is called once per frame
    void Update()
    {
        if (pw.IsMine)
        {
            Hareket();
            Z�pla();
            AtesEt();
        }
        
    }

    void AtesEt()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit))
            {
                if (hit.collider.CompareTag("Dusman"))
                {
                    hit.collider.GetComponent<PhotonView>().RPC("HasarAl", RpcTarget.All, 10);
                }
            }
        }
    }

    void Hareket()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * Input.GetAxis("Vertical") * 20f);
        transform.Translate(Vector3.right * Time.deltaTime * Input.GetAxis("Horizontal") * 20f);
    }

    void Z�pla()
    { 
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GetComponent<Rigidbody>().AddForce(Vector3.up * 5, ForceMode.Impulse);
        }
    }

    [PunRPC]
    void HasarAl(int darbegucu)
    {
        sagl�k -= darbegucu;
        Debug.Log("Hasar ald�m: " + sagl�k);
        if (sagl�k <= 0)
        {
            PhotonNetwork.Destroy(gameObject);
        }
       
    }
}
