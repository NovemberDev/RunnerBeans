using UnityEngine;

public class TextureSetter : MonoBehaviour
{
    public Texture[] Textures;

	void Start ()
    {
		GetComponentInChildren<Renderer>().material.SetTexture("_MainTex", Textures[Random.Range(0, Textures.Length)]);
    }
}