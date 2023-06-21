using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Events;
//using UnityEngine.Purchasing;
//using UnityEngine.Purchasing.Security;

public class Helper : MonoBehaviour
{
    public static IEnumerator StartAction(UnityAction action, float timeDelay)
    {
        yield return new WaitForSeconds(timeDelay);
        action();
    }
    public static IEnumerator StartActionRealtimes(UnityAction action, float timeDelay)
    {
        yield return new WaitForSecondsRealtime(timeDelay);
        action();
    }

    public static IEnumerator StartAction(UnityAction action, System.Func<bool> condition)
    {
        yield return new WaitUntil(condition);
        action();
    }
    public static void LookAtToDirection(Vector3 diretion, GameObject objLookAt, float speedLockAt = 500)
    {
        float xTarget = diretion.x;
        float yTarget = diretion.y;
        float angle = Mathf.Atan2(yTarget, xTarget) * Mathf.Rad2Deg + 90;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        objLookAt.transform.rotation = Quaternion.Slerp(objLookAt.transform.rotation, q, Time.deltaTime * speedLockAt);
    }
    public static Vector3 GetPointDistanceFromObject(float distance, Vector3 direction, Vector3 fromPoint)
    {
        distance -= 1;
        //if (distance < 0)
        //    distance = 0;

        Vector3 finalDirection = direction + direction.normalized * distance;
        Vector3 targetPosition = fromPoint + finalDirection;

        return targetPosition;
    }
    public static class IListExtensions
    {
        /// <summary>
        /// Shuffles the element order of the specified list.
        /// </summary>
        public static void Shuffle(List<Transform> ts)
        {
            var count = ts.Count;
            var last = count - 1;
            for (var i = 0; i < last; ++i)
            {
                var r = UnityEngine.Random.Range(i, count);
                var tmp = ts[i];
                ts[i] = ts[r];
                ts[r] = tmp;
            }
        }

    }

    //public static bool VerifyIap(Product product)
    //{
    //    bool validPurchase = true;
    //    //#if (UNITY_ANDROID || UNITY_IOS || UNITY_STANDALONE_OSX) && !UNITY_EDITOR
    //    // Prepare the validator with the secrets we prepared in the Editor
    //    // obfuscation window.
    //    //var validator = new CrossPlatformValidator(GooglePlayTangle.Data(),
    //    //    AppleTangle.Data(), Application.identifier);

    //    //try
    //    //{
    //    //    // On Google Play, result has a single product ID.
    //    //    // On Apple stores, receipts contain multiple products.
    //    //    var result = validator.Validate(product.receipt);
    //    //    // For informational purposes, we list the receipt(s)
    //    //    Debug.Log("Receipt is valid. Contents:");
    //    //    //foreach (IPurchaseReceipt productReceipt in result)
    //    //    //{
    //    //    //    Debug.Log(productReceipt.productID);
    //    //    //    Debug.Log(productReceipt.purchaseDate);
    //    //    //    Debug.Log(productReceipt.transactionID);
    //    //    //}
    //    //}
    //    //catch (IAPSecurityException)
    //    //{
    //    //    Debug.Log("Invalid receipt, not unlocking content");
    //    //    validPurchase = false;
    //    //}
    //    //#endif
    //    //GameAnalytics.LogFirebaseUserProperty(GameAnalytics.USER_PROPERTIES_IAP_USER, validPurchase);
    //    GameData.IsIAPUser = validPurchase;
    //    return validPurchase;
    //}


    public static bool isAllDigit(string str)
    {
        for (int i = 0; i < str.Length; i++)
        {
            if (!Char.IsDigit(str[i]))
            {
                return false;
            }
        }
        return true;
    }

    public static string getDigitOnly(string str)
    {
        Regex regexObj = new Regex(@"[^\d]");
        return regexObj.Replace(str, "");
    }

    public static string getNonWhiteSpace(string str)
    {
        Regex regex = new Regex(@"[\s]");
        return regex.Replace(str, "");
    }

    public static string getFormattedTimeFromSeconds(long seconds)
    {
        if (seconds > 3600)
        {
            return string.Format("{0:00}:{1:00}:{2:00}", seconds / 3600, (seconds / 60) % 60, seconds % 60);
        }
        else
        {
            return string.Format("{0:00}:{1:00}", seconds / 60, seconds % 60);
        }
    }

    public static int RandomByWeight(int[] weights)
    {
        int sum = 0;
        for (int i = 0; i < weights.Length; i++)
        {
            sum += weights[i];
        }
        int rand = UnityEngine.Random.Range(0, sum) + 1;
        for (int i = 0; i < weights.Length; i++)
        {
            if (rand <= weights[i] && weights[i] > 0)
            {
                return i;
            }
            rand -= weights[i];
        }
        return 0;
    }

    public static int RandomByWeightStrictly(int[] weights)
    {
        int sum = 0;
        for (int i = 0; i < weights.Length; i++)
        {
            sum += weights[i];
        }
        int rand = UnityEngine.Random.Range(0, sum) + 1;
        for (int i = 0; i < weights.Length; i++)
        {
            if (rand <= weights[i] && weights[i] > 0)
            {
                return i;
            }
            rand -= weights[i];
        }
        return 1;
    }

    public static bool FromIOS145()
    {
#if UNITY_IOS
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            Version version = new Version(UnityEngine.iOS.Device.systemVersion);
            Version ios145Version = new Version("14.5");
            if (version >= ios145Version)
            {
                return true;
            }
        }
#endif
        return false;
    }
    public static bool FromIOS14()
    {
#if UNITY_IOS
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            Version version = new Version(UnityEngine.iOS.Device.systemVersion);
            Version ios145Version = new Version("14.0");
            if (version >= ios145Version)
            {
                return true;
            }
        }
        else {
            return true;
        }
#endif
        return false;
    }
    public static bool TimeOnDay(DateTime newDate, DateTime oldDate)
    {
        TimeSpan timeSpan = newDate - oldDate;
        return (int)timeSpan.TotalDays > 0;
    }
}