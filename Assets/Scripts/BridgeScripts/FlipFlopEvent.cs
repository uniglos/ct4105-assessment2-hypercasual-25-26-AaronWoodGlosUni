using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FlipFlopEvent : MonoBehaviour {

    [Tooltip("Which event will start first (0 is the first event of the list)")]
    [SerializeField] int startingEventNumberID;
    [SerializeField] bool startEventOnStart;
    public List<EventsList> flopEvents = new List<EventsList>();
    int currentID;

    private void Start() {
        currentID = startingEventNumberID;
        print(currentID);
        if (startEventOnStart) { flopEvents[currentID].FlipFlopEvent.Invoke(); }
    }

    public void PlayNextEvent() {
        FlipFlop(1);
    }
    public void PlayPreviousEvent() {
        FlipFlop(-1);
    }
    public void PlayEventXStepsAway(int stepInt) {
        FlipFlop(stepInt);
    }
    public void PlayEventByID(int EventID) {
        currentID = EventID;
        try {
            flopEvents[currentID].FlipFlopEvent.Invoke();
        } catch { }
    }

    private void FlipFlop(int IntChange) {
        currentID += IntChange;
        if (currentID > flopEvents.Count - 1) { 
            currentID -= flopEvents.Count;
        } else if (currentID < 0) {
            currentID += flopEvents.Count;
        }
        flopEvents[currentID].FlipFlopEvent.Invoke();
    }

    private void OnValidate() {
        if (startingEventNumberID > flopEvents.Count - 1) {
            startingEventNumberID = flopEvents.Count - 1;
        } else if (startingEventNumberID < 0) {
            startingEventNumberID = 0;
        }
    }
}


