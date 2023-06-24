using System.Collections;
using System.Collections.Generic;
using DarkTonic.MasterAudio;
using UnityEngine;
using UnityEngine.UI;

public class UIClaimResourcePanel : BaseBox
{

    private static UIClaimResourcePanel instance;

    [SerializeField]
    private GameObject horizontalClaimItems;

    [SerializeField]
    private GameObject claimResourceItemPrefab;
    [SerializeField]
    private GameObject fxConfetti;

    [SoundGroup]
    [SerializeField]
    private string greetingSound;

    private List<GameObject> resourceItemObjects = new List<GameObject>();


    public static UIClaimResourcePanel Setup(params ResourceItem[] items)
    {
        if (instance == null)
        {
            instance = Instantiate(Resources.Load<UIClaimResourcePanel>(Constants.PathPrefabs.CLAIM_RESOURCE_BOX));
        }
        instance.Init(items);
        return instance;
    }
    public override void Close()
    {
        base.Close();
    }
    public static UIClaimResourcePanel Setup(List<ResourceItem> items)
    {
        return Setup(items.ToArray());
    }
    protected override void OnStart()
    {
        base.OnStart();
        GetComponent<Canvas>().sortingOrder = 30;
    }

    public void Init(ResourceItem[] items)
    {
        RemovePreviousChildren();
        foreach (ResourceItem item in items)
        {
            GameObject obj = Instantiate(claimResourceItemPrefab);
            obj.transform.SetParent(horizontalClaimItems.transform, false);
            obj.GetComponent<UIClaimResourceItem>().AssignClaimResourceItem(item);
            resourceItemObjects.Add(obj);
        }
        MasterAudio.PlaySound(Constants.Audio.SOUND_PURCHASE_SUCCESS);
    }

    private void RemovePreviousChildren()
    {
        horizontalClaimItems.transform.DetachChildren();
        if (resourceItemObjects.Count > 0)
        {
            foreach (GameObject go in resourceItemObjects)
            {
                Destroy(go);
            }
            resourceItemObjects.Clear();
        }
    }

    public override void Show()
    {
        base.Show();
        MasterAudio.PlaySound(greetingSound);
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        fxConfetti.SetActive(true);
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        fxConfetti.SetActive(false);
    }
}