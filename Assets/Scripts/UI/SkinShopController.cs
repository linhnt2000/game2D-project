using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkinShopController : ResourceItemShop
{
    [SerializeField] private GameObject selectBtn;
    [SerializeField] private GameObject iAPBtn;
    [SerializeField] private bool isIAP;

    [SerializeField] private Sprite greenBtnSprite;
    [SerializeField] private Sprite yellowBtnSprite;

    public override void OnEnable()
    {
        base.OnEnable();
        UpdateBtnStatus();
    }

    //public override void BuyComplete()
    //{
    //    UIClaimResourcePanel.Setup(resourceItems).Show();
    //    GameData.ClaimCharacter(resourceItems[0].detail);
    //    UpdateBtnStatus();
    //}

    public void Select()
    {
        selectBtn.SetActive(true);
        selectBtn.GetComponentInChildren<Text>().enabled = false;
        selectBtn.GetComponentInChildren<Outline>().enabled = false;
        selectBtn.GetComponent<Image>().sprite = yellowBtnSprite;
        GameData.SelectedCharacter = resourceItems[0].detail;
        ShopCharacterController.instance.UpdateBtnDisplay();
    }

    public void UpdateBtnStatus()
    {
        bool hasCharacter = GameData.HasCharacter(resourceItems[0].detail);
        bool isSelecting = GameData.SelectedCharacter == resourceItems[0].detail;
        if (hasCharacter)
        {
            iAPBtn.SetActive(false);
            if (isSelecting)
            {
                selectBtn.SetActive(true);
                selectBtn.GetComponentInChildren<Text>().enabled = false;
                selectBtn.GetComponentInChildren<Outline>().enabled = false;
                selectBtn.GetComponent<Image>().sprite = yellowBtnSprite;
            }
            else
            {
                selectBtn.SetActive(true);
                selectBtn.GetComponentInChildren<Text>().enabled = true;
                selectBtn.GetComponentInChildren<Outline>().enabled = true;
                selectBtn.GetComponent<Image>().sprite = greenBtnSprite;
            }
        }
        else if (!hasCharacter && !isIAP)
        {
            selectBtn.SetActive(false);
        }
        else if (!hasCharacter && isIAP)
        {
            iAPBtn.SetActive(true);
        }
    }
}
