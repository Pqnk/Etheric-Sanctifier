using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutoManagerScrolls : MonoBehaviour
{
    [Header("Scrolls")]
    [SerializeField] private GameObject _scrollsTutoParent;
    [SerializeField] private int _currentIndexScroll = 0;
    [SerializeField] private bool _isShootingLearned = false;
    [SerializeField] private bool _isSwordLearned = false;
    [SerializeField] private float _timeToLaunchTuto = 3.0f;
    [SerializeField] private float _timeIntroScroll = 5.0f;
    private Scroll currentScroll;
    [SerializeField] private GameObject[] _scrollsTuto;
    private bool _isAlreadyNext = false;

    private void Start()
    {
        _scrollsTuto = GetAllScrolls();
        currentScroll = _scrollsTuto[_currentIndexScroll].GetComponent<Scroll>();
        StartCoroutine(LaunchTutorial());
    }
    private void Update()
    {
        if (currentScroll.isLearned && !_isAlreadyNext)
        {
            StartCoroutine(NextScrollTutorial());
        }
    }

    public GameObject[] GetAllScrolls()
    {
        int childCount = _scrollsTutoParent.transform.childCount;
        GameObject[] children = new GameObject[childCount];

        for (int i = 0; i < childCount; i++)
        {
            children[i] = _scrollsTutoParent.transform.GetChild(i).gameObject;
            children[i].SetActive(false);
        }

        return children;
    }

    IEnumerator LaunchTutorial()
    {
        yield return new WaitForSeconds(_timeToLaunchTuto);
        _scrollsTuto[_currentIndexScroll].SetActive(true);
    }

    public void FinishTutorial()
    {
        SuperManager.instance.ghostManager.SetCanSpawn(true);
    }

    IEnumerator NextScrollTutorial()
    {
        _isAlreadyNext = true;
        _scrollsTuto[_currentIndexScroll].SetActive(false);

        yield return new WaitForSeconds(2);

        _currentIndexScroll++;
        if (_currentIndexScroll < _scrollsTuto.Length)
        {
            currentScroll = _scrollsTuto[_currentIndexScroll].GetComponent<Scroll>();
            _scrollsTuto[_currentIndexScroll].SetActive(true);
            _isAlreadyNext = false;
        }
        else
        {
            FinishTutorial();
        }
    }
}
