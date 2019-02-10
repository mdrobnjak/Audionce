//using OpenTK;
//using System.Collections;
//using System.Collections.Generic;


//public class AudioFlowfield
//{

//    NoiseFlowfield noiseFlowfield;
//    //public AudioPeer audioPeer;
    
//    public bool useScale;
//    public float scale;
//    public Vector2 scaleMinMax;

    
//    public bool useSpeed;
//    public Vector2 moveSpeedMinMax, rotateSpeedMinMax;


//    // Use this for initialization
//    void Start()
//    {
//        noiseFlowfield = GetComponent<NoiseFlowfield>();
//        int countBand = 0;
//        int band;
//        for (int i = 0; i < noiseFlowfield.actualAmountOfParticles; i++)
//        {
//            band = countBand % 8;
//            noiseFlowfield.particles[i].audioBand = band;       //particles[i].audioBand = band;
//            countBand++;
//        }
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        if (useSpeed)
//        {
//            noiseFlowfield.particleMoveSpeed = Mathf.Lerp(moveSpeedMinMax.x, moveSpeedMinMax.y, Band.bands[0].average);//AudioPeer._AmplitudeBuffer);
//            noiseFlowfield.particleRotateSpeed = Mathf.Lerp(rotateSpeedMinMax.x, rotateSpeedMinMax.y, Band.bands[0].average);//AudioPeer._AmplitudeBuffer);
//        }
//        if (useScale)
//        {
//            for (int i = 0; i < noiseFlowfield.actualAmountOfParticles; i++)
//            {
//                scale = Mathf.Lerp(scaleMinMax.x, scaleMinMax.y, AudioPeer._bandBuffer[noiseFlowfield.particles[i].audioBand] * 0.1f);
//                noiseFlowfield.particles[i].transform.localScale = new Vector3(scale, scale, scale);
//            }
//        }
//    }
//}
