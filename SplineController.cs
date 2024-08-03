using UnityEngine;

public class SplineController : MonoBehaviour
{
    [System.Serializable]
    public class SplinePoint
    {
        public Transform point; // Point de contrôle du spline
        public float speed = 1f; // Vitesse de déplacement vers ce point
        public float pauseDuration = 0f; // Temps de pause à ce point
    }

    public SplinePoint[] splinePoints; // Points de contrôle du spline
    public Transform objectToMove; // Objet à déplacer le long du spline

    private int currentPointIndex = 0; // Indice du point de contrôle actuel
    private float distanceToPoint = 0f; // Distance entre l'objet et le point de contrôle actuel
    private float pauseTimer = 0f; // Timer pour le temps de pause

    void Update()
    {
        // Vérifier s'il y a des points de contrôle
        if (splinePoints.Length == 0)
            return;

        // Si le timer de pause est actif, le décrémenter
        if (pauseTimer > 0)
        {
            pauseTimer -= Time.deltaTime;
            return; // Ne pas déplacer l'objet tant que la pause n'est pas terminée
        }

        // Déplacer l'objet vers le point de contrôle actuel
        MoveTowardsCurrentPoint();

        // Vérifier si l'objet est arrivé au point de contrôle actuel
        if (HasReachedCurrentPoint())
        {
            // Démarrer le timer de pause pour le point actuel
            pauseTimer = splinePoints[currentPointIndex].pauseDuration;

            // Passer au point de contrôle suivant
            currentPointIndex = (currentPointIndex + 1) % splinePoints.Length;
        }
    }

    void MoveTowardsCurrentPoint()
    {
        // Calculer la direction vers le point de contrôle actuel
        Vector3 direction = splinePoints[currentPointIndex].point.position - objectToMove.position;

        // Déplacer l'objet vers le point de contrôle actuel à la vitesse spécifiée pour ce point
        objectToMove.position += direction.normalized * splinePoints[currentPointIndex].speed * Time.deltaTime;

        // Mettre à jour la distance restante vers le point de contrôle actuel
        distanceToPoint = direction.magnitude;
    }

    bool HasReachedCurrentPoint()
    {
        // Vérifier si l'objet est suffisamment proche du point de contrôle actuel
        return distanceToPoint < 0.1f;
    }
}
