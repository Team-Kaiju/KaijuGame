using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BuildingMaskScroll : MonoBehaviour
{
	public Image mask1;
	public Image mask2;
	public float scrollSpeed = 1.0F;
	RectTransform m1Rect;
	RectTransform m2Rect;
	
	public void Start()
	{
		m1Rect = mask1.GetComponent<RectTransform>();
		m2Rect = mask2.GetComponent<RectTransform>();
		mask2.transform.localPosition = new Vector3(m1Rect.rect.width, 0F, 0F);
		//m1X = mask1.transform.position.x;
		//m2X = mask2.transform.position.x;
	}
	
	public void Update()
	{
		mask1.transform.Translate(-Time.deltaTime * scrollSpeed, 0F, 0F);
		mask2.transform.Translate(-Time.deltaTime * scrollSpeed, 0F, 0F);
		
		if(mask1.transform.localPosition.x <= -m1Rect.rect.width)
		{
			mask1.transform.localPosition = mask1.transform.localPosition + new Vector3(m1Rect.rect.width*2F, 0F, 0F);
		}
		
		if(mask2.transform.localPosition.x <= -m2Rect.rect.width)
		{
			mask2.transform.localPosition = mask2.transform.localPosition + new Vector3(m2Rect.rect.width*2F, 0F, 0F);
		}
	}
}
