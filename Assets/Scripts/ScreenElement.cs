using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ScreenElement : MonoBehaviour
{
    public Sprite sprite;
    public bool isFloor = false;
    public int width = 1;
    public int height = 1;

    void Awake()
    {
    }
    void Start()
    {

    }

    void Update()
    {
        var s = new Vector2(width, height);
        SpriteRenderer spriteComp = this.GetComponent<SpriteRenderer>();
        spriteComp.sprite = sprite;
        spriteComp.size = s;

        BoxCollider2D colliderComp = this.GetComponent<BoxCollider2D>();
        colliderComp.size = s;

        this.tag = isFloor ? "Ground" : "Untagged";
        gameObject.layer = isFloor ? 8 : 0;
    }
}
