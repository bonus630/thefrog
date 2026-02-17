using UnityEngine;
using System.Collections.Generic;


namespace br.com.bonus630.thefrog.Effects
{
    public class SpriteAfterImageEffect : IEffects
    {
        private SpriteRenderer original;
        private List<SpriteRenderer> clones;
        private float lifeTime;
        private float fadeSpeed;
        private float currentTime;
        private float delayTime;
        private float delayTimer;
        private int limit;
        private bool finished;
        private int nextIndex; // índice circular
        private ushort id;

        private SpriteAfterImageEffect(SpriteRenderer original,
                                      int limit = 6,
                                      float delayTime = 0.08f,
                                      float lifeTime = 0.4f,
                                      float fadeSpeed = 3f)
        {
            this.original = original;
            this.lifeTime = lifeTime;
            this.fadeSpeed = fadeSpeed;
            this.delayTime = delayTime;
            this.limit = Mathf.Max(1, limit);
            id = (ushort)Random.Range(1,ushort.MaxValue);
        }
        public static SpriteAfterImageEffect Create(SpriteRenderer original)
        {
            if (original == null)
                throw new System.ArgumentNullException(nameof(original));

            return new SpriteAfterImageEffect(original, 6, 0.08f, 0.4f, 3f);
        }

        public SpriteAfterImageEffect WithLimit(int value)
        {
            limit = Mathf.Max(1, value);
            return this;
        }

        public SpriteAfterImageEffect WithSpawnInterval(float value)
        {
            delayTime = value;
            return this;
        }

        public SpriteAfterImageEffect WithLifeTime(float value)
        {
            lifeTime = value;
            return this;
        }

        public SpriteAfterImageEffect WithFadeSpeed(float value)
        {
            fadeSpeed = value;
            return this;
        }
        public SpriteAfterImageEffect Build()
        {
            return this;
        }
        public override void Activate()
        {
            finished = false;
            delayTimer = delayTime + 1f;
            currentTime = 0f;
            nextIndex = 0;

            clones ??= new List<SpriteRenderer>(limit);
        }

        public override void UpdateEffects(float deltaTime)
        {
            if (finished)
                return;
            currentTime += deltaTime;
            delayTimer += deltaTime;

            // gera novo clone no intervalo
            if (delayTimer >= delayTime)
            {
                delayTimer = 0f;
                SpawnOrReuseClone();
            }

            // atualiza fade
            for (int i = 0; i < clones.Count; i++)
            {
                SpriteRenderer sr = clones[i];
                if (sr == null) continue;

                Color c = sr.color;
                c.a -= fadeSpeed * deltaTime;
                c.r -= fadeSpeed * deltaTime;
                c.g -= fadeSpeed * deltaTime / 2;
                sr.color = c;
            }

            // encerra quando todos estão invisíveis e passou o tempo de vida
            if (currentTime >= lifeTime && AllClonesInvisible())
                Deactivate();
        }

        private void SpawnOrReuseClone()
        {
            SpriteRenderer target;

            // se ainda não atingiu o limite, cria um novo clone
            if (clones.Count < limit)
            {
                GameObject cloneObj = new GameObject($"AfterImage_{id}_{clones.Count}");
                target = cloneObj.AddComponent<SpriteRenderer>();
                clones.Add(target);
            }
            else
            {
                // reutiliza o mais antigo no buffer circular
                target = clones[nextIndex];
            }

            // avança o índice circular
            nextIndex = (nextIndex + 1) % limit;

            // configura o clone na posição atual
            target.sprite = original.sprite;
            target.flipX = original.flipX;
            target.flipY = original.flipY;
            target.sortingLayerID = original.sortingLayerID;
            target.sortingOrder = original.sortingOrder - 1;
            target.transform.position = original.transform.position;
            target.transform.rotation = original.transform.rotation;
            target.transform.localScale = original.transform.lossyScale;
            target.color = new Color(1f, 1f, 1f, 1f);
        }

        private bool AllClonesInvisible()
        {
            for (int i = 0; i < clones.Count; i++)
            {
                if (clones[i].color.a > 0f)
                    return false;
            }
            return true;
        }

        public override void Deactivate()
        {
            if (clones != null)
            {
                for (int i = 0; i < clones.Count; i++)
                {
                    if (clones[i] != null)
                    {
                        clones[i].name = $"AfterImage_MarkedToDelete_{id}_{i}";
                        Object.Destroy(clones[i].gameObject);
                    }
                }
                clones.Clear();
            }
            finished = true;
        }
    }

