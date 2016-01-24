using UnityEngine;
using System.Collections;

public class Generic_Team : MonoBehaviour 
{
	ushort myTeamNum;
	public ushort TeamNum
	{
		get
		{
			return myTeamNum;
		}
		set
		{
			myTeamNum = value;
		}
	}
}
