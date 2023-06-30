using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PanelShopController : BaseBox
{
    public static PanelShopController instance;
    [SerializeField] GameObject panelComboPack;
    [SerializeField] GameObject panelGemPack;
    [SerializeField] GameObject panelItemPack;
    [SerializeField] GameObject panelCharacterPack;
    [SerializeField] Button btnItemPack;
    [SerializeField] Button btnComboPack;
    [SerializeField] Button btnGemPack;
    [SerializeField] Button btnCharacterPack;
    [SerializeField] GameObject btnSelectItemPack;
    [SerializeField] GameObject btnSelectComboPack;
    [SerializeField] GameObject btnSelectGemPack;
    [SerializeField] GameObject btnSelectCharacterPack;
    [SerializeField] Button btnSubVip, btnRemoveAds;
    [SerializeField] GameObject outOfGem;
    [SerializeField] GameObject shopBtn;

    private string screenName;

    public static PanelShopController Setup()
    {
        if (instance == null)
        {
            instance = Instantiate(Resources.Load<PanelShopController>(Constants.PathPrefabs.PANEL_SHOP));
        }
        return instance;
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        CheckBtnStatus();
        if (GameData.inGame)
        {
            Time.timeScale = 0;
        }
        outOfGem.SetActive(false);
        screenName = SceneManager.GetActiveScene().name;
        if (screenName == Constants.SCENE_NAME.SCENE_GAMEPLAY)
        {
            btnCharacterPack.gameObject.SetActive(false);
            shopBtn.GetComponent<HorizontalLayoutGroup>().spacing = -190;
        }
        else
        {
            btnCharacterPack.gameObject.SetActive(true);
            shopBtn.GetComponent<HorizontalLayoutGroup>().spacing = -55;
        }
    }
    public void CheckBtnStatus()
    {
        if (GameData.vip)
        {
            btnSubVip.interactable = false;
            btnRemoveAds.interactable = false;
        }
        if (GameData.removeAds)
        {
            btnRemoveAds.interactable = false;
        }
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        if (GameData.inGame)
        {
            Time.timeScale = 1;
        }
    }
    public void SelectTapPack(GameObject panel)
    {
        panelComboPack.SetActive(false);
        panelGemPack.SetActive(false);
        panelItemPack.SetActive(false);
        panelCharacterPack.SetActive(false);
        panel.SetActive(true);
    }
    public void TapButtonPack(Button btn)
    {
        btnComboPack.interactable = true;
        btnGemPack.interactable = true;
        btnItemPack.interactable = true;
        btnCharacterPack.interactable = true;
        btn.interactable = false;
    }
    public void TapButtonSelect(GameObject btn)
    {
        btnSelectComboPack.SetActive(false);
        btnSelectGemPack.SetActive(false);
        btnSelectItemPack.SetActive(false);
        btnSelectCharacterPack.SetActive(false);
        btn.SetActive(true);
    }
    public void ShowTapCombo()
    {
        //GameAnalytics.LogUIAppear("tab_combo", screenName);
        SelectTapPack(panelComboPack);
        TapButtonPack(btnComboPack);
        TapButtonSelect(btnSelectComboPack);
    }
    public void ShowTapGem()
    {
        //GameAnalytics.LogUIAppear("tab_gem", screenName);
        SelectTapPack(panelGemPack);
        TapButtonPack(btnGemPack);
        TapButtonSelect(btnSelectGemPack);
    }
    public void ShowTapItem()
    {
        //GameAnalytics.LogUIAppear("tab_item", screenName);
        SelectTapPack(panelItemPack);
        TapButtonPack(btnItemPack);
        TapButtonSelect(btnSelectItemPack);
    }
    public void ShowTabCharacter()
    {
        //GameAnalytics.LogUIAppear("tab_character", screenName);
        SelectTapPack(panelCharacterPack);
        TapButtonPack(btnCharacterPack);
        TapButtonSelect(btnSelectCharacterPack);
    }
    public void ShowVip()
    {
        //GameAnalytics.LogUIAppear("popup_vip_package_shop", screenName);
        //UIVipController.Setup().Show();
    }
    public void OutOfGem()
    {
        outOfGem.GetComponent<Animator>().Rebind();
        outOfGem.SetActive(true);
        ShowTapGem();
    }
}
