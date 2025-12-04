using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardObject : MonoBehaviour, IPointerClickHandler
{
    public Action<CardObject> OnCardClicked;

    [SerializeField] private AudioClip cardFlipAudio;
    [SerializeField] private float animSpeedFactor;
    [SerializeField] private float fadeDuration;

    private AudioSource audioSource;
    private Image imgComponent;

    public Sprite TargetPic => targetPic;
    private Sprite targetPic;

    private Sprite closePic;

    private float closeDelay;
    private bool canClick = true;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        imgComponent = GetComponent<Image>();

        closePic = imgComponent.sprite;
        canClick = false;

        if (LoadingHandler.Instance != null)
            LoadingHandler.Instance.OnSceneLoadCompleted += OnSceneLoaded;
    }
    private void OnDisable()
    {
        OnCardClicked = null;

        if (LoadingHandler.Instance != null)
            LoadingHandler.Instance.OnSceneLoadCompleted -= OnSceneLoaded;
    }

    private void OnSceneLoaded()
    {
        Invoke(nameof(CloseCard), closeDelay);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!canClick)
            return;

        canClick = false;
        DoFlipAnimation(OnCompleted: () => OnCardClicked?.Invoke(this));
        audioSource.PlayOneShot(cardFlipAudio);

        //OnCardClicked?.Invoke(this);
    }

    public void Initialize(Sprite targetPic, float closeDelay)
    {
        this.targetPic = targetPic;

        this.closeDelay = closeDelay;
        imgComponent.sprite = targetPic;
    }

    public void CloseCard()
    {
        DoFlipAnimation(OnCompleted: () => canClick = true, openCard: false);
    }

    public void OnCardMatch()
    {
        imgComponent.CrossFadeColor(new Color(0, 0, 0, 0), fadeDuration, true, true);
        canClick = false;
    }

    private void DoFlipAnimation(Action OnCompleted = null, bool openCard = true)
    {
        Sprite newPic = openCard ? targetPic : closePic;

        StartCoroutine(FlipAnim());
        IEnumerator FlipAnim()
        {
            while (transform.localScale.x > 0.01f)
            {
                float xScale = transform.localScale.x - Time.deltaTime * animSpeedFactor;
                transform.localScale = new Vector3(xScale, 1, 1);

                yield return new WaitForEndOfFrame();
            }

            imgComponent.sprite = newPic;

            while (transform.localScale.x < 1)
            {
                float xScale = transform.localScale.x + Time.deltaTime * animSpeedFactor;
                transform.localScale = new Vector3(xScale, 1, 1);

                yield return new WaitForEndOfFrame();
            }

            yield return new WaitForEndOfFrame();
            OnCompleted?.Invoke();
            OnCompleted = null;
        }

    }
}
