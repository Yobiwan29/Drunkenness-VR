using UnityEngine;

public class BalanceManager : MonoBehaviour
{
    [Header("Paramètres")]
    public float drunkenness = 0f;
    public float maxSwayAngle = 1.5f;
    public float swaySpeed = 0.2f;

    private float seedX, seedZ;
    private Quaternion lastSwayRotation = Quaternion.identity;

    void Start()
    {
        seedX = Random.Range(0f, 100f);
        seedZ = Random.Range(0f, 100f);
    }

    void LateUpdate()
    {
        // ÉTAPE 1: On "annule" le balancement de l'image précédente pour revenir
        // à la rotation pure calculée par le SDK pour CETTE image.
        transform.localRotation = transform.localRotation * Quaternion.Inverse(lastSwayRotation);

        // ÉTAPE 2: On calcule le nouveau balancement pour l'image actuelle.
        if (drunkenness > 0.01f)
        {
            float time = Time.time * swaySpeed;
            float swayX = (Mathf.PerlinNoise(time, seedX) * 2f - 1f) * maxSwayAngle * drunkenness;
            float swayZ = (Mathf.PerlinNoise(time, seedZ) * 2f - 1f) * maxSwayAngle * drunkenness;
            Quaternion currentSwayRotation = Quaternion.Euler(swayX, 0, swayZ);

            // ÉTAPE 3: On applique ce nouveau balancement.
            transform.localRotation = transform.localRotation * currentSwayRotation;

            // ÉTAPE 4: On mémorise ce qu'on vient de faire pour pouvoir l'annuler à la prochaine image.
            lastSwayRotation = currentSwayRotation;
        }
        else
        {
            // S'il n'y a plus d'ivresse, on s'assure que le "dernier balancement" est neutre.
            lastSwayRotation = Quaternion.identity;
        }
    }

    public Vector2 GetSwayAngles()
    {
        if (drunkenness > 0.01f)
        {
            float time = Time.time * swaySpeed;
            float swayX = (Mathf.PerlinNoise(time, seedX) * 2f - 1f) * maxSwayAngle * drunkenness;
            float swayZ = (Mathf.PerlinNoise(time, seedZ) * 2f - 1f) * maxSwayAngle * drunkenness;
            return new Vector2(swayX, swayZ);
        }
        return Vector2.zero;
    }
}