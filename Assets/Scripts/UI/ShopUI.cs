using System;
using System.Linq;
using UnityEngine;

namespace ProjectSecurity.Gameplay
{
    public class ShopUI : MonoBehaviour
    {
        [Header("Required Components")]
        [SerializeField] private InventoryManager inventoryManager;
        [SerializeField] private GameObject container;
        [SerializeField] private Transform actionItemStoringTransform;
        [SerializeField] private GameObject specialActionItemPrefab;

        [Header("Settings")]
        [SerializeField] private int amountOfActionItems;

        private void Start()
        {
            ActionData[] actionDatas = ActionDataLoader.ActionList;

            System.Random random = new System.Random();
            actionDatas = actionDatas.OrderBy(x => random.Next()).ToArray();

            for(int i = 0; i < amountOfActionItems; i++)
            {
                ActionShopItem actionShopItem = Instantiate(specialActionItemPrefab, 
                    actionItemStoringTransform).GetComponent<ActionShopItem>();
                actionShopItem.Instantiate(this, actionDatas[i]);
            }
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.P))
            {
                container.SetActive(!container.activeSelf);
            }
        }

        public void TryPurchase(ActionShopItem actionShopItem, int cost)
        {
            if (inventoryManager.Heap < cost) return;

            inventoryManager.GetNewSpecialAction(actionShopItem.specialAction);
            actionShopItem.Sold();
        }
    }
}