using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class HealSelfLeafTask : LeafTask
    {
        public HealSelfLeafTask(int healThreshold) : base("Heal Self")
        {
            this.healThreshold = healThreshold;
        }

        public HealSelfLeafTask(int healThreshold, ExecutionContext context) : base("Heal Self", context)
        {
            this.healThreshold = healThreshold;
        }

        protected override TaskStatus PerformAction()
        {
            if (IsStatusWaiting())
                InitializeTask();

            if (context == null)
            {
                SetStatus(TaskStatus.Failure);
                return GetStatus();
            }

            BotInfo bot = context.Self;
            bot.SetHealth(bot.Health + healRate);

            if (bot.Health >= healThreshold)
            {
                SetStatus(TaskStatus.Success);
            }

            return GetStatus();
        }

        private float healRate = 1f;
        private int healThreshold;
    }
}