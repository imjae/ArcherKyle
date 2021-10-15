using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommendDictionary
{
    private Dictionary<List<KeyCode>, Action> _commandDictionary;
    public Dictionary<List<KeyCode>, Action> CommandDictionary { get => this._commandDictionary; }

    CommendDictionary()
    {
        this._commandDictionary = new Dictionary<List<KeyCode>, Action>
        {
            {new List<KeyCode> { KeyCode.W, KeyCode.W }, ActionForwardDash},
            {new List<KeyCode> { KeyCode.S, KeyCode.S }, ActionBackDash},
            {new List<KeyCode> { KeyCode.A, KeyCode.A }, ActionLeftDash},
            {new List<KeyCode> { KeyCode.D, KeyCode.D }, ActionRightDash}
        };
    }

    Action ActionForwardDash = () =>
    {

    };
    Action ActionBackDash = () =>
    {

    };
    Action ActionLeftDash = () =>
    {

    };
    Action ActionRightDash = () =>
    {

    };

}
