using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour {


	float TimeUntilNextTrack,trackPlantingDelay=0.4f;
	Vector3 walkingDirection,shootDir;
	float animationTime=0.0f;
	public Sprite[] animationSprites;
	public Sprite[] idle;
	public GameObject trackPrefab;
	public Controller moveController;
	public BoxCollider2D collider1,collider2;
	Vector3 lastDirection;
    private float finishAnimationYMovement = 0.15f;
	// Use this for initialization

	
	// Update is called once per frame
	void Update () {
        if (moveController.InputDirection != Vector3.zero && !(Rooms.complete || Rooms.lose))
        {
            TimeUntilNextTrack += Time.deltaTime;
            if (TimeUntilNextTrack > trackPlantingDelay)
            {
                Instantiate(trackPrefab, this.transform.position, Quaternion.identity);
                TimeUntilNextTrack = 0f;
            }
            walkingDirection = moveController.InputDirection;
            Vector3 rotatedDirection = new Vector3(walkingDirection.x, walkingDirection.z, 0);
            lastDirection = rotatedDirection;
            this.transform.position += rotatedDirection / 12;
            animationTime += Time.deltaTime * Mathf.Clamp(Mathf.Max(Mathf.Abs(walkingDirection.x), Mathf.Abs(walkingDirection.z)), 0.25f, float.MaxValue);
            GetComponent<SpriteRenderer>().sprite = animationSprites[(int)Mathf.Floor(animationTime * 8) % 4 + (getOrientation(rotatedDirection) * 4)];

        }
        else
        {
            animationTime = 0.0f;
            GetComponent<SpriteRenderer>().sprite = idle[getOrientation(lastDirection)];
            if (Rooms.complete)
            {
                this.transform.GetChild(0).Translate(0, finishAnimationYMovement*-1, 0);
                this.transform.Translate(0, finishAnimationYMovement, 0);
                finishAnimationYMovement -= Time.deltaTime / 2f;
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
