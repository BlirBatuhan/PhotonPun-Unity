using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using TMPro;


public class sunucuYönetimi : MonoBehaviourPunCallbacks
{
    public TextMeshProUGUI oyuncu1;
    public TextMeshProUGUI oyuncu2;
    public TextMeshProUGUI OyunBekleniyor;


    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        InvokeRepeating("İsimKontrolEt", 0, 1f);
    }
    public override void OnConnectedToMaster()
    {
        Debug.Log("Sunucuya bağlandım: " + PhotonNetwork.CloudRegion);
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
       PhotonNetwork.JoinOrCreateRoom("Oda1", new RoomOptions { MaxPlayers = 4 }, null);
        Debug.Log("Lobiye bağlandım");

    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Odaya bağlanıldı");
        string Ad = Random.Range(1, 100) + " Oyuncu";
        GameObject objem = PhotonNetwork.Instantiate("oyuncu", Vector3.zero, Quaternion.identity, 0);      
        objem.GetComponent<PhotonView>().Owner.NickName = Ad;
        oyuncu1.text = Ad;
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
       InvokeRepeating("İsimKontrolEt", 0, 1f);

    }
    void İsimKontrolEt()
    {       
        if(PhotonNetwork.PlayerList.Length == 2)
        {
            OyunBekleniyor.text = "Oyun Başlıyor";
            oyuncu1.text = PhotonNetwork.PlayerList[0].NickName;
            oyuncu2.text = PhotonNetwork.PlayerList[1].NickName;
            CancelInvoke("İsimKontrolEt");
        }
        else
        {
            OyunBekleniyor.text = "Oyuncu Bekleniyor";
            oyuncu1.text = PhotonNetwork.PlayerList[0].NickName;
            oyuncu2.text = ".....";
        }
      
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
