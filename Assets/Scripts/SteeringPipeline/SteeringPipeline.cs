using GameAI.Actions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameAI.AgentCore;

namespace GameAI.SteeringPipeline
{
    public class SteeringPipeline : Wander
    {
        public int constraintSteps = 3;
        Targeter[] targeters;
        Decomposer[] decomposers;
        Constraint[] constraints;
        Actuator actuator;

        private void Start()
        {
            targeters = GetComponents<Targeter>();
            decomposers = GetComponents<Decomposer>();
            constraints = GetComponents<Constraint>();
            actuator = GetComponent<Actuator>();
        }

        public override Steering GetSteering()
        {
            Goal goal = new Goal();

            for(int i = 0; i < targeters.Length; ++i)
                goal.UpdateChannels(targeters[i].GetGoal());
            for (int i = 0; i < decomposers.Length; ++i)
                goal = decomposers[i].Decompose(goal);

            for (int i = 0; i < constraintSteps; ++i)
            {
                Path path = actuator.GetPath(goal);
                for (int j = 0; j < constraints.Length; ++j)
                {
                    if (constraints[j].WillViolate(path))
                    {
                        goal = constraints[i].Suggest(path);
                        break;
                    }
                    return actuator.GetOutput(path, goal);
                }
            }

            return base.GetSteering();
        }
    }
}