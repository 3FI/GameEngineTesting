using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace GameEngine
{
    public static class PlayerMoveset
    {

        static Vector2[] UpTiltInput = { new Vector2(0, 1) };
        static void UpTilt(Player player)
        {
            System.Diagnostics.Debug.WriteLine("↑");
        }

        static Vector2[] UpClockwiseInput = { new Vector2(0, 1), new Vector2(0.5f, 0.5f), new Vector2(1, 0), new Vector2(0.5f, -0.5f), new Vector2(0, -1), new Vector2(-0.5f, -0.5f), new Vector2(-1, 0), new Vector2(-0.5f, 0.5f), new Vector2(0, 1) };
        static Vector2[] UpCounterClockwiseInput = { new Vector2(0, 1), new Vector2(-0.5f, 0.5f), new Vector2(-1, 0), new Vector2(-0.5f, -0.5f), new Vector2(0, -1), new Vector2(0.5f, -0.5f), new Vector2(1, 0), new Vector2(0.5f, 0.5f), new Vector2(0, 1) };

        static Vector2[] RightTiltInput = { new Vector2(1, 0) };
        static void RightTilt(Player player)
        {
            System.Diagnostics.Debug.WriteLine("→");
        }

        static Vector2[] RightClockwiseInput = { new Vector2(1, 0), new Vector2(0.5f, -0.5f), new Vector2(0, -1), new Vector2(-0.5f, -0.5f), new Vector2(-1, 0), new Vector2(-0.5f, 0.5f), new Vector2(0, 1), new Vector2(0.5f, 0.5f), new Vector2(1, 0) };
        static Vector2[] RightCounterClockwiseInput = { new Vector2(1, 0), new Vector2(0.5f, 0.5f), new Vector2(0, 1), new Vector2(-0.5f, 0.5f), new Vector2(-1, 0), new Vector2(-0.5f, -0.5f), new Vector2(0, -1), new Vector2(0.5f, -0.5f), new Vector2(1, 0) };

        static Vector2[] DownTiltInput = { new Vector2(0, -1) };
        static void DownTilt(Player player)
        {
            System.Diagnostics.Debug.WriteLine("↓");
        }

        static Vector2[] DownClockwiseInput = { new Vector2(0, -1), new Vector2(-0.5f, -0.5f), new Vector2(-1, 0), new Vector2(-0.5f, 0.5f), new Vector2(0, 1), new Vector2(0.5f, 0.5f), new Vector2(1, 0), new Vector2(0.5f, -0.5f), new Vector2(0, -1) };
        static Vector2[] DownCounterClockwiseInput = { new Vector2(0, -1), new Vector2(0.5f, -0.5f), new Vector2(1, 0), new Vector2(0.5f, 0.5f), new Vector2(0, 1), new Vector2(-0.5f, 0.5f), new Vector2(-1, 0), new Vector2(-0.5f, -0.5f), new Vector2(0, -1) };

        static Vector2[] LeftTiltInput = { new Vector2(-1, 0) };
        static void LeftTilt(Player player)
        {
            System.Diagnostics.Debug.WriteLine("←");
        }

        static Vector2[] LeftClockwiseInput = { new Vector2(-1, 0), new Vector2(-0.5f, 0.5f), new Vector2(0, 1), new Vector2(0.5f, 0.5f), new Vector2(1, 0), new Vector2(0.5f, -0.5f), new Vector2(0, -1), new Vector2(-0.5f, -0.5f), new Vector2(-1, 0) };
        static Vector2[] LeftCounterClockwiseInput = { new Vector2(-1, 0), new Vector2(-0.5f, -0.5f), new Vector2(0, -1), new Vector2(0.5f, -0.5f), new Vector2(1, 0), new Vector2(0.5f, 0.5f), new Vector2(0, 1), new Vector2(-0.5f, 0.5f), new Vector2(-1, 0) };

        static Vector2[] UpRightTiltInputV1 = { new Vector2(0.5f, 0.5f) };
        static Vector2[] UpRightTiltInputV2 = { new Vector2(0, 1), new Vector2(0.5f, 0.5f) };
        static Vector2[] UpRightTiltInputV3 = { new Vector2(1, 0), new Vector2(0.5f, 0.5f) };
        static Vector2[] UpRightClockwiseInput = { new Vector2(0.5f, 0.5f), new Vector2(1, 0), new Vector2(0.5f, -0.5f), new Vector2(0, -1), new Vector2(-0.5f, -0.5f), new Vector2(-1, 0), new Vector2(-0.5f, 0.5f), new Vector2(0, 1), new Vector2(0.5f, 0.5f) };
        static Vector2[] UpRightCounterClockwiseInput = { new Vector2(0.5f, 0.5f), new Vector2(0, 1), new Vector2(-0.5f, 0.5f), new Vector2(-1, 0), new Vector2(-0.5f, -0.5f), new Vector2(0, -1), new Vector2(0.5f, -0.5f), new Vector2(1, 0), new Vector2(0.5f, 0.5f) };
        static void UpRightTilt(Player player)
        {
            System.Diagnostics.Debug.WriteLine("🡥");
        }
        static Vector2[] DownRightTiltInputV1 = { new Vector2(0.5f, -0.5f) };
        static Vector2[] DownRightTiltInputV2 = { new Vector2(1, 0), new Vector2(0.5f, -0.5f) };
        static Vector2[] DownRightTiltInputV3 = { new Vector2(0, -1), new Vector2(0.5f, -0.5f) };
        static Vector2[] DownRightClockwiseInput = { new Vector2(0.5f, -0.5f), new Vector2(0, -1), new Vector2(-0.5f, -0.5f), new Vector2(-1, 0), new Vector2(-0.5f, 0.5f), new Vector2(0, 1), new Vector2(0.5f, 0.5f), new Vector2(1, 0), new Vector2(0.5f, -0.5f) };
        static Vector2[] DownRightCounterClockwiseInput = { new Vector2(0.5f, -0.5f), new Vector2(1, 0), new Vector2(0.5f, 0.5f), new Vector2(0, 1), new Vector2(-0.5f, 0.5f), new Vector2(-1, 0), new Vector2(-0.5f, -0.5f), new Vector2(0, -1), new Vector2(0.5f, -0.5f) };
        static void DownRightTilt(Player player)
        {
            System.Diagnostics.Debug.WriteLine("🡮");
        }
        static Vector2[] DownLeftTiltInputV1 = { new Vector2(-0.5f, -0.5f) };
        static Vector2[] DownLeftTiltInputV2 = { new Vector2(-1, 0), new Vector2(-0.5f, -0.5f) };
        static Vector2[] DownLeftTiltInputV3 = { new Vector2(0, -1), new Vector2(-0.5f, -0.5f) };
        static Vector2[] DownLeftClockwiseInput = { new Vector2(-0.5f, -0.5f), new Vector2(-1, 0), new Vector2(-0.5f, 0.5f), new Vector2(0, 1), new Vector2(0.5f, 0.5f), new Vector2(1, 0), new Vector2(0.5f, -0.5f), new Vector2(0, -1), new Vector2(-0.5f, -0.5f) };
        static Vector2[] DownLeftCounterClockwiseInput = { new Vector2(-0.5f, -0.5f), new Vector2(0, -1), new Vector2(0.5f, -0.5f), new Vector2(1, 0), new Vector2(0.5f, 0.5f), new Vector2(0, 1), new Vector2(-0.5f, 0.5f), new Vector2(-1, 0), new Vector2(-0.5f, -0.5f) };
        static void DownLeftTilt(Player player)
        {
            System.Diagnostics.Debug.WriteLine("🡯");
        }
        static Vector2[] UpLeftTiltInputV1 = { new Vector2(-0.5f, 0.5f) };
        static Vector2[] UpLeftTiltInputV2 = { new Vector2(-1, 0), new Vector2(-0.5f, 0.5f) };
        static Vector2[] UpLeftTiltInputV3 = { new Vector2(0, 1), new Vector2(-0.5f, 0.5f) };
        static Vector2[] UpLeftClockwiseInput = { new Vector2(-0.5f, 0.5f), new Vector2(0, 1), new Vector2(0.5f, 0.5f), new Vector2(1, 0), new Vector2(0.5f, -0.5f), new Vector2(0, -1), new Vector2(-0.5f, -0.5f), new Vector2(-1, 0), new Vector2(-0.5f, 0.5f) };
        static Vector2[] UpLeftCounterClockwiseInput = { new Vector2(-0.5f, 0.5f), new Vector2(-1, 0), new Vector2(-0.5f, -0.5f), new Vector2(0, -1), new Vector2(0.5f, -0.5f), new Vector2(1, 0), new Vector2(0.5f, 0.5f), new Vector2(0, 1), new Vector2(-0.5f, 0.5f) };
        static void UpLeftTilt(Player player)
        {
            System.Diagnostics.Debug.WriteLine("🡬");
        }

        static void FullCircle(Player player)
        {
            System.Diagnostics.Debug.WriteLine("⟲");
        }

        static public Dictionary<Vector2[], Action<Player>> attackLibrary = new Dictionary<Vector2[], Action<Player>>
                        {
                            //ALWAYS PUT THE LONGEST MOVE AT THE TOP OF THE DICTIONNARY
                            { UpClockwiseInput, new Action<Player>(FullCircle) },
                            { UpRightClockwiseInput, new Action<Player>(FullCircle) },
                            { RightClockwiseInput, new Action<Player>(FullCircle) },
                            { DownRightClockwiseInput, new Action<Player>(FullCircle) },
                            { DownClockwiseInput, new Action<Player>(FullCircle) },
                            { DownLeftClockwiseInput, new Action<Player>(FullCircle) },
                            { LeftClockwiseInput, new Action<Player>(FullCircle) },
                            { UpLeftClockwiseInput, new Action<Player>(FullCircle) },

                            { UpCounterClockwiseInput, new Action<Player>(FullCircle) },
                            { UpRightCounterClockwiseInput, new Action<Player>(FullCircle) },
                            { RightCounterClockwiseInput, new Action<Player>(FullCircle) },
                            { DownRightCounterClockwiseInput, new Action<Player>(FullCircle) },
                            { DownCounterClockwiseInput, new Action<Player>(FullCircle) },
                            { DownLeftCounterClockwiseInput, new Action<Player>(FullCircle) },
                            { LeftCounterClockwiseInput, new Action<Player>(FullCircle) },
                            { UpLeftCounterClockwiseInput, new Action<Player>(FullCircle) },

                            {UpRightTiltInputV2, new Action<Player>(UpRightTilt)},
                            {DownRightTiltInputV2, new Action<Player>(DownRightTilt)},
                            {DownLeftTiltInputV2, new Action<Player>(DownLeftTilt)},
                            {UpLeftTiltInputV2, new Action<Player>(UpLeftTilt)},
                            {UpRightTiltInputV3, new Action<Player>(UpRightTilt)},
                            {DownRightTiltInputV3, new Action<Player>(DownRightTilt)},
                            {DownLeftTiltInputV3, new Action<Player>(DownLeftTilt)},
                            {UpLeftTiltInputV3, new Action<Player>(UpLeftTilt)},

                            {UpTiltInput, new Action<Player>(UpTilt)},
                            {RightTiltInput, new Action<Player>(RightTilt)},
                            {DownTiltInput, new Action<Player>(DownTilt)},
                            {LeftTiltInput, new Action<Player>(LeftTilt)},

                            {UpRightTiltInputV1, new Action<Player>(UpRightTilt)},
                            {DownRightTiltInputV1, new Action<Player>(DownRightTilt)},
                            {DownLeftTiltInputV1, new Action<Player>(DownLeftTilt)},
                            {UpLeftTiltInputV1, new Action<Player>(UpLeftTilt)}
                        };

    }
}
