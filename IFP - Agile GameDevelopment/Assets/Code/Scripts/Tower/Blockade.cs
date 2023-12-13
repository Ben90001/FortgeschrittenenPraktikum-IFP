using UnityEngine;

public class Blockade : BaseTower
{
    [SerializeField]
    private int initialBlockadeHealth;
    private int blockadeHealth;

    [SerializeField]
    private float delayBetweenReleases;
    private float nextReleaseDelay;

    public int BlockadeHealth { get { return blockadeHealth; } }

    public void Reset()
    {
        this.blockadeHealth = this.initialBlockadeHealth;
        this.nextReleaseDelay = 0;
    }

    protected override void TowerUpgrade()
    {
        // TODO: Add funtionality
    }

    protected override bool PerformAction()
    {
        return true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Enemy enemy = other.GetComponent<Enemy>();

        if (enemy != null)
        {
            HandleEnemyEnteredBlockade(enemy);
        }
    }

    private void Start()
    {
        Reset();
    }

    private void HandleEnemyEnteredBlockade(Enemy enemy)
    {
        if (this.blockadeHealth > 0)
        {
            --this.blockadeHealth;

            if (this.blockadeHealth > 0)
            {
                enemy.StopAtBlockade(this, this.nextReleaseDelay);

                this.nextReleaseDelay += this.delayBetweenReleases;
            }
            else
            {
                this.gameObject.SetActive(false);
            }
        }
    }
}
