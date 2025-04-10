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
		IAstarAI ai;
		public bool isConfused;
		public List<GameObject> targets;
		public CurrentState currentState = CurrentState.normal;
		public static List<GameObject> players = new List<GameObject>();

  

        void OnEnable () {
			ai = GetComponent<IAstarAI>();
			// Update the destination right before searching for a path as well.
			// This is enough in theory, but this script will also update the destination every
			// frame as the destination is used for debugging and may be used for other things by other
			// scripts as well. So it makes sense that it is up to date every frame.
			if (ai != null) ai.onSearchPath += Update;
		}
		public enum CurrentState
		{
			normal,
			retreating,
            frozen
		}
		void OnDisable () {
			if (ai != null) ai.onSearchPath -= Update;
		}
        
        /// <summary>Updates the AI's destination every frame</summary>
        void Update () {
            getTarget();
			if (target != null && ai != null) ai.destination = target.position;
		}
		private void getTarget()
        {
            if (currentState == CurrentState.normal)
            {
                getPlayerTarget();
            }

        }

        private void getPlayerTarget()
        {
                if (players.Count == 1)
                {
                    target = players[0].transform;
                    return;
                }
                Vector2 p1 = players[0].transform.position - transform.position;
                Vector2 p2 = players[1].transform.position - transform.position;
                if (p1.sqrMagnitude < p2.sqrMagnitude)
                {
                    target = players[0].transform;
                    return;
                }
                target = players[1].transform;
        }
    }
}
