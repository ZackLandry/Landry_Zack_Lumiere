using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GestionnaireBalises : MonoBehaviour
{
    [SerializeField] private BaliseLumineuse[] balises;

    [Header("Délais (secondes)")]
    [SerializeField] private float delaiPremiereBalise = 3f;
    [SerializeField] private float delaiEntreBalises = 2f;
    [SerializeField] private float delaiAvantFin = 2.5f;

    [Header("Références")]
    [SerializeField] private HudJeu hud;
    [SerializeField] private JoueurControleur joueur;
    [SerializeField] private LampeTorche lampe;
    [SerializeField] private GameObject[] objetsActivesALaVictoire;

    [Header("Textes")]
    [SerializeField] private string messageVictoire = "La lumière est revenue.\n\nAppuie sur R pour recommencer";

    private int indexCourant = -1;   
    private float minuteur;
    private bool enAttente = true;
    private bool victoire = false;
    private bool finAffichee = false;

    private void Start()
    {
        minuteur = delaiPremiereBalise;
    }

    private void Update()
    {
        if (victoire)
        {
            GererFin();
            return;
        }

        if (enAttente)
        {
            minuteur -= Time.deltaTime;
            if (minuteur <= 0f)
            {
                enAttente = false;
                indexCourant++;
                balises[indexCourant].Activer();
            }
            return;
        }


        if (balises[indexCourant].EstTerminee())
        {
            if (indexCourant + 1 < balises.Length)
            {
                enAttente = true;
                minuteur = delaiEntreBalises;
            }
            else
            {
                victoire = true;
                minuteur = delaiAvantFin;
                lampe.DefinirActif(false);
                foreach (GameObject objet in objetsActivesALaVictoire)
                {
                    objet.SetActive(true);
                }
            }
        }
    }

    private void GererFin()
    {
        minuteur -= Time.deltaTime;
        if (minuteur <= 0f && !finAffichee)
        {
            finAffichee = true;
            joueur.DefinirControlesActifs(false);
            hud.AfficherFin(messageVictoire);
        }


        if (finAffichee && Keyboard.current.rKey.wasPressedThisFrame)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
