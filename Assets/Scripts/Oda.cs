using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Oda : MonoBehaviour
{
    public TextMeshProUGUI �sim;

    public void JoinRoom()
    {
        GameObject.FindWithTag("SunucuYonetimi").GetComponent<sunucuyonetim>().JoinRoomInList(�sim.text);
    }
}
