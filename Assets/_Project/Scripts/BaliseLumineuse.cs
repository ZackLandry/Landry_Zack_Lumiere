using UnityEngine;

public class BaliseLumineuse : MonoBehaviour
{
    [Header("Éléments visuels")]
    [SerializeField] private Light lumiere;
    [SerializeField] private GameObject faisceau;
    [SerializeField] private ParticleSystem effetFin;
    [SerializeField] private float intensiteActive = 60f;
    [SerializeField] private float amplitudePulsation = 15f;
    [SerializeField] private float vitessePulsation = 2.2f;

    [Header("Audio")]
    [SerializeField] private AudioClip sonApparition;
    [SerializeField] private AudioClip sonTerminee;

    private bool active = false;
    private bool terminee = false;

    private void Awake()
    {
        Eteindre();
    }

    private void Update()
    {
        if (!active) return;

       
        lumiere.intensity = intensiteActive + Mathf.Sin(Time.time * vitessePulsation) * amplitudePulsation;
    }

    public bool EstTerminee()
    {
        return terminee;
    }

  
    public void Activer()
    {
        active = true;
        lumiere.enabled = true;
        faisceau.SetActive(true);
        if (sonApparition != null) AudioSource.PlayClipAtPoint(sonApparition, transform.position);
    }

    private void Eteindre()
    {
        lumiere.enabled = false;
        faisceau.SetActive(false);
    }

 
    private void OnTriggerEnter(Collider autre)
    {
        if (!active) return;
        if (autre.GetComponentInParent<JoueurControleur>() == null) return;

        active = false;
        terminee = true;
        Eteindre();
        effetFin.Play();
        if (sonTerminee != null) AudioSource.PlayClipAtPoint(sonTerminee, transform.position);
    }
}
