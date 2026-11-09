using UnityEngine;


public class AnimationJoueur : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private CharacterController controleur;

    private void Update()
    {
        Vector3 vitesse = controleur.velocity;
        vitesse.y = 0f;
        animator.SetFloat("Vitesse", vitesse.magnitude);
    }
}
