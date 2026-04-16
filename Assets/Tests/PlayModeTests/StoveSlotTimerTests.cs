using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class StoveSlotTimerTests
{
    private readonly List<GameObject> _created = new List<GameObject>();
    private static readonly WaitForSeconds WaitForCook = new WaitForSeconds(6.1f);

    private T Make<T>() where T : MonoBehaviour
    {
        GameObject go = new GameObject();
        _created.Add(go);
        return go.AddComponent<T>();
    }

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        Time.timeScale = 10f;
        yield return null;
    }

    [TearDown]
    public void TearDown()
    {
        Time.timeScale = 1f;
        foreach (GameObject go in _created)
            if (go != null) Object.Destroy(go);
        _created.Clear();
    }

    private StoveSlot MakeSlot()
    {
        StoveSlot slot = Make<StoveSlot>();
        slot.cookedMeatPrefab = Make<CookedMeat>();
        return slot;
    }

    [UnityTest]
    public IEnumerator AfterCookDuration_CookedMeatSpawnsAtRawMeatPosition()
    {
        StoveSlot slot = MakeSlot();
        PlayerHand hand = new PlayerHand();
        RawMeat raw = Make<RawMeat>();
        raw.transform.position = new Vector3(1f, 0f, 2f);
        hand.TryPickUp(raw);
        slot.Interact(hand);

        yield return WaitForCook;

        CookedMeat cooked = Object.FindFirstObjectByType<CookedMeat>();
        Assert.AreEqual(new Vector3(1f, 0f, 2f), cooked.transform.position);
    }

    [UnityTest]
    public IEnumerator WhenMeatIsCooked_Interact_WithEmptyHand_PutsItInHand()
    {
        StoveSlot slot = MakeSlot();
        PlayerHand hand = new PlayerHand();
        hand.TryPickUp(Make<RawMeat>());
        slot.Interact(hand);

        yield return WaitForCook;

        slot.Interact(hand);

        Assert.IsInstanceOf<CookedMeat>(hand.Held);
    }

}
