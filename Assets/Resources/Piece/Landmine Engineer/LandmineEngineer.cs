public class LandmineEngineer : Trigger {

	public override void AtEnemyBottom ()
	{
		GameController.PlaceTrap (piece.location, Database.RandomTrap (), InfoLoader.playerID);
	}
}
