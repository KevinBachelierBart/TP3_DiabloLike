using System.Collections;
using UnityEngine;

public class FadableWalls : MonoBehaviour
{
    private Renderer m_renderer;
    [SerializeField] private float m_fadeDuration = 1f;
    [SerializeField] private float m_fadeInValue = 1f;
    [SerializeField] private float m_fadeOutValue = 0.5f;
    private bool m_isFading;

    private bool m_fadingIn;
    private bool m_fadingOut;

    private Material m_material; 
    //---------------DEBUG----------------|
    [ContextMenu("Debug / Fade In")]//----|
    private void DebugFadeIn()//----------|
    {//-----------------------------------|
        FadeIn();//-----------------------|
    }//-----------------------------------|
    [ContextMenu("Debug / Fade Out")]//---|
    private void DebugFadeOut()//---------|
    {//-----------------------------------|
        FadeOut();//----------------------|
    }//-----------------------------------|
    //---------------DEBUG----------------|


    private void Start()
    {
        m_renderer = GetComponent<Renderer>();
    }
    public void FadeIn()//+1 Fade
    {
        if (m_isFading == false) 
        {
            StartCoroutine(Fading(m_fadeInValue));
        }
    }
        
    public void FadeOut()//+0.5 Fade out
    {
        if (m_isFading == false)
        {
            StartCoroutine(Fading(m_fadeOutValue));
        }
    }
    private IEnumerator Fading(float fadeDirection)//Le gros par LLM puis modularise par moi
    {
        m_isFading = true;

        Material mat = m_renderer.material;

        Color color = mat.color;
        float startAlpha = color.a;
        float elapsed = 0f;

        while (elapsed < m_fadeDuration)
        {
            elapsed += Time.deltaTime;

            float alpha = Mathf.Lerp(startAlpha, fadeDirection, elapsed / m_fadeDuration);

            color.a = alpha;
            mat.color = color;

            yield return null;
        }

        color.a = fadeDirection;
        mat.color = color;

        m_isFading = false;
    }
}
