using Breezorio.Ghosts;
using GamblingScripts;
using GamblingScripts.BasicComponents;
using GameEvents;
using UnityEngine;

public class GL_BaseGamblingMachine : GL_GamblingMachine
{
    protected override void Awake()
    {
        CoinHolder = GetComponentInChildren<GL_CoinHolder>();
    }

    protected override void Start()
    {
        
    }

    protected override void Update()
    {
        (this as GL_IStateMachine).DoAction();
        (this as GL_IStateMachine).DoCheckSwitch();
    }
}