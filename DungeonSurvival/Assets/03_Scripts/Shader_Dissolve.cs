using System.Collections;
using UnityEngine;

[System.Serializable]
public class Shader_Dissolve
{
    public const string CUT_OFF_HEIGHT = "_CutOffHeight"; // Lo que hace que desaparezca el objeto
    public const string EDGE_WIDTH = "_EdgeWidth"; // El contorno de lo desaparecido
    public const string EDGE_COLOR = "_EdgeColor"; // El color del contorno de lo desaparecido (emisivo)

    public void ReverseDissolveGameObject ( float speed, float increaseLimit, Renderer renderer, GameObject rendererParent, MonoBehaviour monoBehaviourClass )
    {
        monoBehaviourClass.StartCoroutine(IncreaseFloatValue(speed, increaseLimit, renderer, rendererParent));
    }

    private IEnumerator IncreaseFloatValue ( float speed, float increaseLimit, Renderer renderer, GameObject rendererParent ) //Spawnear objeto
    {
        MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
        renderer.GetPropertyBlock(materialPropertyBlock);

        float currentValue = materialPropertyBlock.GetFloat(CUT_OFF_HEIGHT);
        while (currentValue < increaseLimit)
        {
            currentValue += speed * Time.deltaTime; // Aumenta currentValue por speed cada segundo
            materialPropertyBlock.SetFloat(CUT_OFF_HEIGHT, currentValue); // Establece el valor actualizado en el shader
            renderer.SetPropertyBlock(materialPropertyBlock);
            yield return null; // Espera hasta el próximo frame
        }
    }
    public void DissolveGameObject ( float speed, float decreaseValueLimit, Renderer renderer, GameObject rendererParent, MonoBehaviour monoBehaviourClass )
    {
        monoBehaviourClass.StartCoroutine(DecreaseFloatValue(speed, decreaseValueLimit, renderer, rendererParent));
    }

    private IEnumerator DecreaseFloatValue ( float speed, float decreaseValueLimit, Renderer renderer, GameObject rendererParent )
    {
        MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
        renderer.GetPropertyBlock (materialPropertyBlock);

        float currentValue = renderer.material.GetFloat(CUT_OFF_HEIGHT);
        Debug.Log(currentValue);
        while (currentValue + 1 > decreaseValueLimit)
        {
            currentValue -= speed * Time.deltaTime; // Disminuye currentValue por speed cada segundo
            materialPropertyBlock.SetFloat(CUT_OFF_HEIGHT, currentValue); // Establece el valor actualizado en el shader
            renderer.SetPropertyBlock(materialPropertyBlock);
            yield return null; // Espera hasta el próximo frame
        }
        Debug.Log("destroyed");
        rendererParent.gameObject.SetActive(false);
    }
}