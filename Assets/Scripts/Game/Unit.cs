using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Unit : MonoBehaviour {
	public string team;
	public int teamID;
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
		spriteRenderer.color = Color.white;
	}

	public void Setup(){
		transform.position = MapController.instance.map.CoordToWorldPoint(coord);
		MapController.instance.map[coord].unit = this;
		spriteRenderer.sprite = attributes.sprite;
		SetHasMoved(false);
		var cont = GetComponentInParent<TurnController>();
		team = cont.controllerName;
		teamID = cont.teamID;
		hud.hudColor = cont.controllerColor;
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
		if(!alive) return;
		alive = false;
		gameObject.SetActive(false);
		MapController.instance.map[coord].unit = null;
		SFXPlayer.instance.Play(3);
	}

	public void SetCollider(bool b){
		col.enabled = b;
	}

	public void SetHasMoved(bool b){
		hasMoved = b;
		spriteRenderer.color = b ? Color.gray : Color.white;
	}

	async void BlinkInRed(){
		await AsyncTweener.Tween(.25f, t => spriteRenderer.color = Color.Lerp(Color.white, Color.red, Mathf.PingPong(t*2, 1)));
	}

	public void TakeDamage(int damage){

		if(!alive) return;
		hp -= damage;
		hp = Mathf.Max(0, hp);
		hud.hp_fill = hp / (float)max_hp;
		BlinkInRed();
		if(hp <= 0){
			Die();
		}
	}

	void OnValidate(){
		ApplyChanges();
	}

	[ContextMenu("Apply Changes")]
	void ApplyChanges(){
		transform.position = Map<Tile>.CoordToWorldPoint(coord.x, coord.y, 32, 32);
		
		spriteRenderer.sprite = attributes.sprite;
		var cont = GetComponentInParent<TurnController>();
		team = cont.controllerName;
		teamID = cont.teamID;
		hud = GetComponentInChildren<WorldHUD>();
		hud.hudColor = cont.controllerColor;
	}
}
