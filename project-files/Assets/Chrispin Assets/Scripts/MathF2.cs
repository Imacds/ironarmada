using UnityEngine;
using System.Collections;

public class MathF2 : MonoBehaviour 
{
	public static Vector2 RotateVector2( Vector2 v, float angle )
	{
		float sin = Mathf.Sin(angle);
		float cos = Mathf.Cos(angle);

		float tx = v.x;
		float ty = v.y;
		v.x = (cos*tx) - (sin*ty);
		v.y = (cos*ty) + (sin*tx);

		return v;
	}
}
