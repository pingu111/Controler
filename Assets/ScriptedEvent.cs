using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScriptedEvent
{
    /// <summary>
    /// The type of event : spike in, out, ...
    /// </summary>
    public TypeScriptedEvent typeOfEvent;

    /// <summary>
    /// The time to launch the event
    /// </summary>
    public float timeToLaunchEvent = 0;

    /// <summary>
    /// The id of the spike or the text
    /// </summary>
    public int id = 0;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="__type"></param>
    /// <param name="__time"></param>
    /// <param name="__id"></param>
    ScriptedEvent(TypeScriptedEvent __type, float __time, int __id)
    {
        this.typeOfEvent = __type;
        this.timeToLaunchEvent = __time;
        this.id = __id;
    }
}

