using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject Player;
    public GameObject BallPrefab;
    private Animator _playerAnimator;
    private GameObject _currentBall;
    private GameObject _mainCamera;
    private Camera _mainCamCamera;
    private Camera _ballCamera;
    // Start is called before the first frame update
    void Start()
    {
        _playerAnimator = Player.GetComponentInChildren<Animator>();
        _mainCamera = GameObject.Find("Main Camera");
        _mainCamCamera = _mainCamera.GetComponent<Camera>();


        _currentBall = Instantiate(BallPrefab, new Vector3(0.198f, 0.499f, -4.099f), Quaternion.identity);
        _ballCamera = _currentBall.GetComponentInChildren<Camera>();
        _ballCamera.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (_currentBall != null)
        {
            float dist = Vector3.Distance(_currentBall.transform.position, Player.transform.position);
            Debug.Log(dist * 1.094);
        }

        var state = _playerAnimator.GetCurrentAnimatorStateInfo(0);
        if (state.IsName("idle"))
        {
            if (Input.GetKeyDown("space"))
            {
                _playerAnimator.Play("Backswing");
            }

            if (_currentBall.tag == "struck")
            {
                _currentBall = Instantiate(BallPrefab, new Vector3(0.250f, 0.499f, -4.099f), Quaternion.identity);
                _ballCamera.gameObject.SetActive(false);
                _mainCamCamera.gameObject.SetActive(true);
                _ballCamera = _currentBall.GetComponentInChildren<Camera>();
                _ballCamera.gameObject.SetActive(false);

            }
        }

        var animStateInfo = _playerAnimator.GetCurrentAnimatorStateInfo(0);
        var nTime = animStateInfo.normalizedTime;

        if (state.IsName("Backswing"))
        {

            var badness = nTime < 1 ? 1 - nTime : nTime - 1;

            if (Input.GetKeyDown("space"))
            {
                _playerAnimator.Play("Frontswing");
                StartCoroutine(MoveBall(badness));
                //if (badness > )
                //Debug.Log("bad swing");
            }

        }

        if (Input.GetKeyDown("b") && _ballCamera.gameObject.activeSelf == false)
        {
            _ballCamera.gameObject.SetActive(true);
            _mainCamCamera.gameObject.SetActive(false);

        }

        if (Input.GetKeyUp("b"))
        {
            if (_ballCamera.gameObject.activeSelf == true)
            {
                _ballCamera.gameObject.SetActive(false);
                _mainCamCamera.gameObject.SetActive(true);

            }

        }


    }



    IEnumerator MoveBall(float badSwingAmount)
    {

        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(0.5f);

        var badMulti = badSwingAmount * 100;

        _currentBall.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(0 - badMulti, 0 + badMulti), Random.Range(80 - badMulti, 100 + badMulti), Random.Range(270 - badSwingAmount * 125, 310 - badSwingAmount * 50)));


        _currentBall.tag = "struck";

    }
}
