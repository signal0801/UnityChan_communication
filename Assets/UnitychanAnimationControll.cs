using UnityEngine;
using System.Collections;

public class UnitychanAnimationControll : MonoBehaviour
{
	AnimatorStateInfo animInfo;
	Animator animator;
	AudioSource audiosource;
	bool isLooked = false;

	public bool IsLooked { get { return isLooked; } set { isLooked = value; } }

	float speed = 50.0f;
	float normalTimer = 0.0f;
	float normalInterval = 15.0f;
	float signTimer = 0.0f;
	float signInterval = 6.5f;
	public AudioClip voice_Yes;
	public AudioClip voice_Syukkin;
	public AudioClip voice_ok;
	/// <summary>
	/// ユーザーのカメラのゲームオブジェクト
	/// </summary>
	[SerializeField]
	private GameObject userGameObject;

	enum State
	{
		NORMAL,
		WAVE,
		RUN,
		SIGN
	}

	State state;
	bool isEndRotate = true;

	public void OnMouseDown ()
	{
		isLooked = true;
	}
	// Use this for initialization
	void Start ()
	{
		animator = GetComponent<Animator> ();
		audiosource = GetComponent<AudioSource> ();
	}
	// Update is called once per frame
	void Update ()
	{
		animInfo = animator.GetCurrentAnimatorStateInfo (0);
		switch (state) {
		case State.NORMAL:
			Normal ();
			break;
		case State.WAVE:
			//Wave();
			break;
		case State.RUN:
			Run ();
			break;
		case State.SIGN:
			Sign ();
			break;
		}
	}

	void Normal ()
	{
		if (isLooked) {
			animator.SetTrigger ("sign");
			audiosource.clip = voice_Yes;
			audiosource.Play ();
			state = State.SIGN;
		} else if (normalTimer >= normalInterval && !isLooked) {
			int rand = Random.Range (0, 2);
			switch (rand) {
			case 0:
				animator.SetTrigger ("wave");
				audiosource.clip = voice_ok;
				audiosource.Play ();
				state = State.WAVE;
				break;
			case 1:
				animator.SetBool ("run", true);
				audiosource.clip = voice_Syukkin;
				audiosource.Play ();
				state = State.RUN;
				break;
			}
			normalTimer = 0.0f;
		}
		normalTimer += Time.deltaTime;
	}

	public void EndWave ()
	{/*	
        if (animInfo.IsName("Base Layer.WaveHands") && animInfo.normalizedTime < 1.0f)
        {
            Debug.Log("end wave");
            state = State.NORMAL;
        }*/
		state = State.NORMAL;
	}

	/// <summary>
	/// Playerの目の前に移動する
	/// </summary>
	void Run ()
	{
		// ユーザーの目の前の座標
		Vector3 offset = userGameObject.transform.forward * 5.0f;

		// unityちゃんの到達地点
		// ユーザーの現在位置にoffsetを足す
		Vector3 destination = userGameObject.transform.position + offset;
		destination.y = 0.0f;

		// MoveTowardsで移動させる
		transform.position = 
			Vector3.MoveTowards (
			transform.position,
			destination,
			Time.deltaTime * 5.0f
		);

		float diff = (destination - transform.position).magnitude;
		// 目的地の誤差が0.1f以内だったら
		if (diff <= 0.1f) {
			state = State.NORMAL;
			transform.LookAt (userGameObject.transform);
			animator.SetBool ("run", false);
		}
	}

	void OnTriggerExit (Collider other)
	{
		if (isEndRotate && state == State.RUN) {
			isEndRotate = false;
		}
	}

	void OnTriggerEnter (Collider other)
	{
		if (!isEndRotate && state == State.RUN) {
			isEndRotate = true;
			state = State.NORMAL;
			animator.SetBool ("run", false);
			Debug.Log ("Exit");
		}
	}

	void Sign ()
	{
		signTimer += Time.deltaTime;
		if (signTimer >= signInterval) {
			state = State.NORMAL;
			signTimer = 0.0f;
		}
	}
}