using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Unit : MonoBehaviour {
	public Coord coord;
	public bool active = false;
	[SerializeField]SpriteRenderer spriteRenderer;
	Collider col;
	public UnitAttributes attributes;
	public bool hasMoved = false;
	public UnityEvent onSelected => GetComponent<IClickable>().onClick;

	void Awake(){
		col = GetComponent<Collider>();
	}

	public void Setup(){
		transform.position = MapController.instance.map.CoordToWorldPoint(coord);
		spriteRenderer.sprite = attributes.sprite;
		SetHasMoved(false);
	}

	public void StartTurn(){
		SetHasMoved(false);
	}

	public void EndTurn(){
		SetHasMoved(false);
	}

	public void Move(Coord p){
		coord = p;
		transform.position = MapController.instance.map.CoordToWorldPoint(coord);
		SetHasMoved(true);
	}

	public void Die(){
		Destroy(gameObject);
	}

	public void SetCollider(bool b){
		col.enabled = b;
	}

	public void SetHasMoved(bool b){
		hasMoved = b;
		spriteRenderer.color = b ? Color.gray : Color.white;
	}
}
