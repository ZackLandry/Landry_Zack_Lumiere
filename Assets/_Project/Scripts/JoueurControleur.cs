using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(CharacterController))]
public class JoueurControleur : MonoBehaviour
{
    [SerializeField] private InputActionAsset actionsAsset;

    [Header("Déplacement")]
    [SerializeField] private float vitesseMarche = 3f;
    [SerializeField] private float vitesseCourse = 5.5f;
    [SerializeField] private float vitesseRotation = 12f;
    [SerializeField] private float gravite = -20f;

    [Header("Caméra")]
    [SerializeField] private Transform pivotCamera;
    [SerializeField] private float sensibiliteSouris = 0.12f;

    [Header("Physique")]
    [SerializeField] private float forcePoussee = 60f;

    private InputAction actionDeplacement;
    private InputAction actionRegard;
    private InputAction actionCourse;
    private CharacterController controleur;

    private float yaw;
    private float pitch = 10f;
    private float vitesseVerticale;
    private bool controlesActifs = true;

    private void Awake()
    {
        controleur = GetComponent<CharacterController>();
        actionDeplacement = actionsAsset.FindAction("Player/Move");
        actionRegard = actionsAsset.FindAction("Player/Look");
        actionCourse = actionsAsset.FindAction("Player/Sprint");
    }

    private void OnEnable()
    {
        actionDeplacement.Enable();
        actionRegard.Enable();
        actionCourse.Enable();
    }

    private void OnDisable()
    {
        actionDeplacement.Disable();
        actionRegard.Disable();
        actionCourse.Disable();
    }

    private void Start()
    {
        yaw = transform.eulerAngles.y;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        if (!controlesActifs) return;

      
        Vector2 regard = actionRegard.ReadValue<Vector2>();
        yaw += regard.x * sensibiliteSouris;
        pitch -= regard.y * sensibiliteSouris;
        pitch = Mathf.Clamp(pitch, -35f, 60f);
        pivotCamera.rotation = Quaternion.Euler(pitch, yaw, 0f);

    
        Vector2 entree = actionDeplacement.ReadValue<Vector2>();
        Vector3 direction = Quaternion.Euler(0f, yaw, 0f) * new Vector3(entree.x, 0f, entree.y);

        float vitesse = vitesseMarche;
        if (actionCourse.IsPressed()) vitesse = vitesseCourse;


        if (direction.magnitude > 0.1f)
        {
            Quaternion rotationVoulue = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotationVoulue, vitesseRotation * Time.deltaTime);
        }

        // Gravité
        if (controleur.isGrounded && vitesseVerticale < 0f) vitesseVerticale = -2f;
        vitesseVerticale += gravite * Time.deltaTime;

        Vector3 mouvement = direction * vitesse;
        mouvement.y = vitesseVerticale;
        controleur.Move(mouvement * Time.deltaTime);
    }

 
    public void DefinirControlesActifs(bool actif)
    {
        controlesActifs = actif;
        if (actif) Cursor.lockState = CursorLockMode.Locked;
        else Cursor.lockState = CursorLockMode.None;
    }


    private void OnControllerColliderHit(ControllerColliderHit impact)
    {
        Rigidbody corps = impact.collider.attachedRigidbody;
        if (corps == null || corps.isKinematic) return;

        Vector3 poussee = new Vector3(impact.moveDirection.x, 0f, impact.moveDirection.z);
        corps.AddForce(poussee * forcePoussee);
    }
}
