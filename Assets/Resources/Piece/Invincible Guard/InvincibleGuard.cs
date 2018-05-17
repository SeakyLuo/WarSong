public class InvincibleGuard : Trigger {

	public override void BloodThirsty ()
	{
		GameInfo.actions [InfoLoader.playerID]++;
	}
}
