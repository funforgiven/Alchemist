using System.Collections;
using Alchemist.AI;

namespace Ability
{
    public class AbilityBase
    {
        public bool HasFinished;
        public bool HasStarted;
        public bool Result;

        protected CharacterStats Self { get; private set; }
        protected NodeController NodeController { get; private set; }
        
        public AbilityBase(CharacterStats self)
        {
            Self = self;
            NodeController = Self.GetComponent<NodeController>();
            
            HasStarted = false;
            HasFinished = true;
            Result = true;
        }

        public virtual IEnumerator OnAbilityUse()
        {
            HasStarted = false;
            HasFinished = true;
            Result = true;
            yield return null;
        }

        public virtual IEnumerator OnAbilityUse(params CharacterStats[] targets)
        {
            HasStarted = false;
            HasFinished = true;
            Result = true;
            yield return null;
        }

        public virtual void OnUpdate() { }
        
        public virtual bool CheckConditions() { return true; }
        public virtual bool CheckConditions(params CharacterStats[] targets) { return true; }
    }
}