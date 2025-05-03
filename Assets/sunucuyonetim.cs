using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class sunucuyonetim : MonoBehaviourPunCallbacks
{

    
    public Text serverbilgi;
    public InputField kulad;
    public InputField odaadi;
    string Kullanıcıadi;
    string OdaAdi;
    


    void Start()
    {
        
        
       PhotonNetwork.ConnectUsingSettings();

        /*  if(PhotonNetwork.IsConnected)
          {
              serverbilgi.text = "Servere Bağlandı";

          }*/

        DontDestroyOnLoad(gameObject);
       
    }
    public void OdaKur()
    {
        SceneManager.LoadScene(1);
        Kullanıcıadi = kulad.text;
        OdaAdi = odaadi.text;
        PhotonNetwork.JoinLobby();     

    }
    public void GirisYap()
    {
        SceneManager.LoadScene(1);
        Kullanıcıadi = kulad.text;
        OdaAdi = odaadi.text;
        PhotonNetwork.JoinLobby();    

    }
    public override void OnConnectedToMaster()
    {
        serverbilgi.text = "Servere Bağlandı";

    }
    public override void OnJoinedLobby()
     {
        if (Kullanıcıadi!="" && OdaAdi!="")
        {
            PhotonNetwork.JoinOrCreateRoom(OdaAdi, new RoomOptions { MaxPlayers = 2, IsOpen = true, IsVisible = true }, TypedLobby.Default);

        }else
        {
            PhotonNetwork.JoinRandomRoom();
        }
        
    }
    public override void OnJoinedRoom() 
    {
        InvokeRepeating("BilgiKontrolEt", 0, 1f);
        GameObject objem = PhotonNetwork.Instantiate("Oyuncu", Vector3.zero, Quaternion.identity, 0, null);        
        objem.GetComponent<PhotonView>().Owner.NickName = Kullanıcıadi;
        
    }
    public override void OnLeftRoom()
    {
        Debug.Log("Odadan çıkıldı");

    }
    public override void OnLeftLobby()
    {
        Debug.Log("Lobiden çıkıldı");

    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
      
        InvokeRepeating("BilgiKontrolEt", 0, 1f);
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        // herhangi bir oyuncu girdiğinde tetiklenen fonksiyondur.
    }


    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("Herhangi bir odaya girilemedi");

    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Rastgele bir odaya girilemedi");

    }
    public override void OnCreateRoomFailed(short returnCode, string message)
      {
        Debug.Log("Oda oluşturulamadı");

    }

    void BilgiKontrolEt()
    {

        if (PhotonNetwork.PlayerList.Length == 2)
        {
            GameObject.FindWithTag("OyuncuBekleniyor").GetComponent<TextMeshProUGUI>().text = "";
            GameObject.FindWithTag("Oyuncu_1").GetComponent<TextMeshProUGUI>().text = PhotonNetwork.PlayerList[0].NickName;
            GameObject.FindWithTag("Oyuncu_2").GetComponent<TextMeshProUGUI>().text = PhotonNetwork.PlayerList[1].NickName;
            CancelInvoke("BilgiKontrolEt");

        }else
        {
            // GameObject.Find("OyuncuBekleniyor").GetComponent<TextMeshProUGUI>().text= "8888Oyuncu Bekleniyor";
            GameObject.FindWithTag("OyuncuBekleniyor").GetComponent<TextMeshProUGUI>().text = "Oyuncu Bekleniyor";
            GameObject.FindWithTag("Oyuncu_1").GetComponent<TextMeshProUGUI>().text = PhotonNetwork.PlayerList[0].NickName;
            GameObject.FindWithTag("Oyuncu_2").GetComponent<TextMeshProUGUI>().text = ".....";
            
        }

        
    }
    // Update is called once per frame
    void Update()
    {

        
    }
}
