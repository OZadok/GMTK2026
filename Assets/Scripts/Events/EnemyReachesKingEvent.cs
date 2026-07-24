namespace Events
{
	public class EnemyReachesKingEvent
	{
		public Enemy Enemy;
		public EnemyReachesKingEvent(Enemy enemy)
		{
			Enemy = enemy;
		}
	}
}
