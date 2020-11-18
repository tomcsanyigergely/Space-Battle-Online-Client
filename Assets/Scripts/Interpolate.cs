using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UInt8 = System.Byte;

public class Interpolate : MonoBehaviour
{
    private class Point {
        public Vector3 position;
        public float direction;
        public float time;
        public Vector3 tangent;
        public UInt8 tickCounter;
    }

    public static bool interpolate = false;

    public bool interpolateRotation = true;
    
    private static float tickRate = 20;
    private static float tickPeriod = 1.0f / tickRate;
    private static float packetWindow = 2f; // 2 seconds
    private static float tickCounterWindow = packetWindow * tickRate; // 10, we allow messages with tickcounter from the next 10 in the following 2 seconds, then we allow anything
    
    public static float delay = tickPeriod * 2f;

    private List<Point> points = new List<Point>();
    private float timeOfLastValidMessage;

    private Vector3 tangent;

    // Update is called once per frame
    private void Update()
    {
        if (interpolate)
        {
            if (points.Count == 3)
            {
                if (Time.time <= points[1].time)
                {
                    transform.localPosition = GetHermiteInterpolation(0);

                    if (interpolateRotation)
                    {
                        GetComponent<SpaceshipDrawer>().SetRotation(GetLinearInterpolation(0));
                    }
                }
                else if (Time.time <= points[2].time)
                {
                    float angle = Vector3.Angle(points[1].tangent, points[2].position - points[1].position);
                    points[2].tangent = Quaternion.AngleAxis(angle, new Vector3(0, 0, -1)) * (points[2].position - points[1].position);
                    transform.localPosition = GetHermiteInterpolation(1);

                    if (interpolateRotation)
                    {
                        GetComponent<SpaceshipDrawer>().SetRotation(GetLinearInterpolation(1));
                    }
                }
                else
                {
                    transform.localPosition = points[2].position;
                }
            }
        }
        else
        {
            if (points.Count >= 1)
            {
                transform.localPosition = points[points.Count - 1].position;
                GetComponent<SpaceshipDrawer>().SetRotation(points[points.Count - 1].direction);
            }
        }
    }

    public void Clear()
    {
        points.Clear();
    }

    public void SetInterpolateRotation(bool value)
    {
        interpolateRotation = value;
    }

    public void AddPoint(Vector3 position, float direction, UInt8 tickCounter)
    {
        if (points.Count == 0 ||
            Time.time <= timeOfLastValidMessage + packetWindow && ((UInt8)(tickCounter - points[points.Count - 1].tickCounter - 1)) <= tickCounterWindow)
        {
            timeOfLastValidMessage = Time.time;
            Point newPoint = new Point();
            newPoint.position = position;
            newPoint.direction = direction;
            newPoint.tangent = new Vector3(0, 0, 0);
            newPoint.tickCounter = tickCounter;
            points.Add(newPoint);

            if (points.Count >= 2)
            {
                UInt8 tickDifference = (UInt8)(points[points.Count - 1].tickCounter - points[points.Count - 2].tickCounter);

                points[points.Count - 1].time = points[points.Count - 2].time + tickDifference * tickPeriod;

                if (points[points.Count - 1].time <= Time.time)
                {
                    points[points.Count - 1].time = Time.time + 2 * tickPeriod;
                }
                else
                {

                    float lerp = tickPeriod / 10 * tickDifference;
                    if (points[points.Count - 1].time >= Time.time + 2 * tickPeriod)
                    {
                        if (Mathf.Abs(points[points.Count - 1].time - (Time.time + 2 * tickPeriod)) >= lerp)
                        {
                            points[points.Count - 1].time -= lerp;
                        }
                        else
                        {
                            points[points.Count - 1].time = Time.time + 2 * tickPeriod;
                        }
                    }
                    else
                    {
                        if (Mathf.Abs(points[points.Count - 1].time - (Time.time + 2 * tickPeriod)) >= lerp)
                        {
                            points[points.Count - 1].time += lerp;
                        }
                        else
                        {
                            points[points.Count - 1].time = Time.time + 2 * tickPeriod;
                        }
                    }
                }
            }
            else if (points.Count == 1)
            {
                points[0].time = Time.time + 2 * tickPeriod;
            }

            if (points.Count > 3)
            {
                points.RemoveAt(0);
            }

            if (points.Count == 3)
            {
                CalculateTangent(1);

                if (Time.time - Time.deltaTime >= points[1].time)
                {
                    points[0].position = transform.localPosition;
                    points[0].time = Time.time - Time.deltaTime;
                    points[0].tangent = tangent;
                }
            }
        }
        else if (Time.time > timeOfLastValidMessage + packetWindow)
        {
            timeOfLastValidMessage = Time.time;

            points.Clear();
            Point newPoint = new Point();
            newPoint.position = position;
            newPoint.tangent = new Vector3(0, 0, 0);
            newPoint.direction = direction;
            newPoint.tickCounter = tickCounter;
            newPoint.time = Time.time + 2 * tickPeriod;
            points.Add(newPoint);
        }
    }

