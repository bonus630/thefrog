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

        private void Start()
        {
            if (teleported == null)
                teleported = ServiceLocator.Instance.Get("Player");
        }

        public override void Activate()
        {

            if (teleported.TryGetComponent<IPlayer>(out var player))
            {
                AdvancedTeleport();
            }
            else
            {
                StartCoroutine(SimpleTeleport());
            }
        }
        private IEnumerator SimpleTeleport()
        {
            yield return new WaitForSeconds(delayTime);
            ExecuteTeleport();
        }
        private void AdvancedTeleport()
        {
            var director = teleported.GetComponent<IScheduler>();
            var screenFx = ServiceLocator.Instance.Get<ScreenEffects>().screenFader;
            IPlayer player = teleported.GetComponent<IPlayer>();
            if (cancel) return;

            // 1️⃣ Delay inicial
            director.AddAction(new SchedulerData(
                () => { },
                delayTime
            ));
            director.AddAction(new SchedulerData(
                () => { player.AllInputsOn(false); },
                0f
            ));

            if (fade && screenFx != null)
            {
                // 2️⃣ FadeOut
                director.AddAction(new SchedulerData(
                    () => screenFx.FadeOut(0.5f),
                    0.5f
                ));
            }

            // 3️⃣ Teleporte crítico
            director.AddAction(new SchedulerData(
                () =>
                {
                    ExecuteTeleport();
                },
                0f
            ));

            // 4️⃣ Pequeno delay pós teleporte
            director.AddAction(new SchedulerData(
                () => { },
                0.1f
            ));

            // 5️⃣ ActiveOnArrival
            if (ActiveOnArrival != null)
            {
                director.AddAction(new SchedulerData(
                    () => ActiveOnArrival.Activate(),
                    0f
                ));
            }

            if (fade && screenFx != null)
            {
                // 6️⃣ Espera estabilização câmera (tempo máximo)
                director.AddAction(new SchedulerData(
                    () => { },
                    0.3f   // ou valor que você julgar seguro
                ));

                // 7️⃣ FadeIn
                director.AddAction(new SchedulerData(
                    () => screenFx.FadeIn(0.5f),
                    0.5f
                ));
            }
            director.AddAction(new SchedulerData(
           () => { player.AllInputsOn(true); },
           0f
             ));
        }
        private void ScheduleWithAgendador(IScheduler schedule)
        {
            var screenFx = ServiceLocator.Instance.Get<ScreenEffects>().screenFader;

            schedule.AddAction(new SchedulerData(() => { }, delayTime));

            if (fade && screenFx != null)
            {
                schedule.AddAction(new SchedulerData(
                    () => screenFx.FadeOut(0.5f),
                    0.5f
                ));
            }

            schedule.AddAction(new SchedulerData(ExecuteTeleport, 0f));
            schedule.AddAction(new SchedulerData(() => { }, 0.1f));

            if (ActiveOnArrival != null)
                schedule.AddAction(new SchedulerData(() => ActiveOnArrival.Activate(), 0f));

            if (fade && screenFx != null)
                schedule.AddAction(new SchedulerData(() => screenFx.FadeIn(0.5f), 0.5f));
        }

        //public override void Activate()
        //{
        //    StartCoroutine(Acvation());
        //}
        //private IEnumerator Acvation()
        //{
        //    yield return new WaitForSeconds(delayTime);
        //    if (cancel)
        //        yield break;
        //    var screenFx = ServiceLocator.Instance.Get<ScreenEffects>().screenFader;
        //    if (fade && screenFx != null)
        //    {
        //        Debug.Log("[Teleporter] screenFx:" + screenFx);
        //        screenFx.fadeDuration = 0.5f;
        //        yield return StartCoroutine(screenFx.FadeOut());
        //    }
        //    SpriteRenderer render = null;
        //    Rigidbody2D rb = null;
        //    IPlayer player = null;

        //    if (teleported.TryGetComponent<SpriteRenderer>(out render))
        //        render.enabled = false;
        //    if (teleported.TryGetComponent<Rigidbody2D>(out rb))
        //    {
        //        rb.bodyType = RigidbodyType2D.Kinematic;
        //        rb.linearVelocity = Vector2.zero;
        //    }
        //    if (teleported.TryGetComponent<IPlayer>(out player))
        //        player.AllInputsOn(false);
        //    teleported.transform.position = to.transform.position;
        //    if (render != null)
        //        render.enabled = true;
        //    if (rb != null)
        //        rb.bodyType = RigidbodyType2D.Dynamic;
        //    if (player != null)
        //        player.AllInputsOn(true);
        //    yield return new WaitForSeconds(0.1f);
        //    if (ActiveOnArrival != null)
        //        ActiveOnArrival.Activate();
        //    if (fade && screenFx != null)
        //    {
        //        Vector2 targetPosition = new Vector2(to.transform.position.x, to.transform.position.y);
        //        var cam = Camera.main.transform;
        //       // Debug.Log("[Teleporter] cam:" + cam);
        //        int frameLimiter = 90;
        //        while (cam.position.Distance2D(targetPosition) > 0.2f && (frameLimiter--) >= 0)
        //        {
        //           // Debug.Log("[Teleporter] cam distance:" + cam.position.Distance2D(targetPosition));
        //            yield return null;
        //        }

        //        yield return StartCoroutine(screenFx.FadeIn());
        //    }
        //}
        private void ExecuteTeleport()
        {
            SpriteRenderer render = null;
            Rigidbody2D rb = null;

            if (teleported.TryGetComponent(out render))
                render.enabled = false;

            if (teleported.TryGetComponent(out rb))
            {
                rb.bodyType = RigidbodyType2D.Kinematic;
                rb.linearVelocity = Vector2.zero;
            }
            teleported.transform.position = to.transform.position;

            if (render != null)
                render.enabled = true;

            if (rb != null)
                rb.bodyType = RigidbodyType2D.Dynamic;

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
