using System.Collections;
using System.Collections.Generic;
using Spine.Unity;
using UnityEngine;
using UnityEngine.UI;

public class UIClaimResourceItem : MonoBehaviour
{
    public GameObject staticImage;
    public GameObject spineAnim;

    [SerializeField]
    private Text txtQuantity;

    public void AssignClaimResourceItem(ResourceItem item)
    {
        if (item.type != ResourceType.Character)
        {
            spineAnim.SetActive(false);
            staticImage.SetActive(true);
            if (item.spriteUI != null)
            {
                staticImage.GetComponent<Image>().sprite = item.spriteUI;
            }
            else
            {
                Sprite spr = DataHolder.Instance.defaultResourceTypeSprites[item.type];
                if (spr != null)
                {
                    staticImage.GetComponent<Image>().sprite = spr;
                }
            }
            if (item.countable)
            {
                txtQuantity.gameObject.SetActive(true);
                txtQuantity.text = item.quantity.ToString();
            }
            else
            {
                txtQuantity.gameObject.SetActive(false);
            }
        }
        else
        {
            spineAnim.SetActive(true);
            staticImage.SetActive(false);
            if (item.countable)
            {
                txtQuantity.gameObject.SetActive(true);
                txtQuantity.text = item.quantity.ToString();
            }
            else
            {
                txtQuantity.gameObject.SetActive(false);
            }
            CharacterInfo characterInfo = DataHolder.Instance.characters[item.detail];
            if (characterInfo != null)
            {
                spineAnim.transform.localPosition = new Vector3(0, -150, 0);
                spineAnim.transform.localScale = new Vector3(3.4f, 3.4f, 1);
                SkeletonGraphic skeletonGraphic = spineAnim.GetComponent<SkeletonGraphic>();
                skeletonGraphic.skeletonDataAsset = characterInfo.skeletonDataAsset;
                skeletonGraphic.initialSkinName = characterInfo.skinName;
                skeletonGraphic.Initialize(true);
                //skeletonGraphic.allowMultipleCanvasRenderers = true;
                skeletonGraphic.AnimationState.SetAnimation(0, characterInfo.animName, true);
                skeletonGraphic.Skeleton.SetSlotsToSetupPose();
                //skeletonGraphic.AnimationState.Apply(skeletonGraphic.Skeleton);
            }
        }
    }
}
