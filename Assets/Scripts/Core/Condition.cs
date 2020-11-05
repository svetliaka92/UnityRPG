using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    [System.Serializable]
    public class Condition
    {
        [SerializeField] Disjunction[] and;

        public bool Check(IEnumerable<IPredicateEvaluator> evaluators)
        {
            foreach (Disjunction dis in and)
                if (!dis.Check(evaluators))
                    return false;

            return true;
        }

        [System.Serializable]
        class Disjunction
        {
            [SerializeField] Predicate[] or;

            public bool Check(IEnumerable<IPredicateEvaluator> evaluators)
            {
                foreach (Predicate pred in or)
                    if (pred.Check(evaluators))
                        return true;

                return false;
            }
        }

        [System.Serializable]
        class Predicate
        {
            [SerializeField] string predicate;
            [SerializeField] bool isNegated = false;
            [SerializeField] string[] parameters;

            public bool Check(IEnumerable<IPredicateEvaluator> evaluators)
            {
                foreach (IPredicateEvaluator evaluator in evaluators)
                {
                    bool? result = evaluator.Evaluate(predicate, parameters);
                    if (result == null)
                        continue;

                    if (result == isNegated)
                        return false;
                }

                return true;
            }
        }
    }
}
