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
        float screenWidth = Screen.width;

        m1Rect.Translate(-Time.deltaTime * scrollSpeed, 0F, 0F);
        m2Rect.Translate(-Time.deltaTime * scrollSpeed, 0F, 0F);

        if (m1Rect.anchoredPosition.x <= -screenWidth || m1Rect.anchoredPosition.x > m2Rect.anchoredPosition.x)
		{
            m1Rect.anchoredPosition = new Vector3(screenWidth + m2Rect.anchoredPosition.x, 0F, 0F);
		}

        if (m2Rect.anchoredPosition.x <= -screenWidth || m2Rect.anchoredPosition.x > m1Rect.anchoredPosition.x)
		{
            m2Rect.anchoredPosition = new Vector3(screenWidth + m1Rect.anchoredPosition.x, 0F, 0F);
		}
	}
}
