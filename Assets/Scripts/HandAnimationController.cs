using UnityEngine;
using OVR; // N'oublie pas d'ajouter la bibliothèque de Meta !

public class HandAnimationController : MonoBehaviour
{
    public Animator handAnimator;
    public OVRInput.Controller controllerType;

    void Update()
    {
        if (handAnimator == null) return;

        // --- Lecture des entrées ---

        // 1. Gâchette de l'index (Trigger) - ne change pas
        float triggerValue = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, controllerType);

        // 2. Gâchette du majeur (Grip) - ne change pas
        float gripValue = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, controllerType);

        // 3. Boutons (A, B, X, Y) - NOUVEAU
        // On vérifie si le pouce TOUCHE les boutons principaux.
        bool buttonOneTouch = OVRInput.Get(OVRInput.Button.One, controllerType); // Bouton A (droite) ou X (gauche)
        bool buttonTwoTouch = OVRInput.Get(OVRInput.Button.Two, controllerType); // Bouton B (droite) ou Y (gauche)
        bool buttonThreeTouch = OVRInput.Get(OVRInput.Button.Three, controllerType); // Bouton Meta (droite) ou Menu (gauche)
        // L'animator attend une seule valeur pour savoir si le pouce est sur les boutons.
        float buttonsValue = (buttonOneTouch || buttonTwoTouch) ? 1.0f : 0.0f;

        // 4. Joystick (Thumbstick) - NOUVEAU
        // On récupère la position du joystick sous forme de Vector2 (X, Y)
        Vector2 thumbstickValue = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, controllerType);


        // --- Transmission à l'Animator ---

        handAnimator.SetFloat("Trigger", triggerValue);
        handAnimator.SetFloat("Grip", gripValue);

        // On envoie les nouvelles valeurs à l'Animator - NOUVEAU
        handAnimator.SetFloat("Button 1", buttonOneTouch ? 1.0f : 0.0f);
        handAnimator.SetFloat("Button 2", buttonTwoTouch ? 1.0f : 0.0f);
        handAnimator.SetFloat("Button 3", buttonThreeTouch ? 1.0f : 0.0f);
        handAnimator.SetFloat("Joy X", thumbstickValue.x);
        handAnimator.SetFloat("Joy Y", thumbstickValue.y);
    }
}