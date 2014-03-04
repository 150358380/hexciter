﻿using UnityEngine;
using System.Collections;

public class PlayerActions : MonoBehaviour {

	public static PlayerActions Instance;
	public bool swiping;
	public GridPlace selected;

	const float OFFSET = 0.8f;

	void Awake(){
		Instance = this;
	}
	
	void Start () {
		swiping = false;
		selected = null;
	}

	public void Deselect(){
		swiping = false;
		selected = null;
	}

	public void Destroy(){
		if (selected != null) {
			GridLogic.Instance.Destroy(selected);
		}
		Deselect();
	}

	public void Flood(){
		if (selected != null){
			GridLogic.Instance.Flood(selected);
		}
		Deselect();
	}

	public void DownAction(GridPlace gp){
		if (gp == null) return;

		if (!swiping && !gp.busy && gp.alive) {
			swiping = true;
			selected = gp;
		}
	}

	public void UpAction(){
		if (swiping) {
			// not the same place, do swiping action
			Vector2 diff = (Vector2)selected.transform.position - (Vector2)Camera.main.ScreenToWorldPoint(InputHandler.Instance.inputVector);
			if (Mathf.Abs(diff.x) >= OFFSET) {
				//Action was a drag - perform action based on up positio
				if (diff.x > 0)
					Flood();
				else
					Destroy();
			}
		}
		Deselect();
	}

	public void Update(){
		if (swiping && InputHandler.Instance.inputSignalUp){
			UpAction();
		}
	}
}
