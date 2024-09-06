public interface IEnemyBehavior
{
    // Defines shared properties of behavior scripts.
    bool IsDefeated { get; set; }
    string EnemyType { get; }
}