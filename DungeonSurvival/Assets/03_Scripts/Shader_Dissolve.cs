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

    private IEnumerator IncreaseFloatValue ( float speed, float increaseLimit, Renderer renderer, GameObject rendererParent )
    {
        MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();

        while (true)
        {
            bool allMaterialsMaxed = true;
            for (int i = 0; i < renderer.materials.Length; i++)
            {
                renderer.GetPropertyBlock(materialPropertyBlock, i);
                float currentValue = materialPropertyBlock.GetFloat(CUT_OFF_HEIGHT);
                currentValue += speed * Time.deltaTime;
                currentValue = Mathf.Min(currentValue, increaseLimit);
                materialPropertyBlock.SetFloat(CUT_OFF_HEIGHT, currentValue);
                renderer.SetPropertyBlock(materialPropertyBlock, i);

                if (currentValue < increaseLimit)
                    allMaterialsMaxed = false;
            }

            if (allMaterialsMaxed)
                break;

            yield return null;
        }
    }
    public void DissolveGameObject ( float speed, float decreaseValueLimit, Renderer renderer, GameObject rendererParent, MonoBehaviour monoBehaviourClass )
    {
        monoBehaviourClass.StartCoroutine(DecreaseFloatValue(speed, decreaseValueLimit, renderer, rendererParent));
    }

    private IEnumerator DecreaseFloatValue ( float speed, float decreaseValueLimit, Renderer renderer, GameObject rendererParent )
    {
        MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();

        while (true)
        {
            bool allMaterialsMinimized = true;
            for (int i = 0; i < renderer.materials.Length; i++)
            {
                renderer.GetPropertyBlock(materialPropertyBlock, i);
                float currentValue = materialPropertyBlock.GetFloat(CUT_OFF_HEIGHT);
                currentValue -= speed * Time.deltaTime;
                currentValue = Mathf.Max(currentValue, decreaseValueLimit);
                materialPropertyBlock.SetFloat(CUT_OFF_HEIGHT, currentValue);
                renderer.SetPropertyBlock(materialPropertyBlock, i);

                if (currentValue > decreaseValueLimit)
                    allMaterialsMinimized = false;
            }

            if (allMaterialsMinimized)
            {
                rendererParent.gameObject.SetActive(false);
                break;
            }

            yield return null;
        }
    }
}