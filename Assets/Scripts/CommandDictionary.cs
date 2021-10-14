using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommendDictionary
{
    public List<KeyCode> commendForwardDash;
    public List<KeyCode> commendLeftDash;
    public List<KeyCode> commendBackdDash;
    public List<KeyCode> commendRightDash;

    Dictionary<List<KeyCode>, Action> commandDictionary;

    CommendDictionary()
    {
        commendForwardDash = new List<KeyCode> { KeyCode.W, KeyCode.W };
        commendLeftDash = new List<KeyCode> { KeyCode.A, KeyCode.A };
        commendBackdDash = new List<KeyCode> { KeyCode.S, KeyCode.S };
        commendRightDash = new List<KeyCode> { KeyCode.D, KeyCode.D };

        commandDictionary = new Dictionary<List<KeyCode>, Action>
        {
            {new List<KeyCode> { KeyCode.W, KeyCode.W }, ActionForwardDash},
            {new List<KeyCode> { KeyCode.W, KeyCode.W }, ActionForwardDash},
            {new List<KeyCode> { KeyCode.W, KeyCode.W }, ActionForwardDash},
            {new List<KeyCode> { KeyCode.W, KeyCode.W }, ActionForwardDash}
        };
    }

    Action ActionForwardDash = () =>
    {

    };

}
