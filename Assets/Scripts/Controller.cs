using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Controller : MonoBehaviour, IDragHandler,IPointerDownHandler,IPointerUpHandler {

	private Image backgroundImage,controllerImage;

	public Vector3 InputDirection{ set; get; }

	private void Start(){
		backgroundImage = GetComponent<Image> ();
		controllerImage = transform.GetChild(0).GetComponent<Image> ();
		InputDirection = Vector3.zero;
	}

	public virtual void OnDrag(PointerEventData ped){
		Vector2 pos = Vector2.zero;
		if (RectTransformUtility.ScreenPointToLocalPointInRectangle
			(backgroundImage.rectTransform,
			   ped.position,
			   ped.pressEventCamera,
			   out pos)) 
		{
			pos.x = (pos.x / backgroundImage.rectTransform.sizeDelta.x);
			pos.y = (pos.y / backgroundImage.rectTransform.sizeDelta.y);

			float x = (backgroundImage.rectTransform.pivot.x == 1) ? pos.x * 2 + 1 : pos.x * 2 - 1;
			float y = (backgroundImage.rectTransform.pivot.y == 1) ? pos.y * 2 + 1 : pos.y * 2 - 1;
			InputDirection = new Vector3 (x, 0, y);
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
