using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;

public class OdaListesi : MonoBehaviourPunCallbacks
{
    public GameObject Odaprefab;
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
       
        for (int i = 0; i < roomList.Count; i++)
        {
            Debug.Log(roomList[i].Name);
            GameObject Room = Instantiate(Odaprefab,Vector3.zero,Quaternion.identity,GameObject.FindWithTag("Content").transform);
            Room.GetComponent<Oda>().Ýsim.text = roomList[i].Name;
        }
    }

}
 
