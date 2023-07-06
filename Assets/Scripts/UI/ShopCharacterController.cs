using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopCharacterController : MonoBehaviour
{
    public static ShopCharacterController instance;

    [SerializeField] private List<SkinShopController> skinShopControllers;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void UpdateBtnDisplay()
    {
        for (int i = 0; i < skinShopControllers.Count; i++)
        {
            skinShopControllers[i].UpdateBtnStatus();
        }
    }
}