    private void CalculateTangent(int i)
    {
        points[i].tangent = 0.5f * ((points[i+1].position-points[i].position) / (points[i+1].time-points[i].time) + (points[i].position - points[i-1].position) / (points[i].time - points[i-1].time));
    }

    private Vector3 GetHermiteInterpolation(int i)
    {
        points[i].position = transform.localPosition;
        points[i].time = Time.time - Time.deltaTime;
        points[i].tangent = tangent;

        if (Mathf.Abs(Time.time - points[i].time) < 0.0001f)
        {
            return points[i].position;
        }
        else if (Mathf.Abs(Time.time - points[i+1].time) < 0.0001f)
        {
            return points[i+1].position;
        }
        else
        {

            Vector3 a0 = points[i].position;
            Vector3 a1 = points[i].tangent;
            Vector3 a2 = 3 * (points[i+1].position - points[i].position) / Mathf.Pow(points[i+1].time - points[i].time, 2) - (points[i+1].tangent + 2 * points[i].tangent) / (points[i+1].time - points[i].time);
            Vector3 a3 = 2 * (points[i].position - points[i+1].position) / Mathf.Pow(points[i+1].time - points[i].time, 3) + (points[i+1].tangent + points[i].tangent) / Mathf.Pow(points[i+1].time - points[i].time, 2);

            tangent = 3 * a3 * Mathf.Pow(Time.time - points[i].time, 2) + 2 * a2 * (Time.time - points[i].time) + a1;

            Vector3 result = a3 * Mathf.Pow(Time.time - points[i].time, 3) + a2 * Mathf.Pow(Time.time - points[i].time, 2) + a1 * (Time.time - points[i].time) + a0;

            return result;
        }
    }

    private float GetLinearInterpolation(int i)
    {
        points[i].direction = transform.Find("Empty").Find("Empty").Find("Model").localRotation.eulerAngles.z;

        if (Mathf.Abs(Time.time - points[i].time) < 0.0001f)
        {
            return points[i].direction;
        }
        else if (Mathf.Abs(Time.time - points[i + 1].time) < 0.0001f)
        {
            return points[i + 1].direction;
        }
        else
        {
            Vector3 from = Quaternion.AngleAxis(points[i].direction, new Vector3(0, 0, 1)) * new Vector3(1, 0, 0);
            Vector3 to = Quaternion.AngleAxis(points[i + 1].direction, new Vector3(0, 0, 1)) * new Vector3(1, 0, 0);

            float angle = Vector3.Angle(from, to);

            float rotationAngle = (Time.time - points[i].time) / (points[i + 1].time - points[i].time) * angle;

            Vector3 asd = Vector3.RotateTowards(from, to, rotationAngle / 180.0f * Mathf.PI, 0);

            return Mathf.Atan2(asd.y, asd.x) * 180f / Mathf.PI;
        }
    }
}
