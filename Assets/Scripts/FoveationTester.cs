using UnityEngine;

public class FoveationTester : MonoBehaviour
{
    void Start()
    {
        // On attend une seconde pour être sûr que l'OVRManager est bien initialisé.
        Invoke(nameof(ForceFoveation), 1.0f);
    }

    void ForceFoveation()
    {
        OVRManager manager = OVRManager.instance;

        if (manager != null)
        {
            // On s'assure que le rendu fovéal est bien activé
            OVRManager.foveatedRenderingLevel = OVRManager.FoveatedRenderingLevel.High;

            // On force immédiatement le niveau le plus bas et le plus visible
            //OVRManager.foveatedRenderingLevel = OVRManager.FoveatedRenderingLevel.Low;

            Debug.Log("Rendu fovéal forcé au niveau 'Low'. Regardez attentivement sur les bords extrêmes de votre vision.");
        }
        else
        {
            Debug.LogError("OVRManager introuvable dans la scène. Le rendu fovéal ne peut pas être contrôlé.");
        }
    }
}