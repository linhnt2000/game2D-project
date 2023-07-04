using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupComingSoon : BaseBox
{
    public static PopupComingSoon instance;
    public static PopupComingSoon Setup()
    {
        if (instance == null)
        {
            instance = Instantiate(Resources.Load<PopupComingSoon>(Constants.PathPrefabs.POPUP_COMING_SOON));
        }
        return instance;
    }
}
