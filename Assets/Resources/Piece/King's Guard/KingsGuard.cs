public class KingsGuard : Trigger {

	public override void Activate ()
	{
		foreach (Piece piece in GameInfo.activePieces[InfoLoader.playerID])
			if (piece.GetPieceType () == "General")
				GameController.boardSetup.pieces [piece.location].GetComponent<PieceInfo> ().trigger.cantBeDestroyedBy = Database.types;
	}
}
