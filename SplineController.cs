using UnityEngine;

public class SplineController : MonoBehaviour
{
    [System.Serializable]
    public class SplinePoint
    {
        public Transform point; // Point de contr�le du spline
        public float speed = 1f; // Vitesse de d�placement vers ce point
        public float pauseDuration = 0f; // Temps de pause � ce point
    }

    public SplinePoint[] splinePoints; // Points de contr�le du spline
    public Transform objectToMove; // Objet � d�placer le long du spline

    private int currentPointIndex = 0; // Indice du point de contr�le actuel
    private float distanceToPoint = 0f; // Distance entre l'objet et le point de contr�le actuel
    private float pauseTimer = 0f; // Timer pour le temps de pause

    void Update()
    {
        // V�rifier s'il y a des points de contr�le
        if (splinePoints.Length == 0)
            return;

        // Si le timer de pause est actif, le d�cr�menter
        if (pauseTimer > 0)
        {
            pauseTimer -= Time.deltaTime;
            return; // Ne pas d�placer l'objet tant que la pause n'est pas termin�e
        }

        // D�placer l'objet vers le point de contr�le actuel
        MoveTowardsCurrentPoint();

        // V�rifier si l'objet est arriv� au point de contr�le actuel
        if (HasReachedCurrentPoint())
        {
            // D�marrer le timer de pause pour le point actuel
            pauseTimer = splinePoints[currentPointIndex].pauseDuration;

            // Passer au point de contr�le suivant
            currentPointIndex = (currentPointIndex + 1) % splinePoints.Length;
        }
    }

    void MoveTowardsCurrentPoint()
    {
        // Calculer la direction vers le point de contr�le actuel
        Vector3 direction = splinePoints[currentPointIndex].point.position - objectToMove.position;

        // D�placer l'objet vers le point de contr�le actuel � la vitesse sp�cifi�e pour ce point
        objectToMove.position += direction.normalized * splinePoints[currentPointIndex].speed * Time.deltaTime;

        // Mettre � jour la distance restante vers le point de contr�le actuel
        distanceToPoint = direction.magnitude;
    }

    bool HasReachedCurrentPoint()
    {
        // V�rifier si l'objet est suffisamment proche du point de contr�le actuel
        return distanceToPoint < 0.1f;
    }
}
