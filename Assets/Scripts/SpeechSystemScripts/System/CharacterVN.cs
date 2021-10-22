using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class CharacterVN : Character
{
    [HideInInspector] public RectTransform root;

    public bool enabled { get { return root.gameObject.activeInHierarchy; } set { root.gameObject.SetActive(value); } }
    public Vector2 anchorPadding { get { return root.anchorMax - root.anchorMin; } }

    DialogueSystem dialogue;
    public void Say(string speech, bool additive = false)
    {
        if (!enabled)
            enabled = true;

        dialogue.Say(speech, this.characterName,additive);
    }

    Vector2 targetPosition;
    Coroutine moving;
    bool isMoving { get { return moving != null; } }

    public void MoveTo(Vector2 target, float speed, bool smooth = true)
    {
        StopMoving();
        moving = CharacterManagerVN.instance.StartCoroutine(Moving(target, speed, smooth));
    }

    public void StopMoving(bool arriveAtTargetPosImmediately = false)
    {
        if (isMoving)
        {
            CharacterManagerVN.instance.StopCoroutine(moving);
            if (arriveAtTargetPosImmediately)
                SetPosition(targetPosition);
        }
        moving = null;
    }

    public void SetPosition(Vector2 target)
    {
        Vector2 padding = anchorPadding;
        float maxX = 1f - padding.x;
        float maxY = 1f - padding.y;
        Vector2 minAnchorTarget = new Vector2(maxX * targetPosition.x, maxY * targetPosition.y);

        root.anchorMin = minAnchorTarget;
        root.anchorMax = root.anchorMin + padding;
    }

    IEnumerator Moving(Vector2 target, float speed, bool smooth)
    {
        targetPosition = target;
        
        Vector2 padding = anchorPadding;

        float maxX = 1f - padding.x;
        float maxY = 1f - padding.y;

        Vector2 minAnchorTarget = new Vector2(maxX * targetPosition.x, maxY * targetPosition.y);
        speed *= Time.deltaTime;
        while (root.anchorMin != minAnchorTarget)
        {
            root.anchorMin = (!smooth) ? Vector2.MoveTowards(root.anchorMin, minAnchorTarget, speed) : Vector2.Lerp(root.anchorMin, minAnchorTarget, speed);
            root.anchorMax = root.anchorMin + padding;
            yield return new WaitForEndOfFrame();
        }
        StopMoving();
    }

    public Sprite GetSprite(int index = 0)
    {
        Sprite[] sprites = Resources.LoadAll<Sprite>("Textures/VNAssets/Characters/"+characterName);
        return sprites[index];
    }
    public Sprite GetSprite(string spriteName = "")
    {
        Sprite[] sprites = Resources.LoadAll<Sprite>("Textures/VNAssets/Characters/" + characterName);
        for (int a = 0; a < sprites.Length; a++)
        {
            if (sprites[a].name == spriteName)
            {
                return sprites[a];
            }
        }
        return sprites.Length > 0? sprites[0]:null;
    }

    public void SetBody(int index)
    {
        renderers.bodyRenderer.sprite = GetSprite(index);
    }
    public void SetBody(Sprite sprite)
    {
        renderers.bodyRenderer.sprite = sprite;
    }
    public void SetBody(string spriteName)
    {
        renderers.bodyRenderer.sprite = GetSprite(spriteName);
    }

    public void SetExpression(int index)
    {
        renderers.expressionRenderer.sprite = GetSprite(index);
    }
    public void SetExpression(Sprite sprite)
    {
        renderers.expressionRenderer.sprite = sprite;
    }
    public void SetExpression(string spriteName)
    {
        renderers.expressionRenderer.sprite = GetSprite(spriteName);
    }

    //Transition Body
    bool isTransitioningBody { get { return transitioningBody != null; } }
    Coroutine transitioningBody = null;

    public void TransitionBody(Sprite sprite, float speed, bool smooth)
    {

        if (renderers.bodyRenderer.sprite == sprite)
        {
            return;
        }
        StopTransitioningBody();
        transitioningExpression = CharacterManagerVN.instance.StartCoroutine(TransitioningBody(sprite, speed, smooth));
    }

    void StopTransitioningBody()
    {
        if (isTransitioningBody)
            CharacterManagerVN.instance.StopCoroutine(transitioningBody);
        transitioningBody = null;
    }

    public IEnumerator TransitioningBody(Sprite sprite, float speed, bool smooth)
    {
        for (int i = 0; i < renderers.allBodyRenderers.Count; i++)
        {
            Image image = renderers.allBodyRenderers[i];
            if (image.sprite == sprite)
            {
                renderers.bodyRenderer = image;
                break;
            }
        }
        
        if (renderers.bodyRenderer.sprite != sprite)
        {
            Image image = GameObject.Instantiate(renderers.bodyRenderer.gameObject, renderers.bodyRenderer.transform.parent).GetComponent<Image>();
            renderers.allBodyRenderers.Add(image);
            renderers.bodyRenderer = image;
            image.color = FunctionsVN.SetAlpha(image.color, 0f);
            image.sprite = sprite;
            
        }

        while (FunctionsVN.TransitionImages(ref renderers.bodyRenderer, ref renderers.allBodyRenderers, speed, smooth))
            yield return new WaitForEndOfFrame();

        Debug.Log("done");
        StopTransitioningBody();
    }

    //Transition Expression
    bool isTransitioningExpression { get { return transitioningExpression != null; } }
    Coroutine transitioningExpression = null;

    public void TransitionExpression(Sprite sprite, float speed, bool smooth)
    {
        if (renderers.expressionRenderer.sprite == sprite)
            return;

        StopTransitioningExpression();
        transitioningExpression = CharacterManagerVN.instance.StartCoroutine(TransitioningExpression(sprite, speed, smooth));
    }

    void StopTransitioningExpression()
    {
        if (isTransitioningExpression)
            CharacterManagerVN.instance.StopCoroutine(transitioningExpression);
        transitioningExpression = null;
    }

    public IEnumerator TransitioningExpression(Sprite sprite, float speed, bool smooth)
    {
        for (int i = 0; i < renderers.allExpressionRenderers.Count; i++)
        {
            Image image = renderers.allExpressionRenderers[i];
            if (image.sprite == sprite)
            {
                renderers.expressionRenderer = image;
                break;
            }
        }

        if (renderers.expressionRenderer.sprite != sprite)
        {
            Image image = GameObject.Instantiate(renderers.expressionRenderer.gameObject, renderers.expressionRenderer.transform.parent).GetComponent<Image>();
            renderers.allExpressionRenderers.Add(image);
            renderers.expressionRenderer = image;
            image.color = FunctionsVN.SetAlpha(image.color, 0f);
            image.sprite = sprite;
        }

        while (FunctionsVN.TransitionImages(ref renderers.expressionRenderer, ref renderers.allExpressionRenderers, speed, smooth))
            yield return new WaitForEndOfFrame();

        Debug.Log("done");
        StopTransitioningExpression();
    }

    public CharacterVN(string _name, bool enabledOnStart = true)
    {
        CharacterManagerVN cm = CharacterManagerVN.instance;
        GameObject prefab = Resources.Load("Prefabs/VNPrefabs/Character["+_name+"]") as GameObject;
        GameObject go = GameObject.Instantiate(prefab, cm.characterPanel);
        root = go.GetComponent<RectTransform>();
        CharacterName = _name;

        renderers.bodyRenderer = go.transform.Find("BodyLayer").GetComponentInChildren<Image>();
        renderers.expressionRenderer = go.transform.Find("ExpressionLayer").GetComponentInChildren<Image>();
        renderers.allBodyRenderers.Add(renderers.bodyRenderer);
        renderers.allExpressionRenderers.Add(renderers.expressionRenderer);

        dialogue = DialogueSystem.instance;
        enabled = enabledOnStart;
    }
    [System.Serializable]
    public class Renderers
    {
        //public RawImage renderder;
        public Image bodyRenderer;
        public Image expressionRenderer;
        public List<Image> allBodyRenderers =new List<Image>();
        public List<Image> allExpressionRenderers = new List<Image>();

    }
    public Renderers renderers = new Renderers();
}
    