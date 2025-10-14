using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using OVR;
using Oculus.Interaction.Locomotion;

public class GestionnaireEbriete : MonoBehaviour
{
    [Header("Références aux Effets")]
    [Tooltip("Le Global Volume qui contient les effets de post-processing.")]
    public Volume globalVolume;
    [Tooltip("La main gauche visuelle (votre clone) qui a le script DelayedHands.")]
    public DelayedHands leftHand;
    [Tooltip("La main droite visuelle (votre clone) qui a le script DelayedHands.")]
    public DelayedHands rightHand;
    public BalanceManager balanceManager;
    public DrunkLocomotor drunkLocomotor;

    [Header("Paramètres de la Simulation")]
    [Tooltip("La durée en minutes pour que l'effet atteigne son maximum.")]
    public float dureeMonteeEnMinutes = 1.0f;

    [Header("Réglages des Intensités")]
    public float intensiteMaxMotionBlur = 0.5f;
    public float intensiteMaxVignette = 0.6f;
    public float intensiteMaxBloom = 10.0f;
    public float intensiteMaxLensDistortion = -1.5f;

    // --- Variables privées ---
    private float niveauEbriete = 0f;
    private bool isSimulationActive = false;
    private float tempsEcoule = 0f;
    private float dureeMonteeEnSecondes;

    // Références aux composants de post-processing
    private MotionBlur motionBlur;
    private Vignette vignette;
    private Bloom bloom;
    private LensDistortion lensDistortion;

    void Start()
    {
        // --- Initialisation de la simulation ---
        niveauEbriete = 0f;
        isSimulationActive = false;
        tempsEcoule = 0f;
        dureeMonteeEnSecondes = dureeMonteeEnMinutes * 60f;

        // --- Récupération des effets de post-processing ---
        if (globalVolume != null)
        {
            globalVolume.profile.TryGet(out motionBlur);
            globalVolume.profile.TryGet(out vignette);
            globalVolume.profile.TryGet(out bloom);
            globalVolume.profile.TryGet(out lensDistortion);
        }

    }

    void Update()
    {
        // --- Déclenchement de la simulation ---
        if (!isSimulationActive)
        {
            if (OVRInput.GetDown(OVRInput.Button.One))
            {
                Debug.Log("Début de la simulation d'ébriété !");
                isSimulationActive = true;
            }
        }

        // --- Progression de l'effet ---
        if (isSimulationActive)
        {
            tempsEcoule += Time.deltaTime;
            niveauEbriete = Mathf.Clamp01(tempsEcoule / dureeMonteeEnSecondes);
        }

        // --- Application des effets ---
        // On distribue la valeur de 'niveauEbriete' à tous les autres scripts

        // Post-Processing
        if (motionBlur != null) motionBlur.intensity.value = niveauEbriete * intensiteMaxMotionBlur;
        if (vignette != null) vignette.intensity.value = niveauEbriete * intensiteMaxVignette;
        if (bloom != null) bloom.intensity.value = 1f + (niveauEbriete * intensiteMaxBloom);
        if (lensDistortion != null) lensDistortion.intensity.value = niveauEbriete * intensiteMaxLensDistortion;

        // Mains
        if (leftHand != null) leftHand.drunkenness = niveauEbriete;
        if (rightHand != null) rightHand.drunkenness = niveauEbriete;

        // Camera Rig (Balance)
        if (balanceManager != null)
        {
            balanceManager.drunkenness = niveauEbriete;
            // La ligne de log ci-dessous n'est plus indispensable, mais vous pouvez la garder
            Debug.Log($"Gestionnaire envoie la valeur {niveauEbriete} au BalanceManager sur l'objet '{balanceManager.gameObject.name}'");
        }

        // 👇 LA LIGNE MANQUANTE EST ICI ! 👇
        // On met aussi à jour le niveau d'ébriété sur le script de déplacement.
        if (drunkLocomotor != null)
        {
            drunkLocomotor.drunkenness = niveauEbriete;
        }
    }
}