using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameTacticGesture : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

    public GameObject infoCard;

    private Vector3 newPosition;
    private GameObject tactic;
    private Button button;
    private TacticTrigger trigger;
    private float prevClick = 0;
    private float doubleClickInterval = 1;

    private static List<Vector2Int> targets = new List<Vector2Int>();

    private void Awake()
    {
        newPosition = new Vector3(300, transform.localPosition.y, -6.1f);
    }

    private void Start()
    {
        tactic = transform.Find("Tactic").gameObject;
        button = GetComponent<Button>();
        trigger = tactic.GetComponent<TacticInfo>().trigger;
    }

    public void UseTactic(int caller)
    {
        if (MovementController.selected != null) MovementController.PutDownPiece();
        if (ActivateAbility.activated) ActivateAbility.DeactivateButton();
        if (OnEnterGame.current_tactic != -1) Resume();
        else
        {
            if (targets.Count == 0 && Time.time - prevClick < doubleClickInterval)
            {
                trigger.Activate();
                gameObject.SetActive(false);
            }
            else
            {
                OnEnterGame.current_tactic = caller;
                button.GetComponent<Image>().sprite = button.spriteState.highlightedSprite;
                targets = trigger.ValidTargets();
                if (targets.Count != 0) ActivateAbility.ShowTacticTarget(targets, caller, trigger);
            }
        }
        infoCard.SetActive(false);
        prevClick = Time.time;
    }

    public static void Resume()
    {
        OnEnterGame.CancelTacticHighlight();
        ActivateAbility.RemoveTargets();
        targets.Clear();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!tactic.activeSelf) return;
        infoCard.SetActive(true);
        infoCard.GetComponent<CardInfo>().SetAttributes(tactic.GetComponent<TacticInfo>().tactic);
        infoCard.transform.localPosition = newPosition;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (infoCard.activeSelf)
            infoCard.SetActive(false);
    }

}
