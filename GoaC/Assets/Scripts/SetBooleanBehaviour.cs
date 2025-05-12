using UnityEngine;

public class SetBooleanBehaviour : StateMachineBehaviour
{
    public string boolName;
    public bool onState;
    public bool updateOnStateMachine;
    public bool valueOnEnter, valueOnExit;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       if (updateOnStateMachine)
        {
            animator.SetBool(boolName, valueOnExit);

        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (updateOnStateMachine)
        {
            animator.SetBool(boolName, valueOnExit);
        }
    }

}
