using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSource : MonoBehaviour
{
    public float maxResourceAmount = 1000.0f;
    public float hungerSatisfactionMultiplier = 2f;
    [SerializeField]
    private float resourceAmount = 1000.0f;
    public bool isRenewable = false;
    public float regenerationRate = 0.1f;
    public float regenerationDelay = 3f;

    private Animator animator;
    private Coroutine regenerationCoroutine;


    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void LateUpdate() {
        animator.SetFloat(AnimationStrings.resourcePercentage, resourceAmount / maxResourceAmount);
    }

    public bool IsEmpty()
    {
        return !(resourceAmount > 0);
    }

    public float Consume(float amount)
    {
        float consumed = Mathf.Min(amount, resourceAmount);
        resourceAmount -= consumed;

        if (isRenewable)
        {
            if (regenerationCoroutine != null)
            {
                StopCoroutine(regenerationCoroutine);
            }

            regenerationCoroutine = StartCoroutine(Regenerate());
        }

        return consumed * hungerSatisfactionMultiplier;
    }

    public IEnumerator Regenerate()
    {
        yield return new WaitForSeconds(regenerationDelay);

        while (resourceAmount < maxResourceAmount)
        {
            resourceAmount += Time.deltaTime * regenerationRate;
            resourceAmount = Mathf.Min(resourceAmount, maxResourceAmount);
            yield return null;
        }

        regenerationCoroutine = null;

    }

}
