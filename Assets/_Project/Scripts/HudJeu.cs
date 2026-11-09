using UnityEngine;
using UnityEngine.UI;

public class HudJeu : MonoBehaviour
{
    [SerializeField] private LampeTorche lampe;
    [SerializeField] private RectTransform remplissageBatterie;
    [SerializeField] private Image imageRemplissage;
    [SerializeField] private Text texteFin;

    [Header("Couleurs de la batterie")]
    [SerializeField] private Color couleurNormale = new Color(1f, 0.85f, 0.45f);
    [SerializeField] private Color couleurFaible = new Color(0.9f, 0.2f, 0.15f);

    private void Update()
    {
        float valeur = lampe.BatterieNormalisee();
        remplissageBatterie.anchorMax = new Vector2(valeur, 1f);

        if (valeur < 0.2f) imageRemplissage.color = couleurFaible;
        else imageRemplissage.color = couleurNormale;
    }

    public void AfficherFin(string message)
    {
        texteFin.text = message;
    }
}
