﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.LuisPedroFonseca.ProCamera2D;

public class Torpedo : MonoBehaviour {

	public float incSpeed = 1.0f, maxSpeed = 10.0f;
    public float damage;
	public float disappearTime = 10.0f;
    public GameObject hitfx;
    public float lifetime = 5;
	private Rigidbody2D rigidBody;
	private GameObject particle;

	void Start () {
		rigidBody = gameObject.GetComponent<Rigidbody2D> ();
		particle = gameObject.transform.GetChild (0).gameObject;

		particle.transform.localEulerAngles = new Vector3 ((transform.eulerAngles.z * -1 - 90), -90, -90);
        StartCoroutine(longlife());
	}
	void Update () {
		if (rigidBody.velocity.magnitude < maxSpeed)
			rigidBody.AddForce (transform.right * -incSpeed * 50);
	}

    IEnumerator longlife()
    {
        yield return new WaitForSeconds(lifetime);
        Instantiate(hitfx, transform.position, transform.rotation);
        ProCamera2DShake.Instance.Shake(0);
        CameraController.Instance.Unfollow();
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.GetComponent<SubmarineControl>().ChangeHealth(-damage);
            Instantiate(hitfx, transform.position, transform.rotation);
            ProCamera2DShake.Instance.Shake(0);
            CameraController.Instance.Unfollow();
            Destroy(this.gameObject);
        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            Instantiate(hitfx, transform.position, transform.rotation);
            ProCamera2DShake.Instance.Shake(0);
            collision.GetComponent<EnemyBehaviour>().Dead();
            CameraController.Instance.Unfollow();
            Destroy(this.gameObject);
            NotificationTrigger.Instance.TriggerNotification(0);
        }
        else if (collision.gameObject.CompareTag("Egon"))
        {
            Instantiate(hitfx, transform.position, transform.rotation);
            ProCamera2DShake.Instance.Shake(0);
            CameraController.Instance.Unfollow();
            NotificationTrigger.Instance.TriggerNotification(2);
            Destroy(this.gameObject);
            
        }
    }
}
