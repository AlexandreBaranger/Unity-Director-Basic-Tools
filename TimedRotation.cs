using UnityEngine;
using System.Collections.Generic;

public class TimedRotation : MonoBehaviour
{
    [System.Serializable]
    public class RotationEvent
    {
        public float rotationDelay; // Temps avant que la rotation commence
        public float rotationSpeed; // Vitesse de rotation en degr�s par seconde
        public float rotationDuration; // Dur�e de la rotation
        public Vector3 rotationAxis = Vector3.up; // Axe de rotation

        [HideInInspector]
        public bool isCompleted = false; // Indicateur si la rotation est termin�e
        [HideInInspector]
        public float rotationStartTime = 0.0f; // Temps de d�but de la rotation
    }

    [Header("Rotation Events")]
    public List<RotationEvent> rotationEvents = new List<RotationEvent>();

    private float elapsedTime = 0.0f;

    void Update()
    {
        // Temps �coul� depuis le d�but
        elapsedTime += Time.deltaTime;

        foreach (var rotationEvent in rotationEvents)
        {
            if (!rotationEvent.isCompleted)
            {
                // Commencer la rotation apr�s le d�lai d�fini
                if (elapsedTime >= rotationEvent.rotationDelay && rotationEvent.rotationStartTime == 0.0f)
                {
                    rotationEvent.rotationStartTime = elapsedTime;
                }

                // Effectuer la rotation si le d�lai est �coul� et si la dur�e de rotation n'est pas encore atteinte
                if (rotationEvent.rotationStartTime != 0.0f && elapsedTime <= rotationEvent.rotationStartTime + rotationEvent.rotationDuration)
                {
                    transform.Rotate(rotationEvent.rotationAxis, rotationEvent.rotationSpeed * Time.deltaTime);
                }

                // Marquer l'�v�nement comme compl�t� apr�s la dur�e d�finie
                if (rotationEvent.rotationStartTime != 0.0f && elapsedTime > rotationEvent.rotationStartTime + rotationEvent.rotationDuration)
                {
                    rotationEvent.isCompleted = true;
                }
            }
        }
    }

    // Pour r�initialiser le script (optionnel)
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
