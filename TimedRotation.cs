using UnityEngine;
using System.Collections.Generic;

public class TimedRotation : MonoBehaviour
{
    [System.Serializable]
    public class RotationEvent
    {
        public float rotationDelay; // Temps avant que la rotation commence
        public float rotationSpeed; // Vitesse de rotation en degrés par seconde
        public float rotationDuration; // Durée de la rotation
        public Vector3 rotationAxis = Vector3.up; // Axe de rotation

        [HideInInspector]
        public bool isCompleted = false; // Indicateur si la rotation est terminée
        [HideInInspector]
        public float rotationStartTime = 0.0f; // Temps de début de la rotation
    }

    [Header("Rotation Events")]
    public List<RotationEvent> rotationEvents = new List<RotationEvent>();

    private float elapsedTime = 0.0f;

    void Update()
    {
        // Temps écoulé depuis le début
        elapsedTime += Time.deltaTime;

        foreach (var rotationEvent in rotationEvents)
        {
            if (!rotationEvent.isCompleted)
            {
                // Commencer la rotation après le délai défini
                if (elapsedTime >= rotationEvent.rotationDelay && rotationEvent.rotationStartTime == 0.0f)
                {
                    rotationEvent.rotationStartTime = elapsedTime;
                }

                // Effectuer la rotation si le délai est écoulé et si la durée de rotation n'est pas encore atteinte
                if (rotationEvent.rotationStartTime != 0.0f && elapsedTime <= rotationEvent.rotationStartTime + rotationEvent.rotationDuration)
                {
                    transform.Rotate(rotationEvent.rotationAxis, rotationEvent.rotationSpeed * Time.deltaTime);
                }

                // Marquer l'événement comme complété après la durée définie
                if (rotationEvent.rotationStartTime != 0.0f && elapsedTime > rotationEvent.rotationStartTime + rotationEvent.rotationDuration)
                {
                    rotationEvent.isCompleted = true;
                }
            }
        }
    }

    // Pour réinitialiser le script (optionnel)
    public void ResetRotation()
    {
        elapsedTime = 0.0f;
        foreach (var rotationEvent in rotationEvents)
        {
            rotationEvent.isCompleted = false;
            rotationEvent.rotationStartTime = 0.0f;
        }
    }
}
