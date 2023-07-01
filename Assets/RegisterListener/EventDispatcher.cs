using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;



public class EventDispatcher : MonoBehaviour
{
    #region Singleton
    static EventDispatcher s_instance;
    public static EventDispatcher Instance
    {
        get
        {
            // instance not exist, then create new one
            if (s_instance == null)
            {
                // create new Gameobject, and add EventDispatcher component
                GameObject singletonObject = new GameObject();
                s_instance = singletonObject.AddComponent<EventDispatcher>();
                singletonObject.name = "Singleton - EventDispatcher";
                Commons.Log("Create singleton : {0}", singletonObject.name);
            }
            return s_instance;
        }
        private set { }
    }

    public static bool HasInstance()
    {
        return s_instance != null;
    }

    void Awake()
    {
        // check if there's another instance already exist in scene
        if (s_instance != null && s_instance.GetInstanceID() != this.GetInstanceID())
        {
            // Destroy this instances because already exist the singleton of EventsDispatcer
            Commons.Log("An instance of EventDispatcher already exist : <{1}>, So destroy this instance : <{2}>!!", s_instance.name, name);
            Destroy(gameObject);
        }
        else
        {
            // set instance
            s_instance = this as EventDispatcher;
        }

       //DontDestroyOnLoad(this);
    }


    void OnDestroy()
    {
        // reset this static var to null if it's the singleton instance
        if (s_instance == this)
        {
            ClearAllListener();
            s_instance = null;
        }
    }
    #endregion


    #region Fields
    /// Store all "listener"
    Dictionary<EventID, Action<object>> _listeners = new Dictionary<EventID, Action<object>>();
    #endregion


    #region Add Listeners, Post events, Remove listener

    /// <summary>
    /// Register to listen for eventID
    /// </summary>
    /// <param username="eventID">EventID that object want to listen</param>
    /// <param username="callback">Callback will be invoked when this eventID be raised</param>
    public void RegisterListener(EventID eventID, Action<object> callback)
    {
        // checking params
        Commons.Assert(callback != null, "AddListener, event {0}, callback = null !!", eventID.ToString());
        Commons.Assert(eventID != EventID.None, "RegisterListener, event = None !!");

        // check if listener exist in distionary
        if (_listeners.ContainsKey(eventID))
        {
            // add callback to our collection
            _listeners[eventID] += callback;
        }
        else
        {
            // add new key-value pair
            _listeners.Add(eventID, null);
            _listeners[eventID] += callback;
        }
    }

    /// <summary>
    /// Posts the event. This will notify all listener that register for this event
    /// </summary>
    /// <param username="eventID">EventID.</param>
    /// <param username="sender">Sender, in some case, the Listener will need to know who send this message.</param>
    /// <param username="param">Parameter. Can be anything (struct, class ...), Listener will make a cast to get the data</param>
    public void PostEvent(EventID eventID, object param = null)
    {
        if (!_listeners.ContainsKey(eventID))
        {
            Commons.Log("No listeners for this event : {0}", eventID);
            return;
        }

        // posting event
        var callbacks = _listeners[eventID];
        // if there's no listener remain, then do nothing
        if (callbacks != null)
        {
            callbacks(param);
        }
        else
        {
            Commons.Log("PostEvent {0}, but no listener remain, Remove this key", eventID);
            _listeners.Remove(eventID);
        }
    }

    /// <summary>
    /// Removes the listener. Use to Unregister listener
    /// </summary>
    /// <param username="eventID">EventID.</param>
    /// <param username="callback">Callback.</param>
    public void RemoveListener(EventID eventID, Action<object> callback)
    {
        // checking params
        Commons.Assert(callback != null, "RemoveListener, event {0}, callback = null !!", eventID.ToString());
        Commons.Assert(eventID != EventID.None, "AddListener, event = None !!");

        if (_listeners.ContainsKey(eventID))
        {
            _listeners[eventID] -= callback;
        }
        else
        {
          //  Commons.Warning(false, "RemoveListener, not found key : " + eventID);
        }
    }

    /// <summary>
    /// Clears all the listener.
    /// </summary>
    public void ClearAllListener()
    {
        _listeners.Clear();
    }
    #endregion
}


#region Extension class
/// <summary>
/// Delare some "shortcut" for using EventDispatcher easier
/// </summary>
public static class EventDispatcherExtension
{
    /// Use for registering with EventsManager
    public static void RegisterListener(this MonoBehaviour listener, EventID eventID, Action<object> callback)
    {
        EventDispatcher.Instance.RegisterListener(eventID, callback);
    }

    /// Post event with param
    public static void PostEvent(this MonoBehaviour listener, EventID eventID, object param)
    {
        EventDispatcher.Instance.PostEvent(eventID, param);
    }

    /// Post event with no param (param = null)
    public static void PostEvent(this MonoBehaviour sender, EventID eventID)
    {
        EventDispatcher.Instance.PostEvent(eventID, null);
    }

    /// Post event with no param (param = null)
    public static void RemoveListener(this MonoBehaviour sender, EventID eventID, Action<object> callback)
    {
        EventDispatcher.Instance.RemoveListener(eventID, callback);
    }
}
#endregion
