using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class Unit : MonoBehaviour {
	public string team;
	public int teamID;
	public Vector2Int coord;
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
	public Vector2 pos{
		get => transform.position;
		set => transform.position = value;
	}

	public int facingIdx = 0;
	public Vector2Int[] facingDirs = {
		Vector2Int.down,
		Vector2Int.right,
		Vector2Int.left,
		Vector2Int.up
	};
	public Vector2Int facingDir => facingDirs[facingIdx];

	void Awake(){
		hud = GetComponentInChildren<WorldHUD>();
		col = GetComponent<Collider>();
		hp = max_hp / 2;
		hud.hp_fill = hp / (float)max_hp;
		spriteRenderer.color = Color.white;
	}

	public void SetLook((int, int) target){
		var a = Vector2Int.right;
		var b = Vector2Int.up;
		//Vector2Int.
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

	public async Task<bool> Move(Vector2Int p){
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
		
		var path = map.AStar(coord, p, t => t.const_compound);
		map[coord].unit = null;
		coord = p;
		map[coord].unit = this;
		SetHasMoved(false);
		foreach(var c in path){
			var opos = pos;
			var npos = map.CoordToWorldPoint(c);
			var dir = (npos - opos).normalized;
			spriteRenderer.sprite = attributes.SpriteFromDir(dir);
			await AsyncTweener.Tween(.1f, t => {
				pos = Vector2.Lerp(pos, npos, t);
			});
		}
		
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

	public void Heal(int ammount){
		hp += ammount;
		hp = Mathf.Max(0, hp);
		hud.hp_fill = hp / (float)max_hp;
		VFXManager.instance.ShowHealFX(coord);
	}

	void OnValidate(){
		ApplyChanges();
	}

	[ContextMenu("Apply Changes")]
	public void ApplyChanges(){
		transform.position = Map<Tile>.CoordToWorldPoint(coord.x, coord.y, 32, 32);
		
		spriteRenderer.sprite = attributes.sprite;
		var cont = GetComponentInParent<TurnController>();
		team = cont.controllerName;
		teamID = cont.teamID;
		hud = GetComponentInChildren<WorldHUD>();
		hud.hudColor = cont.controllerColor;
	}
}
