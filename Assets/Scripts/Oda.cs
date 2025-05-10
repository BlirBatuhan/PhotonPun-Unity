using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Oda : MonoBehaviour
{
    public TextMeshProUGUI Ýsim;

    public void JoinRoom()
    {
        GameObject.FindWithTag("SunucuYonetimi").GetComponent<sunucuyonetim>().JoinRoomInList(Ýsim.text);
    }
}
