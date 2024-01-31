using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float Speed;
    public float Damage = 1.0f;

    [HideInInspector]
    public Enemy Target;

    public void FixedUpdate()
    {
        // TODO: How should bullets react when the target is destroyed before it could be reached?
        
        if (Target != null && Target.gameObject != null)
        {
            Vector3 targetPosition = Target.transform.position;
            Vector3 currentPosition = transform.position;

            Vector3 direction = targetPosition - currentPosition;

            float distanceToTarget = direction.magnitude;
            float distanceToTravel = Speed * Time.fixedDeltaTime;

            if (distanceToTarget > distanceToTravel)
            {
                transform.Translate(direction.normalized * distanceToTravel);
            }
            else
            {
                // TODO: Add damage stats

                Target.ApplyDamage(Damage);

                Destroy(gameObject);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
