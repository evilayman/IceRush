namespace VRTK.Examples.Archery
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class Follow : MonoBehaviour
    {
        public bool followPosition;
        public bool followRotation;
        public List<Transform> myTargets;
        private Transform target;

        private void Start()
        {
            StartCoroutine(getTarget(0.2f));
        }

        IEnumerator getTarget(float time)
        {
            yield return new WaitForSeconds(time);

            for (int i = 0; i < myTargets.Count; i++)
            {
                if (myTargets[i] && myTargets[i].gameObject.activeInHierarchy)
                {
                    target = myTargets[i];
                    break;
                }
            }
        }

        private void Update()
        {
            if (target != null)
            {
                if (followRotation)
                    transform.rotation = target.rotation;

                if (followPosition)
                    transform.position = target.position;
            }
        }
    }
}