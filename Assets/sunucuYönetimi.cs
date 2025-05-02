using UnityEngine;
using Photon.Realtime;
using Photon.Pun;


public class sunucuYönetimi : MonoBehaviourPunCallbacks
{

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
        GameObject objem = PhotonNetwork.Instantiate("oyuncu", Vector3.zero, Quaternion.identity, 0);
        objem.GetComponent<PhotonView>().Owner.NickName = "Batuhan";
    }
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