    //public class SpriteAfterImageEffect : IEffects
    //{
    //    private SpriteRenderer original;
    //    private List<GameObject> clones;
    //    private SpriteRenderer cloneRenderer;

    //    private float lifeTime;
    //    private float fadeSpeed;
    //    private float currentTime;
    //    private float delayTime;
    //    private float delayTimer;


    //    private int limit;
    //    private int current;
    //    private int reverseCurrent;
    //    private bool finished;

    //    public bool IsFinished => finished;

    //    public SpriteAfterImageEffect(SpriteRenderer original, int limit = 4, float delayTime = 0.1f, float lifeTime = 0.3f, float fadeSpeed = 3f)
    //    {
    //        this.original = original;
    //        this.lifeTime = lifeTime;
    //        this.fadeSpeed = fadeSpeed;
    //        this.delayTime = delayTime;
    //        this.limit = limit;
    //    }

    //    public void Activate()
    //    {
    //        currentTime = 0;
    //        finished = false;
    //        delayTimer = delayTime + 1;
    //        clones = new();
    //    }

    //    public void UpdateEffects(float deltaTime)
    //    {
    //        Debug.Log("index" + current);
    //        if (delayTimer > delayTime)
    //        {
    //            delayTimer = 0;
    //            if (clones.Count < limit)
    //            {
    //                clones.Add(GetGameObject(original));
    //            }
    //            else
    //            {
    //                ResetRender(clones[reverseCurrent].GetComponent<SpriteRenderer>());
    //                reverseCurrent++;
    //                if (reverseCurrent > limit - 1)
    //                    reverseCurrent = 0;
    //            }
    //        }
    //        UpdateSpriteRender(deltaTime);
    //        currentTime += deltaTime;
    //        if (currentTime > lifeTime)
    //            Deactivate();
    //        //if (currentTime >= lifeTime || c.a <= 0f)
    //        //{
    //        //    Deactivate();
    //        //}
    //        if (delayTimer == 0)
    //        {
    //            current++;
    //            if (current > limit - 1)
    //                current = 0;
    //        }
    //        delayTimer += deltaTime;
    //    }
    //    private GameObject GetGameObject(SpriteRenderer original)
    //    {
    //        GameObject clone = new GameObject("AfterImage");
    //        cloneRenderer = clone.AddComponent<SpriteRenderer>();
    //        //  clone.transform.parent = original.transform;

    //        cloneRenderer.sprite = original.sprite;
    //        cloneRenderer.flipX = original.flipX;
    //        cloneRenderer.flipY = original.flipY;
    //        cloneRenderer.color = new Color(1, 1, 1, 1);
    //        cloneRenderer.sortingLayerID = original.sortingLayerID;
    //        cloneRenderer.sortingOrder = original.sortingOrder - 1;

    //        clone.transform.position = original.transform.position;
    //        //+ (Vector3.right * Mathf.Sign(original.transform.localScale.x) * (0.5f * (current+1)));
    //        clone.transform.rotation = original.transform.rotation;
    //        clone.transform.localScale = original.transform.lossyScale;

    //        return clone;
    //    }
    //    private void UpdateSpriteRender(float deltaTime)
    //    {
    //        for (int i = 0; i < clones.Count; i++)
    //        {
    //            cloneRenderer = clones[i].GetComponent<SpriteRenderer>();
    //            if (cloneRenderer == null)
    //                continue;
    //            cloneRenderer.sprite = original.sprite;
    //            //if (finished || cloneRenderer == null)
    //            //    return;
    //            Color c = cloneRenderer.color;

    //            c.a -= fadeSpeed * deltaTime;
    //            cloneRenderer.color = c;

    //            if (c.a <= 0)
    //            {
    //                ResetRender(cloneRenderer);
    //            }

    //        }
    //    }
    //    private void ResetRender(SpriteRenderer cloneRenderer)
    //    {
    //        cloneRenderer.color = new Color(1, 1, 1, 1);
    //        cloneRenderer.transform.position = original.transform.position;
    //    }
    //    public void Deactivate()
    //    {
    //        for (int i = 0; i < limit; i++)
    //        {
    //            if (clones[i] != null)
    //                Object.Destroy(clones[i]);
    //        }

    //        finished = true;
    //    }
    //}

}
