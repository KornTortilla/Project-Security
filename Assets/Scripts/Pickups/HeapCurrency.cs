using UnityEngine;

public class HeapCurrency : UpgradeCurrency
{
    public override void ProcessPickUp(GameObject playerObj)
    {
        Debug.Log("Picked up " + base.value + " heaps");
        // /script for player pickup/ = playerObj.getComponent</script for player pickup/>
        // Increment count for stack
    }
}
