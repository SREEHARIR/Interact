﻿using LightBuzz.Vitruvius.Gestures;
using Microsoft.Kinect;
using System;
using System.Linq;
using System.Collections.Generic;

namespace KinectWindowsUI
{
    /// <summary>
    /// Represents a gesture controller.
    /// </summary>
    public class GestureController : BaseController<Body>
    {
        #region Members

        /// <summary>
        /// A list of all the gestures the controller is searching for.
        /// </summary>
        private List<Gesture> _gestures = new List<Gesture>();

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of <see cref="GestureController"/> with all of the available gesture types.
        /// </summary>
        public GestureController()
        {
            foreach (GestureType t in Enum.GetValues(typeof(GestureType)))
            {
                AddGesture(t);
            }
        }

        /// <summary>
        /// Initializes a new instance of <see cref="GestureController"/> with the specified gesture type.
        /// </summary>
        /// <param name="type">The gesture type to recognize.</param>
        public GestureController(GestureType type)
        {
            AddGesture(type);
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs when a gesture is recognized.
        /// </summary>
        public event EventHandler<GestureEventArgs> GestureRecognized;

        #endregion

        #region Methods

        /// <summary>
        /// Updates all gestures.
        /// </summary>
        /// <param name="body">The body data to search for gestures.</param>
        public override void Update(Body body)
        {
            base.Update(body);

            foreach (Gesture gesture in _gestures)
            {
                gesture.Update(body);
            }
        }

        /// <summary>
        /// Adds the specified gesture for recognition.
        /// </summary>
        /// <param name="type">The predefined <see cref="GestureType" />.</param>
        public void AddGesture(GestureType type)
        {
            // Check whether the gesure is already added.
            if (_gestures.Where(g => g.GestureType == type).Count() > 0) return;

            IGestureSegment[] segments = null;

            // DEVELOPERS: If you add a new predefined gesture with a new GestureType,
            // simply add the proper segments to the switch statement here.
            switch (type)
            {
                case GestureType.JoinedHands:
                    segments = new IGestureSegment[20];

                    JoinedHandsSegment1 joinedhandsSegment = new JoinedHandsSegment1();
                    for (int i = 0; i < 20; i++)
                    {
                        segments[i] = joinedhandsSegment;
                    }
                    break;
                case GestureType.Menu:
                    segments = new IGestureSegment[20];

                    MenuSegment1 menuSegment = new MenuSegment1();
                    for (int i = 0; i < 20; i++)
                    {
                        segments[i] = menuSegment;
                    }
                    break;
                //case GestureType.SwipeDown:
                //    segments = new IGestureSegment[3];

                //    segments[0] = new SwipeDownSegment1();
                //    segments[1] = new SwipeDownSegment2();
                //    segments[2] = new SwipeDownSegment3();
                //    break;
                //case GestureType.SwipeLeft:
                //    segments = new IGestureSegment[3];

                //    segments[0] = new SwipeLeftSegment1();
                //    segments[1] = new SwipeLeftSegment2();
                //    segments[2] = new SwipeLeftSegment3();
                //    break;
                //case GestureType.SwipeRight:
                //    segments = new IGestureSegment[3];

                //    segments[0] = new SwipeRightSegment1();
                //    segments[1] = new SwipeRightSegment2();
                //    segments[2] = new SwipeRightSegment3();
                //    break;
                //case GestureType.SwipeUp:
                //    segments = new IGestureSegment[3];

                //    segments[0] = new SwipeUpSegment1();
                //    segments[1] = new SwipeUpSegment2();
                //    segments[2] = new SwipeUpSegment3();
                //    break;

                case GestureType.SwipeDown:
                    segments = new IGestureSegment[2];

                    segments[0] = new SwipeUpSegment2();
                    segments[1] = new SwipeUpSegment1();
                    //segments[2] = new SwipeUpSegment1();
                    break;
                case GestureType.SwipeUp:
                    segments = new IGestureSegment[2];

                    segments[0] = new SwipeUpSegment1();
                    segments[1] = new SwipeUpSegment2();
                    //segments[2] = new SwipeUpSegment3();
                    break;
                case GestureType.SwipeLeft:
                    segments = new IGestureSegment[2];

                    segments[0] = new SwipeLeftSegment1();
                    segments[1] = new SwipeLeftSegment2();
                    //segments[2] = new SwipeLeftSegment3();
                    break;
                case GestureType.SwipeRight:
                    segments = new IGestureSegment[2];

                    segments[0] = new SwipeLeftSegment2();
                    segments[1] = new SwipeLeftSegment1();
                    //segments[2] = new SwipeLeftSegment1();
                    break;

                case GestureType.WaveLeft:
                    segments = new IGestureSegment[6];

                    WaveLeftSegment1 waveLeftSegment1 = new WaveLeftSegment1();
                    WaveLeftSegment2 waveLeftSegment2 = new WaveLeftSegment2();

                    segments[0] = waveLeftSegment1;
                    segments[1] = waveLeftSegment2;
                    segments[2] = waveLeftSegment1;
                    segments[3] = waveLeftSegment2;
                    segments[4] = waveLeftSegment1;
                    segments[5] = waveLeftSegment2;
                    break;
                case GestureType.WaveRight:
                    segments = new IGestureSegment[6];

                    WaveRightSegment1 waveRightSegment1 = new WaveRightSegment1();
                    WaveRightSegment2 waveRightSegment2 = new WaveRightSegment2();

                    segments[0] = waveRightSegment1;
                    segments[1] = waveRightSegment2;
                    segments[2] = waveRightSegment1;
                    segments[3] = waveRightSegment2;
                    segments[4] = waveRightSegment1;
                    segments[5] = waveRightSegment2;
                    break;
                case GestureType.ZoomIn:
                    segments = new IGestureSegment[3];

                    segments[0] = new ZoomSegment1();
                    segments[1] = new ZoomSegment2();
                    segments[2] = new ZoomSegment3();
                    break;
                case GestureType.ZoomOut:
                    segments = new IGestureSegment[3];

                    segments[0] = new ZoomSegment3();
                    segments[1] = new ZoomSegment2();
                    segments[2] = new ZoomSegment1();
                    break;
                default:
                    break;
            }

            if (segments != null)
            {
                Gesture gesture = new Gesture(type, segments);
                gesture.GestureRecognized += OnGestureRecognized;

                _gestures.Add(gesture);
            }
        }

        #endregion

        #region Event handlers

        /// <summary>
        /// Handles the GestureRecognized event of the g control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GestureEventArgs"/> instance containing the event data.</param>
        private void OnGestureRecognized(object sender, GestureEventArgs e)
        {
            if (GestureRecognized != null)
            {
                GestureRecognized(this, e);
            }

            foreach (Gesture gesture in _gestures)
            {
                gesture.Reset();
            }
        }

        public void GestureReset()
        {
            foreach (Gesture gesture in _gestures)
            {
                gesture.Reset();
            }
        }

        #endregion
    }
}
