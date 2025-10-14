using UnityEngine;

public class DelayedHands : MonoBehaviour
{
    [Header("Références")]
    [Tooltip("Le transform de la vraie manette que cette main doit suivre.")]
    public Transform realController;

    [Header("Paramètres de l'Effet")]
    [Tooltip("Niveau d'ébriété actuel (0 à 1), contrôlé par le GestionnaireEbriete.")]
    [Range(0f, 1f)]
    public float drunkenness = 0f;

    [Header("Réglages du Délai")]
    [Tooltip("Vitesse de rattrapage quand on n'est pas ivre.")]
    private float maxSpeed = 40f;
    [Tooltip("Vitesse de rattrapage quand l'ivresse est maximale.")]
    private float minSpeed = 5f;

    void LateUpdate()
    {
        // S'il n'y a pas de manette à suivre, on ne fait rien.
        if (realController == null)
        {
            return;
        }

        // 1. On calcule la vitesse de rattrapage en fonction de l'ivresse.
        float currentSpeed = Mathf.Lerp(maxSpeed, minSpeed, drunkenness);

        // 2. On détermine le facteur de lissage pour cette frame.
        float lerpFactor = Time.deltaTime * currentSpeed;

        // 3. On déplace et fait pivoter doucement la main virtuelle vers la main réelle.
        transform.position = Vector3.Lerp(transform.position, realController.position, lerpFactor);
        transform.rotation = Quaternion.Slerp(transform.rotation, realController.rotation, lerpFactor);
    }
}