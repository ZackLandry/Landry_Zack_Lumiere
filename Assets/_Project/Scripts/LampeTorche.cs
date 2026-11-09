using UnityEngine;
using UnityEngine.InputSystem;


public class LampeTorche : MonoBehaviour
{
    [SerializeField] private InputActionAsset actionsAsset;

    [Header("Références")]
    [SerializeField] private Light lumiere;
    [SerializeField] private AudioSource sourceAudio;
    [SerializeField] private AudioClip sonBascule;

    [Header("Batterie")]
    [SerializeField] private float batterieMax = 100f;
    [SerializeField] private float consommationParSeconde = 1.2f;

    [Header("Lumière")]
    [SerializeField] private float intensiteNormale = 120f;
    [SerializeField] private float intensiteFaible = 40f;

    private InputAction actionBascule;
    private float batterie;
    private bool allumee = true;
    private bool actif = true;

    private void Awake()
    {
        actionBascule = actionsAsset.FindAction("Player/Attack");
    }

    private void OnEnable()
    {
        actionBascule.Enable();
    }

    private void Start()
    {
        batterie = batterieMax;
    }

    private void Update()
    {
        if (!actif) return;

  
        if (actionBascule.WasPerformedThisFrame() && batterie > 0f)
        {
            allumee = !allumee;
            sourceAudio.PlayOneShot(sonBascule);
        }

     
        if (allumee)
        {
            batterie -= consommationParSeconde * Time.deltaTime;
            if (batterie <= 0f)
            {
                batterie = 0f;
                allumee = false;
            }
        }

        lumiere.enabled = allumee;
        if (batterie < batterieMax * 0.2f) lumiere.intensity = intensiteFaible;
        else lumiere.intensity = intensiteNormale;
    }


    public float BatterieNormalisee()
    {
        return batterie / batterieMax;
    }


    public void Recharger(float quantite)
    {
        if (batterie <= 0f) allumee = true;
        batterie = Mathf.Min(batterieMax, batterie + quantite);
    }


    public void DefinirActif(bool valeur)
    {
        actif = valeur;
    }
}
