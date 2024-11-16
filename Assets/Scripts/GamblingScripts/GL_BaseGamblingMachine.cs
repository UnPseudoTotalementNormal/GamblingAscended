using Breezorio.Ghosts;
using GamblingScripts;

public class GL_BaseGamblingMachine : GL_GamblingMachine
{
    protected override void Awake()
    {
        
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
