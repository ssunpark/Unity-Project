using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerX : MonoBehaviour
{
    public bool gameOver;

    public float floatForce = 8; // 상승력 크기를 줄임
    private float gravityModifier = 1.5f;
    private Rigidbody playerRb;

    public ParticleSystem explosionParticle;
    public ParticleSystem fireworksParticle;

    private AudioSource playerAudio;
    public AudioClip moneySound;
    public AudioClip explodeSound;
    public AudioClip bounceSound;

    private float upperLimit = 8.0f; // 최대 높이 제한
    private bool isLowEnough = true; // 너무 높이 올라가지 않도록 확인

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        Physics.gravity *= gravityModifier; // 중력 보정
        playerAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        // 현재 풍선의 높이가 upperLimit 이하인지 확인
        isLowEnough = transform.position.y < upperLimit;

        // 게임이 끝나지 않았고, 스페이스바를 눌렀으며, 높이가 제한선 이하일 때만 힘을 추가
        if (Input.GetKey(KeyCode.Space) && isLowEnough && !gameOver)
        {
            // 상승력을 천천히 적용하여 과도한 힘 방지
            playerRb.AddForce(Vector3.up * floatForce, ForceMode.Force);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        // 폭탄 충돌 처리
        if (other.gameObject.CompareTag("Bomb"))
        {
            explosionParticle.Play();
            playerAudio.PlayOneShot(explodeSound, 1.0f);
            gameOver = true;
            Debug.Log("Game Over!");
            Destroy(other.gameObject);
        }
        // 돈 충돌 처리
        else if (other.gameObject.CompareTag("Money"))
        {
            fireworksParticle.transform.position = transform.position;
            fireworksParticle.Play();
            playerAudio.PlayOneShot(moneySound, 1.0f);
            Destroy(other.gameObject);
        }
        // 지면 충돌 처리 (튕기는 효과)
        else if (other.gameObject.CompareTag("Ground"))
        {
            playerRb.AddForce(Vector3.up * 10, ForceMode.Impulse); // 지면과 충돌 시 튕기기
            playerAudio.PlayOneShot(bounceSound, 1.0f);
        }
    }
}
