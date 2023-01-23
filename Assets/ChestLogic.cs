using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestLogic : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private BasicCustomButton customButton;

    public void OnPressedChest()
    {
        customButton.isInteractable = false;
        anim.SetTrigger("TappedChest");

        Debug.Log("Give Loot");

        StartCoroutine(AfterChestOpened());
    }

    IEnumerator AfterChestOpened()
    {
        yield return new WaitForSeconds(3);

        Debug.Log("GAVE Loot");

        anim.SetTrigger("FinishedLootDisplay");

        UIManager.instance.ContinueAfterChest();


        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }
}
