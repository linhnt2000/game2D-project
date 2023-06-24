using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class DailyRewardItem : MonoBehaviour
{
    [SerializeField]
    private DailyRewardResourceItem firstDailyResourceItem;

    [SerializeField]
    private DailyRewardResourceItem secondDailyResourceItem;

    public int dayIndex;

    [SerializeField]
    private int rewardCount;

    [SerializeField]
    private Sprite selectedBackground;

    [SerializeField]
    private Sprite normalBackground;

    [SerializeField]
    private Image imgBackground;

    [SerializeField]
    private LocalizationPlaceHolderText placeHolderDayText;

    [SerializeField]
    private GameObject imgClaimCheckmark, tickCheck;

    List<DailyRewardResourceItem> drResourceItems;

    private void Awake()
    {
        drResourceItems = new List<DailyRewardResourceItem>();
        if (rewardCount > 0)
        {
            drResourceItems.Add(firstDailyResourceItem);
        }
        if (rewardCount > 1)
        {
            drResourceItems.Add(secondDailyResourceItem);
        }
    }

    private void Start()
    {
        placeHolderDayText.SetValue("DAY " + (dayIndex + 1).ToString());
        for (int i = 0; i < drResourceItems.Count; i++)
        {
            DailyRewardResourceItem drResourceItem = drResourceItems[i];
            ResourceItem resourceItem = drResourceItem.resourceItem;
            if (resourceItem.spriteUI != null)
            {
                drResourceItem.imgIcon.sprite = resourceItem.spriteUI;
            }
            else if (resourceItem.spine != null && drResourceItem.spineGraphic != null)
            {
                //if (resourceItem.type == ResourceType.Character && resourceItem.detail != ResourceDetail.CharacterAny) {
                //    CharacterInfo characterInfo = DataHolder.Instance.characters[drResourceItem.resourceItem.detail];
                //    drResourceItem.spineGraphic.initialSkinName = characterInfo.skinName;
                //    drResourceItem.spineGraphic.Initialize(true);
                //    drResourceItem.spineGraphic.startingAnimation = characterInfo.animName;
                //    drResourceItem.spineGraphic.Skeleton.SetBonesToSetupPose();
                //}
            }
            if (resourceItem.countable)
            {
                drResourceItem.txtQuantity.gameObject.SetActive(true);
                drResourceItem.txtQuantity.text = "x " + resourceItem.quantity.ToString();
            }
            else
            {
                drResourceItem.txtQuantity.gameObject.SetActive(false);
            }
        }
    }

    private void OnEnable()
    {
        int currentDailyRewardDayIndex = GameData.CurrentDailyRewardDayIndex;
        int lastDailyRewardDayIndex = GameData.LastDailyRewardDayIndex;
        if (dayIndex == currentDailyRewardDayIndex)
        {
            imgBackground.sprite = selectedBackground;
        }
        else
        {
            imgBackground.sprite = normalBackground;
        }
        if (dayIndex <= lastDailyRewardDayIndex && (lastDailyRewardDayIndex <= currentDailyRewardDayIndex))
        {
            imgClaimCheckmark.SetActive(true);
            tickCheck.SetActive(true);
            imgBackground.GetComponent<Image>().enabled = false;
        }
        else
        {
            imgClaimCheckmark.SetActive(false);
            tickCheck.SetActive(false);
            imgBackground.GetComponent<Image>().enabled = true;
        }
    }

    public void Claim(int factor)
    {
        List<ResourceItem> claimResourceItems = new List<ResourceItem>();
        foreach (DailyRewardResourceItem drResourceItem in drResourceItems)
        {
            if (drResourceItem.resourceItem.detail == ResourceDetail.None)
            {
                int realQuantity = drResourceItem.resourceItem.quantity * factor;
                ResourceItem claimResourceItem = (ResourceItem)(drResourceItem.resourceItem).Clone();
                claimResourceItem.quantity = realQuantity;
                claimResourceItems.Add(claimResourceItem);
                //if (drResourceItem.resourceItem.type == ResourceType.Gem)
                //{
                //    GameData.GemReceived += realQuantity;
                //}
            }
            else
            {
                claimResourceItems.Add(drResourceItem.resourceItem);
            }
        }
        GameData.ClaimResourceItems(claimResourceItems);
        //GameAnalytics.LogEarnVirtualCurrency("daily_reward", claimResourceItems);
        GameData.LastDailyRewardDayIndex = dayIndex;
        GameData.LastDailyRewardClaim = UnbiasedTime.Instance.Now();
        UIClaimResourcePanel.Setup(claimResourceItems).Show();
    }
}
