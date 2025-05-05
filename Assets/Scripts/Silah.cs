using TMPro;
using UnityEngine;

public class Silah : MonoBehaviour
{
    public Transform SilahCıkısNoktası;
    float AtesEtmeSikligi_1;
    public float AtesEtmeSikligi_2 = .2f;
    public float menzil;
    int DarbeGucu = 20;
    bool sesCalabilir = true;
    [Header("SESLER")]
    public AudioSource[] Sesler;
    [Header("EFEKTLER")]
    public ParticleSystem[] Efektler;

    void Start()
    {
        AtesEtmeSikligi_1 = Time.time;

    }


    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (Time.time > AtesEtmeSikligi_1)
            {
                AtesEt();
                AtesEtmeSikligi_1 = Time.time + AtesEtmeSikligi_2;
            }
        }

        void AtesEt()
        {
           
            Sesler[0].Play();

            RaycastHit hit;
            if (Physics.Raycast(SilahCıkısNoktası.position, SilahCıkısNoktası.forward, out hit, menzil))
            {
                if (hit.transform.CompareTag("Player"))
                {
                    hit.transform.GetComponent<oyuncu>().HasarAl(DarbeGucu);
                    Instantiate(Efektler[0], hit.point, Quaternion.LookRotation(hit.normal));

                }
                else
                {
                    Instantiate(Efektler[1], hit.point, Quaternion.LookRotation(hit.normal));
                }
            }
        }
    }
}
