namespace Events
{
	public class TimerTimeChangeEvent
	{
		public readonly float Amount;

		public TimerTimeChangeEvent(float amount)
		{
			Amount = amount;
		}
	}
}
