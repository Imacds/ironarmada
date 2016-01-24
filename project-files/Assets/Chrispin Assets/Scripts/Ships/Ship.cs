using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Ship
{
	public uint id;
	public List<ShipPart> parts;
	public Vector2 pos, vel;
	public float angle, angle_vel;
	public Vector2 old_pos, old_vel;
	public float old_angle;
	public float mass, carryMass;
	public GameObject centerPart;
	public bool initialized;	
	public uint soundsPlayed;
	public string owner;
	public bool isMothership;

	Vector2 net_pos, net_vel;
	float net_angle, net_angle_vel;

	public Ship()
	{
		angle = angle_vel = old_angle = mass = carryMass = 0.0f;
		initialized = false;
		isMothership = false;
		@centerPart = null;
		soundsPlayed = 0;
		owner = "";
	}
}
