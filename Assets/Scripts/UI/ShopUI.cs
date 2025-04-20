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
        [SerializeField] private ActionShopItem[] actionShopItems;

        private void Start()
        {
            ActionData[] actionDatas = ContentLoader.ActionList;

            System.Random random = new System.Random();
            actionDatas = actionDatas.OrderBy(x => random.Next()).ToArray();

            for(int i = 0; i < actionShopItems.Length; i++)
            {
                actionShopItems[i].Instantiate(this, actionDatas[i]);
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
            inventoryManager.ChangeHeap(-cost);
            actionShopItem.Sold();
        }
    }
}