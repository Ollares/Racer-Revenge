using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System;

public class UI_WinLose_Anim : MonoBehaviour
{

	[SerializeField] private CanvasGroup canvasGroup;

	[SerializeField] _UIAnimatorComponent[] uiAnimators;

	private Tween fadeTween;
	public Transform[] rotateObject;
	public float rotateObjectSpeed;
	public RectTransform[] scaleObject;
	public RectTransform button;
	public float buttonDuration;
	public Ease buttonEase;



	public Sequence animationUISequence;
	// Start is called before the first frame update

	public bool showNextLevelButton = true;
	Sequence sequence;
	public float y_pos;
	public static UI_WinLose_Anim anim;
    private void Awake()
    {
		y_pos = button.localPosition.y;
		animationUISequence = DOTween.Sequence();
		canvasGroup.alpha = 0;

		for (int i = 0; i < uiAnimators.Length; i++)
		{
			uiAnimators[i].Initialize();
		}
	}
    private void OnEnable() // on show object
    {
		sequence = DOTween.Sequence();
		sequence.Insert(0, FadeAnim());   //add at the given time position
		sequence.Insert(0, ButtonMove());

		sequence.Play();
		AnimationUI();
	}

    public void OnDisable()
    {
		button.localPosition = new Vector3(0, y_pos,0);

		sequence.Kill();
		canvasGroup.DOFade(0, 0); //instantly lose fade

		StartPos();
	}
    Tween FadeAnim()
	{
		return canvasGroup.DOFade(1, 1);
	}
	Tween ButtonMove() 
	{
        if (!showNextLevelButton) 
		{
			return button.DOAnchorPosY(y_pos, buttonDuration).SetEase(buttonEase);
		}
		return button.DOAnchorPosY(-350f, buttonDuration).SetEase(buttonEase);
	}
    private void Start()
    {
		//AnimationUI();
	}
    private void Update()
	{
		//ray.RotateAround(transform.position, Vector3.forward, rayRotate * Time.deltaTime);
		//rotateObject[0].Rotate(Vector3.forward, rotateObjectSpeed * Time.deltaTime);
		for (int i = 0; i < rotateObject.Length; i++)
		{
			rotateObject[i].Rotate(Vector3.forward, rotateObjectSpeed * Time.deltaTime);
		}

	}
	void StartPos() 
	{
		animationUISequence?.Kill();
		//scaleObject.transform.localScale = Vector3.zero;
		for (int i = 0; i < rotateObject.Length; i++)
		{
			scaleObject[i].transform.localScale = Vector3.zero;
		}

	}
	void AnimationUI()
	{
		StartPos();
		
		//animationUISequence?.Kill();
		animationUISequence = DOTween.Sequence();
		//animationUISequence.Insert(0.1f ,scaleObject.transform.DOScale(1f, 0.3f));
		for (int i = 0; i < scaleObject.Length; i++)
		{
			animationUISequence.Insert(0.1f, scaleObject[i].transform.DOScale(1f, 0.3f));
		}
		animationUISequence.SetAutoKill();
	}

	public Coroutine Invoke(Action action, float time)
	{
		return StartCoroutine(InvokeAfterTime(action, time));
	}

	private IEnumerator InvokeAfterTime(Action action, float time)
	{

		yield return new WaitForSeconds(time);
		action?.Invoke();

	}


}

[System.Serializable]
public class _UIAnimatorComponent
{
	public RectTransform rectTransform;

	public bool PositionComponentEnabled;
	public UIPositionComponent PositionComponent;
	[Space]
	public bool ScaleComponentEnabled;
	public UIScaleComponent ScaleComponent;
	[Space]
	public bool RotationComponentEnabled;
	public UIRotationComponent RotationComponent;

	protected Vector3 originPosition;
	protected Vector3 originEulers;
	protected Vector3 originScale;

	public void Initialize()
	{
		originPosition = rectTransform.localPosition;
		originEulers = rectTransform.localEulerAngles;
		originScale = rectTransform.localScale;
	}

	public void Update()
	{
		if (PositionComponentEnabled)
		{
			PositionComponent.Update(rectTransform, originPosition);
		}
		if (ScaleComponentEnabled)
		{
			ScaleComponent.Update(rectTransform);
		}
		if (RotationComponentEnabled)
		{
			RotationComponent.Update(rectTransform, originEulers);
		}
	}
	[System.Serializable]
	public class UIPositionComponent
	{
		public void Update(RectTransform parent, Vector3 origin)
		{
		}
	}
	[System.Serializable]
	public class UIScaleComponent
	{
		public void Update(RectTransform parent)
		{
		}
	}
	public class UIRotationComponent
	{
		public void Update(RectTransform parent, Vector3 origin)
		{
			
		}
	}
}

