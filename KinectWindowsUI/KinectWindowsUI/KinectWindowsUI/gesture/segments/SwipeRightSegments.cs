
using Microsoft.Kinect;

namespace KinectWindowsUI
{
    /// <summary>
    /// The first part of a <see cref="GestureType.SwipeRight"/> gesture.
    /// </summary>
    public class SwipeRightSegment1 : IGestureSegment
    {
        /// <summary>
        /// Updates the current gesture.
        /// </summary>
        /// <param name="body">The body.</param>
        /// <returns>A GesturePartResult based on whether the gesture part has been completed.</returns>
        public GesturePartResult Update(Body body)
        {
            // //Right hand in front of Right Shoulder
            if (body.Joints[JointType.HandRight].Position.Z < body.Joints[JointType.ElbowRight].Position.Z && body.Joints[JointType.HandRight].Position.Y < body.Joints[JointType.SpineBase].Position.Y)
            {
                // Debug.WriteLine("GesturePart 0 - Right hand in front of Right Shoulder - PASS");
                // //Right hand below shoulder height but above hip height
                if (body.Joints[JointType.HandRight].Position.Y < body.Joints[JointType.Head].Position.Y && body.Joints[JointType.HandRight].Position.Y > body.Joints[JointType.SpineBase].Position.Y)
                {
                    // Debug.WriteLine("GesturePart 0 - Right hand below shoulder height but above hip height - PASS");
                    // //Right hand left of Right Shoulder
                    if (body.Joints[JointType.HandRight].Position.X < body.Joints[JointType.ShoulderRight].Position.X)
                    {
                        // Debug.WriteLine("GesturePart 0 - Right hand left of Right Shoulder - PASS");
                        return GesturePartResult.Succeeded;
                    }

                    // Debug.WriteLine("GesturePart 0 - Right hand left of Right Shoulder - UNDETERMINED");
                    return GesturePartResult.Undetermined;
                }

                // Debug.WriteLine("GesturePart 0 - Right hand below shoulder height but above hip height - FAIL");
                return GesturePartResult.Failed;
            }

            // Debug.WriteLine("GesturePart 0 - Right hand in front of Right Shoulder - FAIL");
            return GesturePartResult.Failed;
        }
    }

    /// <summary>
    /// The second part of a <see cref="GestureType.SwipeRight"/> gesture.
    /// </summary>
    public class SwipeRightSegment2 : IGestureSegment
    {
        /// <summary>
        /// Updates the current gesture.
        /// </summary>
        /// <param name="body">The body.</param>
        /// <returns>A GesturePartResult based on whether the gesture part has been completed.</returns>
        public GesturePartResult Update(Body body)
        {
            // //Right hand in front of Right Shoulder
            if (body.Joints[JointType.HandRight].Position.Z < body.Joints[JointType.ElbowRight].Position.Z && body.Joints[JointType.HandRight].Position.Y < body.Joints[JointType.SpineBase].Position.Y)
            {
                // Debug.WriteLine("GesturePart 1 - Right hand in front of Right Shoulder - PASS");
                // /Right hand below shoulder height but above hip height
                if (body.Joints[JointType.HandRight].Position.Y < body.Joints[JointType.Head].Position.Y && body.Joints[JointType.HandRight].Position.Y > body.Joints[JointType.SpineBase].Position.Y)
                {
                    // Debug.WriteLine("GesturePart 1 - Right hand below shoulder height but above hip height - PASS");
                    // //Right hand left of Right Shoulder
                    if (body.Joints[JointType.HandRight].Position.X < body.Joints[JointType.ShoulderRight].Position.X || body.Joints[JointType.HandRight].Position.X > body.Joints[JointType.ShoulderRight].Position.X)
                    {
                        // Debug.WriteLine("GesturePart 1 - Right hand Right of Right Shoulder - PASS");
                        return GesturePartResult.Succeeded;
                    }

                    // Debug.WriteLine("GesturePart 1 - Right hand left of Right Shoulder - UNDETERMINED");
                    return GesturePartResult.Undetermined;
                }

                // Debug.WriteLine("GesturePart 1 - Right hand below shoulder height but above hip height - FAIL");
                return GesturePartResult.Failed;
            }

            // Debug.WriteLine("GesturePart 1 - Right hand in front of Right Shoulder - FAIL");
            return GesturePartResult.Failed;
        }
    }

    /// <summary>
    /// The third part of a <see cref="GestureType.SwipeRight"/> gesture.
    /// </summary>
    public class SwipeRightSegment3 : IGestureSegment
    {
        /// <summary>
        /// Updates the current gesture.
        /// </summary>
        /// <param name="body">The body.</param>
        /// <returns>A GesturePartResult based on whether the gesture part has been completed.</returns>
        public GesturePartResult Update(Body body)
        {
            // //Right hand in front of Right Shoulder
            if (body.Joints[JointType.HandRight].Position.Z < body.Joints[JointType.ElbowRight].Position.Z && body.Joints[JointType.HandRight].Position.Y < body.Joints[JointType.SpineBase].Position.Y)
            {
                // //Right hand below shoulder height but above hip height
                if (body.Joints[JointType.HandRight].Position.Y < body.Joints[JointType.Head].Position.Y && body.Joints[JointType.HandRight].Position.Y > body.Joints[JointType.SpineBase].Position.Y)
                {
                    // //Right hand left of Right Shoulder
                    if (body.Joints[JointType.HandRight].Position.X > body.Joints[JointType.ShoulderRight].Position.X)
                    {
                        return GesturePartResult.Succeeded;
                    }

                    return GesturePartResult.Undetermined;
                }

                return GesturePartResult.Failed;
            }

            return GesturePartResult.Failed;
        }
    }
}
