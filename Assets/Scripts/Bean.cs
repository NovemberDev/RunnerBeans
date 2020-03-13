using UnityEngine;

public class Bean : MonoBehaviour
{
    public int Value = 30;

    public Texture[] BeansTextures;

    private AudioSource audioBeanCollect;

    private Renderer ren;

    private void Start()
    {
        ren = GetComponentInChildren<Renderer>();
        ren.material.SetTexture("_MainTex", BeansTextures[Random.Range(0, BeansTextures.Length)]);
        audioBeanCollect = GetComponent<AudioSource>();
    }

    void Update()
    {
        transform.Rotate(Vector3.up, 150.0f * Time.deltaTime);
        transform.position = new Vector3(transform.position.x, transform.position.y + 0.05f * Mathf.Sin(10.0f * Time.time), transform.position.z);
    }

    void OnTriggerEnter(Collider other)
    {
        Player.Instance.Score += Value;
        audioBeanCollect.Play();
        ren.enabled = false;
        Destroy(gameObject, 0.25f);
    }
}
