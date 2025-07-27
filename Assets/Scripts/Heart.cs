using UnityEngine;

public class Heart : MonoBehaviour
{
	[SerializeField] private Sprite fullHeart;
	[SerializeField] private Sprite emptyHeart;

	private SpriteRenderer heartSpriteRenderer;

	private void Awake()
	{
		heartSpriteRenderer = GetComponent<SpriteRenderer>();
	}
	
	private void Start()
	{
		SetFull();
	}

	public void SetFull()
	{
		heartSpriteRenderer.sprite = fullHeart;
	}

	public void SetEmpty()
	{
		heartSpriteRenderer.sprite = emptyHeart;
	}
}
