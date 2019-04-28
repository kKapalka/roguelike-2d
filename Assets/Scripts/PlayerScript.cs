using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour {


	//float TimeUntilNextTrack,trackPlantingDelay=0.4f;
	Vector3 walkingDirection;
	float animationTime=0.0f;
	public Sprite[] animationSprites;
	public Sprite[] idle;
    private Animator animator;
	//public GameObject trackPrefab;
	public MovementController moveController;
	public BoxCollider2D collider1,collider2;
    public static Rigidbody2D body;
	Vector3 lastDirection;
    private float finishAnimationYMovement = 0.05f;
    public int orientation;
    // Use this for initialization
    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update () {
        if (moveController.InputDirection != Vector3.zero && !(Rooms.complete || Rooms.lose))
        {
            /*TimeUntilNextTrack += Time.deltaTime;
            if (TimeUntilNextTrack > trackPlantingDelay)
            {
                Instantiate(trackPrefab, this.transform.position, Quaternion.identity);
                TimeUntilNextTrack = 0f;
            }*/
            walkingDirection = moveController.InputDirection;
            Vector3 rotatedDirection = new Vector3(walkingDirection.x, walkingDirection.z, 0);
            animator.SetFloat("MovementX", walkingDirection.x);
            animator.SetFloat("MovementY", walkingDirection.z);
            animator.SetBool("Moving", true);
            orientation = getOrientation(rotatedDirection);
            lastDirection = rotatedDirection;
            body.MovePosition(new Vector2(body.position.x + (walkingDirection.x * Time.deltaTime * 1.5f), body.position.y + (walkingDirection.z * Time.deltaTime * 1.5f)));
        }
        else
        {
            animator.SetFloat("LastMovementX", lastDirection.x);
            animator.SetFloat("LastMovementY", lastDirection.y);
            animator.SetBool("Moving", false);
            if (Rooms.complete)
            {
                this.transform.GetChild(0).Translate(0, finishAnimationYMovement*-1, 0);
                this.transform.Translate(0, finishAnimationYMovement, 0);
                finishAnimationYMovement -= Time.deltaTime / 8f;
            }
        }
	}
	void OnTriggerEnter2D(Collider2D otherCollider){
        if (!collider1.isTrigger)
        {
            if (collider1.IsTouching(otherCollider) && otherCollider.gameObject.tag == "Killer")
            {
                Rooms.lose = true;
                Rooms.DeathText = "you  have  been  smacked   by  the  killer  trap";
            }
            else if (collider1.IsTouching(otherCollider) && otherCollider.gameObject.tag == "Bullet")
            {
                Destroy(otherCollider.gameObject);
                Rooms.lose = true;
                Rooms.DeathText = "you  have  been  shot   by  the  shooter  trap";
            }
            else if (collider1.IsTouching(otherCollider) && otherCollider.gameObject.tag == "Finish")
            {
                StartCoroutine(waitForNextLevel());
            }
            //else if (otherCollider.gameObject.tag == "Wall")
            //{
            //    otherCollider.gameObject.layer = 11;
            //}
        }
	}

    IEnumerator waitForNextLevel()
    {
        collider1.isTrigger = true;
        Rooms.complete = true;
        yield return new WaitForSeconds(1.5f);
        PlayerPrefs.SetInt("Level", PlayerPrefs.GetInt("Level") + 1);
        int scene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(scene, LoadSceneMode.Single);
    }


    int getOrientation(Vector3 direction){
		int orientation = 0;
		if (direction.x > 0)
            orientation += 1;
		if (direction.y > 0)
            orientation += 2;
		return orientation;
	}
}
