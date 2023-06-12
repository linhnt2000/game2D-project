using System.Collections;
using System.Collections.Generic;
using RotaryHeart.Lib.SerializableDictionary;
using UnityEngine;
using UnityEngine.Purchasing;

public class IAPPurchaseHandler : IAPListener
{
    private const string ID_VIP = "rob_world_vip_pack_999";

    [SerializeField]
    private IAPPackContent iapPackContent;

    public void ProcessSuccessPurchase(Product product)
    {
        if (iapPackContent.ContainsKey(product.definition.id))
        {
            if (product.definition.id == ID_VIP)
            {
                GameData.vip = true;
                PanelShopController.instance.CheckBtnStatus();
            }
            ResourceItemCollection coll = iapPackContent[product.definition.id];
            GameData.ClaimResourceItems(coll.items);
            UIClaimResourcePanel.Setup(coll.items).Show();
        }
    }
}

[System.Serializable]
public class IAPPackContent : SerializableDictionaryBase<string, ResourceItemCollection>
{
}
