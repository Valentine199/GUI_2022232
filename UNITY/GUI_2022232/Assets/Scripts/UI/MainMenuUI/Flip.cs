using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flip : MonoBehaviour
{
    [SerializeField] GameObject main;
    [SerializeField] GameObject options;
    [SerializeField] GameObject levelSelect;

    private Animator _animator;
    private bool _hasAnimator;
    private int _flipHash;

    // Start is called before the first frame update
    void Start()
    {
        _hasAnimator = TryGetComponent<Animator>(out _animator);
        _flipHash = Animator.StringToHash("flip");
        main.SetActive(true);
        options.SetActive(false);
        levelSelect.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void SwitchMain(GameObject to)
    {
        PlayAnim();
        StartCoroutine(WaitCoroutine(to));
        StartCoroutine(WaitCoroutine(main));
    }
    public void SwitchOptions(GameObject to)
    {
        PlayAnim();
        StartCoroutine(WaitCoroutine(to));
        StartCoroutine(WaitCoroutine(options));
    }
    public void SwitchLevelSelect(GameObject to)
    {
        PlayAnim();
        StartCoroutine(WaitCoroutine(to));
        StartCoroutine(WaitCoroutine(levelSelect));
    }

    public void PlayAnim()
    {
        _animator.SetTrigger(_flipHash);
    }

    IEnumerator WaitCoroutine(GameObject xd)
    {      
        yield return new WaitForSeconds(0.5f);
        xd.SetActive(!xd.activeSelf);       
    }
}
