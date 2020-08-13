using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

[RequireComponent(typeof(BotInfo))]
public class BTTest : MonoBehaviour
{
    #region PUBLIC
    #endregion
    #region PROTECTED
    #endregion
    #region PRIVATE
    private ExecutionContext context = new ExecutionContext();
    private Task myBehaviourTree;

    private void Start()
    {
        context.SetSelf(GetComponent<BotInfo>());
        context.SetTarget(FindObjectOfType<BehaviourTree.PlayerInfo>());

        myBehaviourTree = CreateBehaviourTree();
    }

    private void FixedUpdate()
    {
        if (myBehaviourTree != null)
        {
            myBehaviourTree.Update(context);
            Debug.Log(myBehaviourTree.Debug());
        }
    }

    private Task CreateBehaviourTree()
    {
        SelectorCompositeTask findTargetSelector = new SelectorCompositeTask("Find Target");
        findTargetSelector.AddChild(new FindNemesisLeafTask());
        findTargetSelector.AddChild(new FindClosestTargetLeafTask());

        SequenceCompositeTask combatSequence = new SequenceCompositeTask("Combat");
        combatSequence.AddChild(findTargetSelector);
        combatSequence.AddChild(new SetTargetPositionAsDestinationLeafTask());
        combatSequence.AddChild(new MoveLeafTask());
        combatSequence.AddChild(new AttackTargetLeafTask());
        
        SequenceCompositeTask fleeSequence = new SequenceCompositeTask("Flee");
        fleeSequence.AddChild(new FindRandomPositionLeafTask());
        fleeSequence.AddChild(new MoveLeafTask());

        SequenceCompositeTask healSequence = new SequenceCompositeTask("Heal");
        healSequence.AddChild(new WaitLeafTask(3));
        healSequence.AddChild(new HealSelfLeafTask(100));

        ParallelCompositeTask fleeAndHealParallelTask = new ParallelCompositeTask("Flee And Heal");
        fleeAndHealParallelTask.AddChild(fleeSequence);
        fleeAndHealParallelTask.AddChild(healSequence);
        
        SelectorCompositeTask fightOrFlightSelector = new SelectorCompositeTask("Fight Or Flight");
        fightOrFlightSelector.AddChild(combatSequence);
        fightOrFlightSelector.AddChild(fleeSequence);

        RepeaterDecoratorTask repeaterDecorator = 
            new RepeaterDecoratorTask(fightOrFlightSelector);

        return repeaterDecorator;
    }
    #endregion
}
