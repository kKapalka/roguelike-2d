using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MovementController : MonoBehaviour, IDragHandler,IPointerDownHandler,IPointerUpHandler {

	private Image backgroundImage,controllerImage;

	public Vector3 InputDirection{ set; get; }

	private void Start(){
		backgroundImage = GetComponent<Image> ();
		controllerImage = transform.GetChild(0).GetComponent<Image> ();
		InputDirection = Vector3.zero;
	}

	public virtual void OnDrag(PointerEventData ped){
		Vector2 pos = new Vector2(backgroundImage.rectTransform.position.x,backgroundImage.rectTransform.position.y);
		if (RectTransformUtility.ScreenPointToLocalPointInRectangle
			(backgroundImage.rectTransform,
			   ped.position,
			   ped.pressEventCamera,
			   out pos)) 
		{
			pos.x = (pos.x / backgroundImage.rectTransform.sizeDelta.x);
			pos.y = (pos.y / backgroundImage.rectTransform.sizeDelta.y);

			InputDirection = new Vector3 (pos.x*5, 0, pos.y*5);
			InputDirection = (InputDirection.magnitude > 1) ? InputDirection.normalized : InputDirection;

			controllerImage.rectTransform.anchoredPosition =
				new Vector3 (InputDirection.x * (backgroundImage.rectTransform.sizeDelta.x / 3),
				InputDirection.z * (backgroundImage.rectTransform.sizeDelta.y / 3));
		}
	}
	public virtual void OnPointerUp(PointerEventData ped){
		InputDirection = Vector3.zero;
		controllerImage.rectTransform.anchoredPosition = Vector3.zero;
	}
	public virtual void OnPointerDown(PointerEventData ped){
		OnDrag (ped);
	}

}
