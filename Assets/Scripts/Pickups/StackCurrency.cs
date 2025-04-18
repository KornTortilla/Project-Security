using UnityEngine;

public class StackCurrency : UpgradeCurrency
{
    public override void ProcessPickUp(GameObject playerObj)
    {
        Debug.Log("Picked up " + base.value + " stacks");
        // /script for player pickup/ = playerObj.getComponent</script for player pickup/>
        // Increment count for stack
    }
}
