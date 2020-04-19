﻿using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Controller2D))]
public class Player : MonoBehaviour {

	public Transform t;
	public PlayerAnimationController playerAnimationController;

	public float maxJumpHeight = 4;
	public float minJumpHeight = 1;

	public float timeToJumpApex = .4f;
	public float moveSpeed = 9;
	public float MoveSpeed {
		get {
			return handController.HasThingToProtect() ? moveSpeed / 2f : moveSpeed;
		}
	}

	readonly float accelerationTimeAirborne = .2f;
	readonly float accelerationTimeGrounded = .1f;

	public Vector2 wallJumpClimb;
	public Vector2 wallJumpOff;
	public Vector2 wallLeap;

	public float wallSlideSpeedMax = 3;
	public float wallStickTime = .25f;

	public void GetHit() {
		CameraShaker.instance.HitCameraShake();
		handController.Throw();
		this.EnsureCoroutineStopped(ref beStunnedRoutine);
		beStunnedRoutine = StartCoroutine(BeStunned());
	}

	private static readonly WaitForSeconds stunLength = new WaitForSeconds(0.5f);
	public bool isStunned = false;
	private Coroutine beStunnedRoutine;
	private IEnumerator BeStunned() {
		isStunned = true;
		yield return stunLength;
		isStunned = false;
		beStunnedRoutine = null;
	}

	float timeToWallUnstick;

	float gravity;
	float maxJumpVelocity;
	public float MaxJumpVelocity {
		get {
			return handController.HasThingToProtect() ? maxJumpVelocity / 2f : maxJumpVelocity;
		}
	}

	float minJumpVelocity;
	public float MinJumpVelocity {
		get {
			return handController.HasThingToProtect() ? minJumpVelocity / 2f : minJumpVelocity;
		}
	}
	Vector3 velocity;
	float velocityXSmoothing;

	Controller2D controller;
	public HandController handController;

	Vector2 directionalInput;
	bool wallSliding;
	int wallDirX;

	public static Player mainPlayer;

	void Awake()
	{
		mainPlayer = this;
	}
	void Start() {
		controller = GetComponent<Controller2D> ();

		gravity = -(2 * maxJumpHeight) / Mathf.Pow (timeToJumpApex, 2);
		maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
		minJumpVelocity = Mathf.Sqrt (2 * Mathf.Abs (gravity) * minJumpHeight);
	}

	void FixedUpdate() {
        CalculateVelocity();
        HandleWallSliding();

        controller.Move(velocity * Time.fixedDeltaTime, directionalInput);

        if (controller.collisions.above || controller.collisions.below) {
            if (controller.collisions.slidingDownMaxSlope) {
                velocity.y += controller.collisions.slopeNormal.y * -gravity * Time.deltaTime;
            } else {
                velocity.y = 0;
            }
        }

		if(t.position.y < -15 && Time.timeScale > 0) {
			GameOverManager.GameOver();
		}
	}

	public void SetDirectionalInput (Vector2 input) {
		directionalInput = input;
	}

	public void OnJumpInputDown() {
		if (Time.timeScale > 0) {
			if (wallSliding) {
				AudioManager.Instance.PlayJumpSound();
				playerAnimationController.Jump();
				if (wallDirX == directionalInput.x) {
					velocity.x = -wallDirX * wallJumpClimb.x;
					velocity.y = wallJumpClimb.y;
				} else if (directionalInput.x == 0) {
					velocity.x = -wallDirX * wallJumpOff.x;
					velocity.y = wallJumpOff.y;
				} else {
					velocity.x = -wallDirX * wallLeap.x;
					velocity.y = wallLeap.y;
				}
			}
			if (controller.collisions.below) {
				AudioManager.Instance.PlayJumpSound();
				playerAnimationController.Jump();
				if (controller.collisions.slidingDownMaxSlope) {
					if (directionalInput.x != -Mathf.Sign(controller.collisions.slopeNormal.x)) { // not jumping against max slope
						velocity.y = MaxJumpVelocity * controller.collisions.slopeNormal.y;
						velocity.x = MaxJumpVelocity * controller.collisions.slopeNormal.x;
					}
				} else {
					velocity.y = MaxJumpVelocity;
				}
			}
		}
	}

	public void OnJumpInputUp() {
		if (velocity.y > MinJumpVelocity) {
			velocity.y = MinJumpVelocity;
		}
	}

	void HandleWallSliding() {
		wallDirX = (controller.collisions.left) ? -1 : 1;
		wallSliding = false;
		if ((controller.collisions.left || controller.collisions.right) && !controller.collisions.below && velocity.y < 0) {
			wallSliding = true;

			if (velocity.y < -wallSlideSpeedMax) {
				velocity.y = -wallSlideSpeedMax;
			}

			if (timeToWallUnstick > 0) {
				velocityXSmoothing = 0;
				velocity.x = 0;

				if (directionalInput.x != wallDirX && directionalInput.x != 0) {
					timeToWallUnstick -= Time.deltaTime;
				}
				else {
					timeToWallUnstick = wallStickTime;
				}
			}
			else {
				timeToWallUnstick = wallStickTime;
			}

		}

	}

	void CalculateVelocity() {
		float targetVelocityX = directionalInput.x * MoveSpeed;
		playerAnimationController.RotateWheel(targetVelocityX);
		velocity.x = Mathf.SmoothDamp (velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below)?accelerationTimeGrounded:accelerationTimeAirborne);
		velocity.y += gravity * Time.deltaTime;
	}
}
