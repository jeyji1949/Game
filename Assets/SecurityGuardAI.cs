
using UnityEngine;
using UnityEngine.AI;

public class SecurityGuard : MonoBehaviour
{
    public Transform player;              // Le joueur à suivre
    public float visionRange = 15f;       // Distance de vision
    public float visionAngle = 120f;      // Angle de vision
    public float stopDistance = 2f;       // Distance à laquelle il s'arrête

    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (CanSeePlayer())
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (distanceToPlayer > stopDistance)
            {
                agent.SetDestination(player.position);
            }
            else
            {
                agent.ResetPath(); // S'arrêter
                // Ici, tu peux déclencher une attaque
                Debug.Log(" Garde attaque ou surveille le joueur !");
            }
        }
        else
        {
            agent.ResetPath(); // Pas de joueur en vue
        }
    }

    bool CanSeePlayer()
    {
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        float angle = Vector3.Angle(transform.forward, directionToPlayer);
        float distance = Vector3.Distance(transform.position, player.position);

        return distance < visionRange && angle < visionAngle * 0.5f;
    }
}
