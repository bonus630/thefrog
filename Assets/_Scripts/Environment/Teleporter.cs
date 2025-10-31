using System.Collections;
using br.com.bonus630.thefrog.Manager;
using br.com.bonus630.thefrog.Shared;
using br.com.bonus630.thefrog.Utils;
using UnityEngine;

namespace br.com.bonus630.thefrog.Environment
{
    public class Teleporter : IActivator
    {
        [SerializeField] GameObject teleported;
        [SerializeField] GameObject from;
        [SerializeField] GameObject to;
        [SerializeField] bool Auto;
        [SerializeField] float delayTime;
        [SerializeField] IActivator ActiveOnArrival;
        [SerializeField] bool fade;
        //[SerializeField] IActivator ActiveOnArrival;

        bool cancel = false;

        public override void Activate()
        {

            StartCoroutine(Acvation());
        }
        private IEnumerator Acvation()
        {
            yield return new WaitForSeconds(delayTime);
            if (cancel)
                yield break;
            var screenFx = ServiceLocator.Instance.Get<ScreenEffects>().screenFader;
            if (fade && screenFx != null)
            {
                Debug.Log("[Teleporter] screenFx:" + screenFx);
                screenFx.fadeDuration = 0.5f;
                yield return StartCoroutine(screenFx.FadeOut());
            }
            SpriteRenderer render = null;
            Rigidbody2D rb = null;
            IPlayer player = null;

            if (teleported.TryGetComponent<SpriteRenderer>(out render))
                render.enabled = false;
            if (teleported.TryGetComponent<Rigidbody2D>(out rb))
            {
                rb.bodyType = RigidbodyType2D.Kinematic;
                rb.linearVelocity = Vector2.zero;
            }
            if (teleported.TryGetComponent<IPlayer>(out player))
                player.AllInputsOn(false);
            teleported.transform.position = to.transform.position;
            if (render != null)
                render.enabled = true;
            if (rb != null)
                rb.bodyType = RigidbodyType2D.Dynamic;
            if (player != null)
                player.AllInputsOn(true);
            yield return new WaitForSeconds(0.1f);
            if (ActiveOnArrival != null)
                ActiveOnArrival.Activate();
            if (fade && screenFx != null)
            {
                Vector2 targetPosition = new Vector2(to.transform.position.x, to.transform.position.y);
                var cam = Camera.main.transform;
               // Debug.Log("[Teleporter] cam:" + cam);
                int frameLimiter = 90;
                while (cam.position.Distance2D(targetPosition) > 0.2f && (frameLimiter--) >= 0)
                {
                   // Debug.Log("[Teleporter] cam distance:" + cam.position.Distance2D(targetPosition));
                    yield return null;
                }

                yield return StartCoroutine(screenFx.FadeIn());
            }
        }
        //private IEnumerator FadeOutWait()
        //{
        //    while (!CameraUtils.GetBounds2D().Contains(teleported.transform.position))
        //        yield return new WaitForSeconds(0.1f);
        //}


        //private IEnumerator Acvation()
        //{

        //    yield return new WaitForSeconds(delayTime);
        //    if(cancel)
        //        yield break;
        //    if (fade)
        //        ServiceLocator.Get<ScreenEffects>().FadeIn(0.5f);
        //    SpriteRenderer render = null;
        //    Rigidbody2D rb = null;
        //    IPlayer player = null;
        //    if (teleported.TryGetComponent<SpriteRenderer>(out render))
        //    {
        //        render.enabled = false;
        //    }
        //    if (teleported.TryGetComponent<Rigidbody2D>(out rb))
        //    {
        //       // Debug.Log("TEleporter: ");
        //        rb.bodyType = RigidbodyType2D.Kinematic;
        //        rb.linearVelocity = Vector2.zero;
        //    }
        //    if (teleported.TryGetComponent<IPlayer>(out player))
        //    {
        //        player.MoveInputOn = false;
        //    }
        //    teleported.transform.position = to.transform.position;
        //    if (fade)
        //        StartCoroutine(FadeOut());
        //    yield return new WaitForSeconds(0.1f);
        //    if (render != null)
        //        render.enabled = true;
        //    if (rb != null)
        //        rb.bodyType = RigidbodyType2D.Dynamic;
        //    if(player!=null)
        //        player.MoveInputOn = true;
        //    if (ActiveOnArrival != null)
        //        ActiveOnArrival.Activate();


        //}
        //IEnumerator FadeOut()
        //{
        //    while(!CameraUtils.GetBounds2D().Contains(teleported.transform.position))
        //        yield return new WaitForSeconds(0.1f);
        //    ServiceLocator.Get<ScreenEffects>().FadeOut(0f);

        //}
        public override void Deactive()
        {
            cancel = true;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            // Debug.Log("Teleported tag + " + teleported.tag);
            if (teleported == null)
                return;
            if (collision != null && collision.CompareTag(teleported.tag))
            {
                cancel = false;
                if (Auto)
                    Activate();
            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (teleported == null)
                return;
            if (collision != null && collision.CompareTag(teleported.tag))
            {
                Deactive();
            }
        }
    }
}
