using UnityEngine;
using Photon.Pun;

public class Silah : MonoBehaviour
{
    public Transform SilahCıkısNoktası;
    float AtesEtmeSikligi_1;
    public float AtesEtmeSikligi_2 = 0.2f;

    private PhotonView ownerPhotonView;

    void Start()
    {
        // Ana oyuncu objesindeki PhotonView bulunuyor
        ownerPhotonView = GetComponentInParent<PhotonView>();
        if (ownerPhotonView == null)
            Debug.LogError("Parent objede PhotonView bulunamadı!");
    }

    void Update()
    {
        if (!ownerPhotonView.IsMine)
            return;

        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (Time.time > AtesEtmeSikligi_1)
            {
                ownerPhotonView.RPC("RPC_AtesEt", RpcTarget.All, SilahCıkısNoktası.position, SilahCıkısNoktası.forward);
                AtesEtmeSikligi_1 = Time.time + AtesEtmeSikligi_2;
            }
        }
    }
}
