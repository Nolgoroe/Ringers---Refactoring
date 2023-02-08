using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestLogic : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private BasicCustomButton customButton;


    private void Start()
    {
        GameManager.instance.summonedChest = this;
    }
    public void OnPressedChest()
    {
        StartCoroutine(InitiateLootGive());
    }

    private IEnumerator InitiateLootGive()
    {
        customButton.isInteractable = false;
        anim.SetTrigger("TappedChest");

        yield return new WaitForSeconds(1.2f);

        GameManager.instance.AdvanceGiveLootFromManager();
    }
    public IEnumerator AfterGiveLoot()
    {
        anim.SetTrigger("FinishedLootDisplay");

        UIManager.instance.ContinueAfterChest();

        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }
}
