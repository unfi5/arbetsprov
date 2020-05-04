using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TutorialEnemy : MonoBehaviour
{
    [SerializeField] Tutorial _tut;

    TextMesh _notice;
    NavMeshAgent _agent;
    TutorialEHead _head;

    // Start is called before the first frame update
    void Awake()
    {
        _notice = GetComponentInChildren<TextMesh>();
        _notice.color = new Vector4(_notice.color.r, _notice.color.g, _notice.color.b, 0.0f);
        _agent = GetComponent<NavMeshAgent>();
        _head = GetComponentInChildren<TutorialEHead>();
    }

    public void Throwable(Transform location)
    {
        _notice.text = "?";
        _head.FaceTarget(location);
        _agent.SetDestination(location.position);
        _notice.color = new Vector4(_notice.color.r, _notice.color.g, _notice.color.b, 1.0f);
        _tut.room3Lure += 1;
        StartCoroutine(NoticeDelay());
    }

    private IEnumerator NoticeDelay()
    {
        yield return new WaitForSeconds(1f);
        StartCoroutine(NoticeFadeTimer());
        _notice.color = new Vector4(_notice.color.r, _notice.color.g, _notice.color.b, 0.0f);
    }

    private IEnumerator NoticeFadeTimer()
    {
        float Fade = 1;
        while (Fade > 0)
        {
            yield return new WaitForSeconds(0.1f);
            Fade -= 0.1f;
            _notice.color = new Vector4(_notice.color.r, _notice.color.g, _notice.color.b, Fade);
        }
    }
}
