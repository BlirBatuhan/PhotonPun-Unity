﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class sunucuyonetim : MonoBehaviourPunCallbacks
{

    public Text serverbilgi;
    public InputField kulad;
    public InputField odaadi;
    string Kullanıcıadi;
    string OdaAdi;
    bool odayaGirildimi = false;
    bool hasInstatiated = false;



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
        Kullanıcıadi = kulad.text;
        OdaAdi = odaadi.text;
        odayaGirildimi = true;
        PhotonNetwork.JoinLobby();
    }
    public void GirisYap()
    {
        Kullanıcıadi = kulad.text;
        OdaAdi = odaadi.text;
        odayaGirildimi = true;
        PhotonNetwork.JoinLobby();
    }
    public override void OnConnectedToMaster()
    {
        serverbilgi.text = "Servere Bağlandı";
        PhotonNetwork.JoinLobby();

    }
    public override void OnJoinedLobby()
    {
        if (!odayaGirildimi)
            return;

        bool bilgilerGecerli = !string.IsNullOrEmpty(Kullanıcıadi) && !string.IsNullOrEmpty(OdaAdi);

        if (bilgilerGecerli)
        {

            StartCoroutine(OdaKurGecikmeli());
        }
        else
        {
            PhotonNetwork.JoinRandomRoom();
        }
    }



    IEnumerator OdaKurGecikmeli()
    {
        yield return new WaitForSeconds(5f); // 🟢 Diğer oyuncuların listeyi güncellemesi için zaman tanır
        PhotonNetwork.JoinOrCreateRoom(OdaAdi, new RoomOptions { MaxPlayers = 2, IsOpen = true, IsVisible = true }, TypedLobby.Default);
    }
    public override void OnJoinedRoom()
    {

        SceneManager.LoadScene(1);

        if (!hasInstatiated)
        {
            hasInstatiated = true;
            StartCoroutine(InstantiateAfterSceneLoad());
        }
        
    }

    IEnumerator InstantiateAfterSceneLoad()
    {
        yield return new WaitForSeconds(0.5f);
        GameObject objem = PhotonNetwork.Instantiate("Varlık", Vector3.zero, Quaternion.identity);

        if (objem != null && objem.GetComponent<PhotonView>() != null)
        {
            objem.GetComponent<PhotonView>().Owner.NickName = Kullanıcıadi;
            Debug.Log("NickName atandı: " + Kullanıcıadi);
            Debug.Log("Oyuncu sayısı: " + GameObject.FindGameObjectsWithTag("Player").Length);
        }
        InvokeRepeating("BilgiKontrolEt", 0, 1f);
    }

    public override void OnLeftRoom()
    {
        Debug.Log("Odadan çıkıldı");

    }
    public override void OnLeftLobby()
    {
        Debug.Log("Lobiden çıkıldı");

    }

    public void JoinRoomInList(string OdaAdı)
    {
        Kullanıcıadi = kulad.text;
        PhotonNetwork.JoinOrCreateRoom(OdaAdı, new RoomOptions { MaxPlayers = 2, IsOpen = true, IsVisible = true }, TypedLobby.Default);
        
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

        }
        else
        {
            // GameObject.Find("OyuncuBekleniyor").GetComponent<TextMeshProUGUI>().text= "8888Oyuncu Bekleniyor";
            GameObject.FindWithTag("OyuncuBekleniyor").GetComponent<TextMeshProUGUI>().text = "Oyuncu Bekleniyor";
            GameObject.FindWithTag("Oyuncu_1").GetComponent<TextMeshProUGUI>().text = PhotonNetwork.PlayerList[0].NickName;
            GameObject.FindWithTag("Oyuncu_2").GetComponent<TextMeshProUGUI>().text = ".....";

        }

        
    }


    public void SkoruGuncelle(int oyuncu_sırası, int sayı_nedir)
    {
        switch (oyuncu_sırası)
        {
            case 0:
                GameObject.FindWithTag("Oyuncu1_isim").GetComponent<TextMeshProUGUI>().text = sayı_nedir.ToString();
                break;
            case 1:
                GameObject.FindWithTag("Oyuncu2_isim").GetComponent<TextMeshProUGUI>().text = sayı_nedir.ToString();
                break;
        }

        if (sayı_nedir <= 0)
        {
            if (oyuncu_sırası == 0)
            {
                //2. oyuncu kazandı
                //Asagidaki kod sahnede olsun ya da olmasın bütün gameobjelerı getirir gameobject tipinde cast ederek
                foreach (GameObject objem in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[])
                {
                    if (objem.gameObject.CompareTag("KazanmaPaneli"))
                    {
                        objem.gameObject.SetActive(true);
                        GameObject.FindWithTag("KazananKisi").GetComponent<TextMeshProUGUI>().text = "2. Oyuncu Kazandı";
                    }
                }
            }
            else
            {
                foreach (GameObject objem in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[])
                {
                    if (objem.gameObject.CompareTag("KazanmaPaneli"))
                    {
                        objem.gameObject.SetActive(true);
                        GameObject.FindWithTag("KazananKisi").GetComponent<TextMeshProUGUI>().text = "1. Oyuncu Kazandı";
                    }
                }
            }
        }
    }

}