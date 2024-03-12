using UnityEngine;
using System.Collections;
using System.Diagnostics.Tracing;
using System.Collections.Generic;

namespace Pathfinding {
	/// <summary>
	/// Sets the destination of an AI to the position of a specified object.
	/// This component should be attached to a GameObject together with a movement script such as AIPath, RichAI or AILerp.
	/// This component will then make the AI move towards the <see cref="target"/> set on this component.
	///
	/// See: <see cref="Pathfinding.IAstarAI.destination"/>
	///
	/// [Open online documentation to see images]
	/// </summary>
	[UniqueComponent(tag = "ai.destination")]
	[HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_a_i_destination_setter.php")]
	public class AIDestinationSetter : VersionedMonoBehaviour {
		/// <summary>The object that the AI should move to</summary>
		public Transform target;
		public Transform player2;
		public bool coop;
		public Transform player1;
		IAstarAI ai;
		public bool isConfused;
		public List<GameObject> targets;
  

        void OnEnable () {
			ai = GetComponent<IAstarAI>();
			// Update the destination right before searching for a path as well.
			// This is enough in theory, but this script will also update the destination every
			// frame as the destination is used for debugging and may be used for other things by other
			// scripts as well. So it makes sense that it is up to date every frame.
			if (ai != null) ai.onSearchPath += Update;
		}

		void OnDisable () {
			if (ai != null) ai.onSearchPath -= Update;
		}
        private GameObject GetTarget()
        {
            GameObject closest = null;
            foreach (GameObject enemy in targets)
            {
                if (enemy != null && enemy != gameObject)
                {
                    if (closest == null)
                    {
                        closest = enemy;
                        continue;
                    }
                    Vector2 EnemyLocation = enemy.transform.position - transform.position;
                    Vector2 closestLocation = closest.transform.position - transform.position;
                    if (EnemyLocation.sqrMagnitude < closestLocation.sqrMagnitude)
                    {
                        closest = enemy;
                    }
                }
            }
            return closest;
        }
        /// <summary>Updates the AI's destination every frame</summary>
        void Update () {
			if (isConfused)
			{
				target = GetTarget().transform;
			}
			if (coop && !isConfused)
			{
				if (!player1)
				{
					target = player2;
				}
				else if (!player2)
				{
					target = player1;
				}
				else
				{


					Vector2 p1 = player1.position - transform.position;
					Vector2 p2 = player2.position - transform.position;
					if (p1.sqrMagnitude < p2.sqrMagnitude)
					{
						target = player1;
					}
					else
					{
						target = player2;
					}
				}
			}
			if (target != null && ai != null) ai.destination = target.position;
		}
	}
}
