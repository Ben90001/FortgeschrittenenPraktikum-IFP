using UnityEngine;

/// <summary>
/// Bullet script that is shot by towers at enemies.
/// </summary>
public class Bullet : MonoBehaviour
{
    private Enemy target;
    private float speed;
    private float damage;

    public void Initialize(Enemy target, float speed, float damage)
    {
        this.target = target;
        this.speed = speed;
        this.damage = damage;
    }

    public void FixedUpdate()
    {
        // TODO: How should bullets react when the target is destroyed before it could be reached?
        
        if (target != null && target.gameObject != null)
        {
            Vector3 targetPosition = target.transform.position;
            Vector3 currentPosition = transform.position;

            Vector3 direction = targetPosition - currentPosition;

            float distanceToTarget = direction.magnitude;
            float distanceToTravel = speed * Time.fixedDeltaTime;

            if (distanceToTarget > distanceToTravel)
            {
                transform.Translate(direction.normalized * distanceToTravel);
            }
            else
            {
                target.ApplyDamage(damage);

                Destroy(gameObject);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
