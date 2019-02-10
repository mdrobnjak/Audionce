//using System;
//using System.Collections;
//using System.Collections.Generic;
//using OpenTK;

//public class NoiseFlowfield
//{
//    FastNoise fastNoise;

//    public Vector3 gridSize;
//    public float cellSize;
//    public Vector3[,,] flowfieldDirection;
//    public float increment;
//    public Vector3 offset, offsetSpeed;
//    //particles
//    public GameObject particlePrefab;
//    public int requestedAmountOfParticles;
//    public int actualAmountOfParticles;
//    public List<FlowfieldParticle> particles;
//    public float particleScale, particleMoveSpeed, particleRotateSpeed;
//    public float spawnRadius;

//    bool ParticleSpawnValidation(Vector3 position)
//    {
//        bool valid = true;
//        foreach(FlowfieldParticle particle in particles)
//        {
//            if(Vector3.Distance(position,particle.transform.position) < spawnRadius)
//            {
//                valid = false;
//                break;
//            }
//        }
//        if(valid)
//        {
//            return true;
//        }
//        else
//        {
//            return false;
//        }
//    }

//    // Use this for initialization
//    void Start()
//    {
//        flowfieldDirection = new Vector3[(int)gridSize.X, (int)gridSize.Y, (int)gridSize.Z];
//        fastNoise = new FastNoise();
//        particles = new List<FlowfieldParticle>();
//        int attempt; 

//        for(int i = 0; i < requestedAmountOfParticles; i++)
//        {
//            attempt = 0;

//            while (attempt < 100)
//            {
//                Vector3 randomPos = new Vector3(
//                    Random.Range(this.transform.position.x, this.transform.position.x + gridSize.x * cellSize),
//                    Random.Range(this.transform.position.y, this.transform.position.y + gridSize.y * cellSize),
//                    Random.Range(this.transform.position.z, this.transform.position.z + gridSize.z * cellSize));

//                if (ParticleSpawnValidation(randomPos))
//                {
//                    GameObject particleInstance = (GameObject)Instantiate(particlePrefab);
//                    particleInstance.transform.position = randomPos;
//                    particleInstance.transform.parent = this.transform;
//                    particleInstance.transform.localScale = new Vector3(particleScale, particleScale, particleScale);
//                    particles.Add(particleInstance.GetComponent<FlowfieldParticle>());
//                    break;
//                }
//                else
//                {
//                    attempt++;
//                } 
//            }
//        }
//        Debug.Log(particles.Count);
//        actualAmountOfParticles = particles.Count;
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        CalculateFlowFieldDirections();
//        ParticleBehavior();
//    }

//    private void CalculateFlowFieldDirections()
//    {
//        offset = new Vector3(offset.X + (offsetSpeed.X * Time.deltaTime), offset.Y + (offsetSpeed.Y * Time.deltaTime), offset.Z + (offsetSpeed.Z * Time.deltaTime));

//        float noise;
//        float xOff = 0f;
//        for (int x = 0; x < gridSize.X; x++)
//        {
//            float yOff = 0f;

//            for (int y = 0; y < gridSize.Y; y++)
//            {
//                float zOff = 0f;

//                for (int z = 0; z < gridSize.Z; z++)
//                {
//                    noise = fastNoise.GetSimplex(xOff + offset.X, yOff + offset.Y, zOff + offset.Z) + 1;
//                    Vector3 noiseDirection = new Vector3((float)Math.Cos(noise * Math.PI), (float)Math.Sin(noise * Math.PI), (float)Math.Cos(noise * Math.PI));
//                    flowfieldDirection[x, y, z] = Vector3.Normalize(noiseDirection);
//                    zOff += increment;
//                }

//                yOff += increment;
//            }

//            xOff += increment;
//        }
//    }

//    private void ParticleBehavior()
//    {
//        foreach(FlowfieldParticle p in particles)
//        {
//            //check edges - x
//            if(p.transform.position.x > this.transform.position.x + (gridSize.x * cellSize))
//            {
//                p.transform.position = new Vector3(this.transform.position.x, p.transform.position.y, p.transform.position.z);
//            }
//            if(p.transform.position.x < this.transform.position.x)
//            {
//                p.transform.position = new Vector3(this.transform.position.x + (gridSize.x * cellSize), p.transform.position.y, p.transform.position.z);
//            }
//            // y
//            if (p.transform.position.y > this.transform.position.y + (gridSize.y * cellSize))
//            {
//                p.transform.position = new Vector3(p.transform.position.x, this.transform.position.y, p.transform.position.z);
//            }
//            if (p.transform.position.y < this.transform.position.y)
//            {
//                p.transform.position = new Vector3(p.transform.position.x, this.transform.position.y + (gridSize.y * cellSize), p.transform.position.z);
//            }
//            // z
//            if (p.transform.position.z > this.transform.position.z + (gridSize.z * cellSize))
//            {
//                p.transform.position = new Vector3(p.transform.position.x, p.transform.position.y, this.transform.position.z);
//            }
//            if (p.transform.position.z < this.transform.position.z)
//            {
//                p.transform.position = new Vector3(p.transform.position.x , p.transform.position.y, this.transform.position.z + (gridSize.z * cellSize));
//            }

//            Vector3Int particlePos = new Vector3Int(
//                Mathf.FloorToInt(Mathf.Clamp((p.transform.position.x - this.transform.position.x) / cellSize, 0, gridSize.x - 1)),
//                Mathf.FloorToInt(Mathf.Clamp((p.transform.position.y - this.transform.position.y) / cellSize, 0, gridSize.y - 1)),
//                Mathf.FloorToInt(Mathf.Clamp((p.transform.position.z - this.transform.position.z) / cellSize, 0, gridSize.z - 1))
//                );
//            p.ApplyRotation(flowfieldDirection[particlePos.x,particlePos.y,particlePos.z], particleRotateSpeed);
//            p.moveSpeed = particleMoveSpeed;
//            //p.transform.localScale = new Vector3(particleScale,particleScale,particleScale);
//        }
//    }

//    private void OnDrawGizmos()
//    {
//        Gizmos.color = Color.white;
//        //Gizmos.DrawWireCube(this.transform.position + new Vector3((gridSize.x * cellSize) * 0.5f, (gridSize.y * cellSize) * 0.5f, (gridSize.z * cellSize) * 0.5f),
//        //    new Vector3(gridSize.x * cellSize, gridSize.y * cellSize, gridSize.z * cellSize));
//    }
//}
