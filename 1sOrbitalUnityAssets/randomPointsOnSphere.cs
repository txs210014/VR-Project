using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class randomPointsOnSphere : MonoBehaviour
{
    // prefab object for each point in the graph
    public GameObject dotPrefab;
    void Start()
    {
        // number of points to generate
        int N = 10000;
        // generate N points around self
        for (int i = 0; i < N; i++) {

            // calculate random latitude and longitude on a sphere of radius 1
            var u1 = Random.Range(0.0f, 1.0f); // random number 1
            var u2 = Random.Range(0.0f, 1.0f); // random number 2
            var latitude = (Mathf.Acos((2 * u1) - 1) - (Mathf.PI / 2)); // equation to calculate a random latitude
            var longitude = (2 * Mathf.PI * u2); // equation to calculate a random longitude

            // calculate cartesion coordinates from lat and lon
            var x = Mathf.Cos(latitude) * Mathf.Cos(longitude); // calculate x
            var y = Mathf.Cos(latitude) * Mathf.Sin(longitude); // calculate y
            var z = Mathf.Sin(latitude); // calculate z

            // randomly assign coordinate a radius based on the PDF of 1s electron
            float radius = getRadius();
            // applying each radius to point
            x *= radius; // calculate new x
            y *= radius; // calculate new y
            z *= radius; // calculate new z

            // spawn object on screen at calculated coordinate
            Vector3 spawnPoint = new Vector3(x, y, z);
            Instantiate(dotPrefab, spawnPoint, this.transform.rotation);
        }
    }

    // samples the Electron PDF and returns the x value
    float getRadius()
    {
        bool foundone = false;
        while(!foundone) {
            // randomly pick 2 numbers within the domain of the probability density function (PDF)
            var r1 = Random.Range(0.0f, 4.38f); // random number 1
            var r2 = Random.Range(0.0f, 4.38f); // random number 2

            var e = 2.7182818284590451f; // value of e

            // calculate y value in PDF using random number 1 as x
            var p = 2.0f * r1; // p = 2Zr / n, n being principal quantum number i.e. number of protons, and Z being effective nuclear charge i.e. number of protons in the atom
            var r = 2.0f * Mathf.Pow(1.0f, (3.0f / 2.0f)) * Mathf.Pow(e, (-1.0f * p)); // radial wave function
            var y = Mathf.Sqrt((1.0f / (4.0f * Mathf.PI))); // angular wave function
            var w = r * y; // wave function
            var E = Mathf.Pow(w, 2.0f); // electron probability density function

            // is random number 2 less than y?
            if (r2 < E) { // if so, return random number 1 as radius of point
                foundone = true;
                return r1;
            }
            // if not, keep trying until successful
        }
        return 0.0f;
    }
}
