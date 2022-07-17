using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Unit : MonoBehaviour {
	public string team;
	public Coord coord;
	public bool active = false;
	[SerializeField] SpriteRenderer spriteRenderer;
	Collider col;
	public UnitAttributes attributes;
	public bool hasMoved = false;
	public UnityEvent onSelected => GetComponent<IClickable>().onClick;
	public int max_hp => attributes.hp;
	public int hp;
	WorldHUD hud;
	public bool alive = true;

	void Awake(){
		hud = GetComponentInChildren<WorldHUD>();
		col = GetComponent<Collider>();
		hp = max_hp;
		hud.hp_fill = 1;
	}

	public void Setup(){
		transform.position = MapController.instance.map.CoordToWorldPoint(coord);
		MapController.instance.map[coord].unit = this;
		spriteRenderer.sprite = attributes.sprite;
		SetHasMoved(false);
		team = GetComponentInParent<TurnController>().controllerName;
	}

	public void StartTurn(){
		SetHasMoved(false);
	}

	public void EndTurn(){
		SetHasMoved(false);
	}

	public bool Move(Coord p){
		var map = MapController.instance.map;
		if(map[p].unit != null){
			if(map[p].unit == this){
				print("hell yeah");
				SetHasMoved(true);
				return true;
			}
			Debug.LogError("Already occupied");
			return false;
		}
		map[coord].unit = null;
		coord = p;
		map[coord].unit = this;
		transform.position = MapController.instance.map.CoordToWorldPoint(coord);
		SetHasMoved(true);

		return true;
	}

	public void Die(){
		alive = false;
		gameObject.SetActive(false);
		MapController.instance.map[coord].unit = null;
	}

	public void SetCollider(bool b){
		col.enabled = b;
	}

	public void SetHasMoved(bool b){
		hasMoved = b;
		spriteRenderer.color = b ? Color.gray : Color.white;
	}

	public void TakeDamage(int damage){
		if(!alive) return;
		hp -= damage;
		hp = Mathf.Max(0, hp);
		hud.hp_fill = hp / (float)max_hp;
		if(hp <= 0){
			Die();
		}
	}
}
