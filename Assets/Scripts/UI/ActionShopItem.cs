using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace ProjectSecurity.Gameplay
{
    public class ActionShopItem : MonoBehaviour
    {
        [Header("Required Components")]
        [SerializeField] private TextMeshProUGUI textMeshName;
        [SerializeField] private TextMeshProUGUI textMeshCost;
        [SerializeField] private GameObject soldContainter;

        public SpecialAction specialAction;
        private int cost = 3;

        private ShopUI shopUI;
        private Button button;

        private void Awake()
        {
            button = GetComponent<Button>();
        }

        public void Instantiate(ShopUI shopUI, ActionData actionData)
        {
            this.shopUI = shopUI;

            textMeshName.text = actionData.actionName;
            textMeshCost.text = cost.ToString();

            specialAction = new SpecialAction(actionData);
        }

        public void NotifyTryPurchase()
        {
            shopUI.TryPurchase(this, cost);
        }

        public void Sold()
        {
            soldContainter.SetActive(true);
            button.interactable = false;
        }
    }
}
