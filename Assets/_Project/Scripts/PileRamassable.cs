using UnityEngine;


public class PileRamassable : MonoBehaviour
{
    [SerializeField] private float charge = 35f;
    [SerializeField] private Transform visuel;
    [SerializeField] private float vitesseRotation = 70f;

    [Header("Références et effets")]
    [SerializeField] private ParticleSystem effetRamassage;
    [SerializeField] private AudioClip sonRamassage;

    private void Update()
    {
        visuel.Rotate(0f, vitesseRotation * Time.deltaTime, 0f, Space.World);
    }

    private void OnTriggerEnter(Collider autre)
    {
 
        LampeTorche lampe = autre.GetComponentInParent<LampeTorche>();
        if (lampe == null) return;

        lampe.Recharger(charge);
        AudioSource.PlayClipAtPoint(sonRamassage, transform.position);

    
        effetRamassage.transform.SetParent(null);
        effetRamassage.Play();
        Destroy(effetRamassage.gameObject, 3f);

        Destroy(gameObject);
    }
}
