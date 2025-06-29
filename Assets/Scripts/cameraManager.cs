using UnityEngine;
using Photon.Pun;

public class MyCam : MonoBehaviourPunCallbacks
{
    float MouseX;
    float MouseY;

    public Transform Body;  // Vücut objesi
    public Transform Head;  // Kamera objesi veya baş objesi

    public float Angle;

    PhotonView pw;

    void Start()
    {
        pw = GetComponentInParent<PhotonView>();

        if (pw.IsMine)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            // Bu kamera yerel değil, devre dışı bırak
            // Böylece başka oyuncuların kameraları aktif olmaz
            Head.gameObject.SetActive(false);
        }
    }

    void LateUpdate()
    {
        // Sadece kendi oyuncum kontrol etsin
        if (!pw.IsMine)
            return;

        MouseX = Input.GetAxis("Mouse X") * 100 * Time.deltaTime;
        Body.Rotate(Vector3.up, MouseX);

        MouseY = Input.GetAxis("Mouse Y") * 100 * Time.deltaTime;
        Angle -= MouseY;
        Angle = Mathf.Clamp(Angle, 0, 30);
        Head.localRotation = Quaternion.Euler(Angle, 0, 0);
    }
}
