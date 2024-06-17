using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CombatStatics;

public class Combatant : MonoBehaviour
{
    public int hP;
    public Allegiance side;
    public CombatAction desiredAction;
    public CombatNode myNode;

    public enum Allegiance { 
        neutral, 
        friendly, 
        hostile
    }

	private void Start()
	{
        //test
        //snap to nearest grid and occupy
        myNode = NodeAtPosition(WorldToGrid(transform.position));
        MoveTo(GridToWorld(myNode.position));
        myNode.occupant = this;
	}

	public virtual void Hit(int damage) {
        hP -= damage;
        if(hP < 1) {
            Kill();
	    }
    }

    public virtual void Kill() {
        //Stuff to call before object destroyed
        //IK unity and C# have built-in destructors but whatever

        Destroy(gameObject);
    }

    public virtual void MoveTo(Vector2 worldPos) {
        transform.position = worldPos;
    }
}
