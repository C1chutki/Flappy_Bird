using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    private const float CAMERA_ORTHO_SIZE = 50f;
    private const float PIPE_WIDTH = 10.4f;
    private const float PIPE_HEAD_HEIGHT = 3f;
    private const float PIPE_MOVE_SPEED = 30f;
    private const float PIPE_DESTROY_X_POSITION = -125f;
    private const float PIPE_SPAWN_X_POSITION = +115f;
    private const float GROUND_DESTROY_X_POSITION = -200f;
    private const float BIRD_X_POSITION = 0f;

    private List<Pipe> pipeList;
    private List<Transform> GroundList;
    private int pipesPassedCount;
    private int pipesSpawned;

    private float pipeSpawnTimer;
    private float pipeSpawnTimerMax;
    private float gapSize;

    private State state;

    private static Level instance;
    public static Level GetInstance()
    {
        return instance;
    }

    public enum Difficulty
    {
        Easy,
        Medium,
        Hard,
        Impossible,
    }

    private enum State
    {
        WaitingToStart,
        Playing,
        BirdDead,
    }

    private void Awake()
    {
        instance = this;
        SpawnInitialGround();
        pipeList = new List<Pipe>();
        pipeSpawnTimerMax = 1f;
        SetDifficulty(Difficulty.Easy);
        state = State.WaitingToStart;
    }

    private void Start()
    {
        Bird.GetInstance().OnDied += Bird_OnDied;
        Bird.GetInstance().OnStartedPlaying += Bird_OnStartedPlaying;
    }

    private void Bird_OnStartedPlaying(object sender, System.EventArgs e)
    {
        state = State.Playing;
    }

    private void Bird_OnDied(object sender, System.EventArgs e)
    {
        state = State.BirdDead;
    }

    private void Update()
    {
        if (state == State.Playing) { 
        HandlePipeMovement();
        HandlePipeSpawning();
        HandleGround();
        }
    }

    private void SpawnInitialGround()
    {
        GroundList = new List<Transform>();
        Transform groundTransform;
        float groundY = -49f;
        float groundWidth = 192f;
        groundTransform = Instantiate(GameAssets.GetInstance().pfGround, new Vector3(0, groundY, 0), Quaternion.identity);
        GroundList.Add(groundTransform);
        groundTransform = Instantiate(GameAssets.GetInstance().pfGround, new Vector3(groundWidth, groundY, 0), Quaternion.identity);
        GroundList.Add(groundTransform);
        groundTransform = Instantiate(GameAssets.GetInstance().pfGround, new Vector3(groundWidth * 2f, groundY, 0), Quaternion.identity);
        GroundList.Add(groundTransform);
    }

    private void HandleGround()
    {
        foreach (Transform groundTransform in GroundList)
        {
            groundTransform.position += new Vector3(-1, 0, 0) * PIPE_MOVE_SPEED * Time.deltaTime;

            if (groundTransform.position.x < GROUND_DESTROY_X_POSITION)
            {
                // Ground passed the left sied, relocate on right side
                // find right most X position
                float rightMostXPosition = -100f;
                for (int i = 0; i < GroundList.Count; i++)
                {
                    if(GroundList[i].position.x > rightMostXPosition) {
                        rightMostXPosition = GroundList[i].position.x;
                    }
                }
                // Place ground on the right most position
                float groundWidth = 192f;
                groundTransform.position = new Vector3(rightMostXPosition + groundWidth, groundTransform.position.y, groundTransform.position.z);
            }
        }
    }

    
    private void HandlePipeSpawning()
    {
        pipeSpawnTimer -= Time.deltaTime;
        if (pipeSpawnTimer < 0)
        {
            //Time to spawn another pipe
            pipeSpawnTimer += pipeSpawnTimerMax;


            float heightEdgeLimit = 10f;
            float totalHeight = CAMERA_ORTHO_SIZE * 2f;

            float minHeight = gapSize * .5f + heightEdgeLimit;
            float maxHeight = totalHeight - gapSize * .5f -heightEdgeLimit;

            float height = Random.Range(minHeight, maxHeight);
            
            
            CreateGapPipes(height, gapSize, PIPE_SPAWN_X_POSITION);
        }
    }

    private void HandlePipeMovement()
    {
        for (int i = 0; i < pipeList.Count; i++)
        {
            Pipe pipe = pipeList[i];
            bool isToTheRightOfBird = pipe.GetXPosition() > BIRD_X_POSITION;
            pipe.Move();
            if (isToTheRightOfBird && pipe.GetXPosition() <= BIRD_X_POSITION)
            {
                //pipes passed bird
                pipesPassedCount++;
                SoundMenager.PlaySound(SoundMenager.Sound.Score);
            }
            if (pipe.GetXPosition() < PIPE_DESTROY_X_POSITION)
            {
                //Destroy Pipe
                pipe.DestroySelf();
                pipeList.Remove(pipe);
                i--;
            }
        }  
    }

    private void SetDifficulty(Difficulty difficulty)
    {
        switch (difficulty)
        {
            case Difficulty.Easy:
                gapSize = 50f;
                pipeSpawnTimerMax = 1.4f;
                break;
                
            case Difficulty.Medium:
                gapSize = 40f;
                pipeSpawnTimerMax = 1.3f;
                break;
                
            case Difficulty.Hard:
                gapSize = 33f;
                pipeSpawnTimerMax = 1.2f;
                break;
                
            case Difficulty.Impossible:
                gapSize = 24f;
                pipeSpawnTimerMax = 1f;
                break;

        }
    }

    private Difficulty GetDifficulty()
    {
        if (pipesSpawned >= 30) return Difficulty.Impossible;
        if (pipesSpawned >= 20) return Difficulty.Hard;
        if (pipesSpawned >= 10) return Difficulty.Medium;
        return Difficulty.Easy;

    }
    
    private void CreateGapPipes(float gapY, float gapSize, float xPosition)
    {
        CreatePipe(gapY - gapSize * .5f, xPosition, true);
        CreatePipe(CAMERA_ORTHO_SIZE * 2f - gapY - gapSize * .5f, xPosition, false);
        pipesSpawned++;
        SetDifficulty(GetDifficulty());
    }

    private void CreatePipe(float height, float xPosition, bool createBottom)
    {

        // Set up pipe Head
        Transform pipeHead = Instantiate(GameAssets.GetInstance().pfPipeHead);
        float pipeHeadYPosition;
        if (createBottom)
        {
            pipeHeadYPosition = -CAMERA_ORTHO_SIZE + height - PIPE_HEAD_HEIGHT * .5f;
        }
        else
        {
            pipeHeadYPosition = +CAMERA_ORTHO_SIZE - height + PIPE_HEAD_HEIGHT * .5f;
            pipeHead.transform.localScale = new Vector3(1, -1, 1);
        }
        pipeHead.position = new Vector3(xPosition, pipeHeadYPosition);

        // set up pipe body
        Transform pipeBody = Instantiate(GameAssets.GetInstance().pfPipeBody);
        float pipeBodyYPosition;
        if (createBottom)
        {
            pipeBodyYPosition = -CAMERA_ORTHO_SIZE;
        }
        else
        {
            pipeBodyYPosition = +CAMERA_ORTHO_SIZE;
            pipeBody.localScale = new Vector3(1, -1, 1);
        }
        pipeBody.position = new Vector3(xPosition, pipeBodyYPosition);

        SpriteRenderer pipeBodySpriteRenderer = pipeBody.GetComponent<SpriteRenderer>();
        pipeBodySpriteRenderer.size = new Vector2(PIPE_WIDTH, height);

        BoxCollider2D pipeBodyBoxCollider = pipeBody.GetComponent<BoxCollider2D>();
        pipeBodyBoxCollider.size = new Vector2(PIPE_WIDTH, height);
        pipeBodyBoxCollider.offset = new Vector2(0f, height * .5f);

        Pipe pipe = new Pipe(pipeHead, pipeBody);
        pipeList.Add(pipe);
             
    }

    public int GetPipesSpawned()
    {
        return pipesSpawned;
    }

    public int GetPipesPassedCount()
    {
        return pipesPassedCount/2;
    }

    private class Pipe {
        private Transform pipeHeadTransform;
        private Transform pipeBodyTransform;
        
        public Pipe(Transform pipeHeadTransform, Transform pipeBodyTransform)
        {
            this.pipeHeadTransform = pipeHeadTransform;
            this.pipeBodyTransform = pipeBodyTransform;
        }

        public void Move()
        {
            pipeHeadTransform.position += new Vector3(-1, 0, 0) * PIPE_MOVE_SPEED * Time.deltaTime;
            pipeBodyTransform.position += new Vector3(-1, 0, 0) * PIPE_MOVE_SPEED * Time.deltaTime;
        }

        public float GetXPosition()
        {
            return pipeHeadTransform.position.x;
        }

        public void DestroySelf()
        {
            Destroy(pipeHeadTransform.gameObject);
            Destroy(pipeBodyTransform.gameObject);
        }
    }
}