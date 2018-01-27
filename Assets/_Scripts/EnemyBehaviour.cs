﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyBehaviour : MonoBehaviour {
	public float movementDelay;
	public float shootDelay;
	public int runProbability;
	private Rigidbody2D enemyBody;
	public float moveSpeed;
	private GameObject player;
	public GameObject torpedo;
	public Transform torpedoSpawnPoint;
	public string playerTag;
    public float shootThreshold = 1f;
    public float lockOnDuration = 4f;

	void Start () {
		player = GameObject.FindGameObjectWithTag (playerTag);
		enemyBody = gameObject.GetComponent<Rigidbody2D> ();
		StartCoroutine (Movement());
		StartCoroutine (Shoot ());
	}

	public void Detected() {
		transform.DOKill ();
		//DOTween.Clear ();
		Debug.Log ("Is this deteccted?");
		int probability = Random.Range (0, 10);
		if (probability <= runProbability) {
			Debug.Log ("Probability chance!");
			float newY = Random.Range (-3f, 3f);
			transform.DOLocalMoveY (newY, moveSpeed);
		}
	}

	IEnumerator Movement() {
		transform.DOKill ();
		//DOTween.Clear ();
		float oldY = gameObject.transform.position.y;
		float newY = player.transform.position.y;
		float deltaY = Mathf.Abs (oldY - newY);
		transform.DOMoveY (newY, deltaY * moveSpeed);
		yield return new WaitForSeconds (movementDelay);
		StartCoroutine (Movement ());
	}

	IEnumerator Shoot() {
		if (gameObject.transform.position.y > player.transform.position.y - 1 && gameObject.transform.position.y < player.transform.position.y + 1) {
            StartCoroutine(LockOn());
		}
		yield return new WaitForSeconds (shootDelay);
		StartCoroutine (Shoot ());
	}

    IEnumerator LockOn()
    {
        AlertManager.Instance.ShowAlert();
        yield return new WaitForSeconds(lockOnDuration);
        FireTorpedo();
    }

	public void FireTorpedo() {
		if (gameObject.transform.position.x > player.transform.position.x) {
			Instantiate (torpedo, torpedoSpawnPoint.position, Quaternion.Euler (new Vector3 (0, 0, 0)));
		} else {
			Instantiate (torpedo, torpedoSpawnPoint.position, Quaternion.Euler (new Vector3 (0, 180,0)));
		}
	}
}