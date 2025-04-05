using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CapyScript.Data
{
    public static class DataGeneration
    {
        public static float RandomGaussian(float min, float max, float mu = 0, float sigma = 1)
        {
            float u1 = UnityEngine.Random.Range(0f, 1f);
            float u2 = UnityEngine.Random.Range(0f, 1f);

            float rand_std_normal = (float)(Math.Sqrt(-2.0 * Math.Log(u1)) *
                                Math.Sin(2.0 * Math.PI * u2));

            float rand_normal = mu + sigma * rand_std_normal;

            return Mathf.Lerp(min, max, rand_normal);
        }

        public static float RandomGaussian(float mu = 0, float sigma = 1)
        {
            float u1 = UnityEngine.Random.Range(0f, 1f);
            float u2 = UnityEngine.Random.Range(0f, 1f);

            float rand_std_normal = (float)(Math.Sqrt(-2.0 * Math.Log(u1)) *
                                Math.Sin(2.0 * Math.PI * u2));

            float rand_normal = mu + sigma * rand_std_normal;

            return rand_normal;
        }

        public static int RandomGaussian(int min, int max, float mu = 0, float sigma = 1)
        {
            float u1 = UnityEngine.Random.Range(0f, 1f);
            float u2 = UnityEngine.Random.Range(0f, 1f);

            float rand_std_normal = (float)(Math.Sqrt(-2.0 * Math.Log(u1)) *
                                Math.Sin(2.0 * Math.PI * u2));

            float rand_normal = mu + sigma * rand_std_normal;

            return Mathf.RoundToInt(Mathf.Lerp(min, max, rand_normal));
        }
    }
}
