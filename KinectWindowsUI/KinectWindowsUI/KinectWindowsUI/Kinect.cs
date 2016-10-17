using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

using System.Windows;
using System.Windows.Threading;
using Microsoft.Kinect;
///using Microsoft.Office.Interop.PowerPoint;
using ppoint = Microsoft.Office.Interop.PowerPoint;

namespace KinectWindowsUI
{
	class Kinect
	{
		/// Active Kinect sensor
		KinectSensor sensor;
		/// Reader for body frames
		BodyFrameReader bodyFrameReader;
		/// Array for the bodies
		private Body[] bodies = null;
		/// Screen width and height for determining the exact mouse sensitivity
		int screenWidth, screenHeight;
		GestureController _gestureController;

		/// timer for pause-to-click feature
		DispatcherTimer timer = new DispatcherTimer();

		/// How far the cursor move according to your hand's movement
		public float mouseSensitivity = MOUSE_SENSITIVITY;

		/// Time required as a pause-clicking
		public float timeRequired = TIME_REQUIRED;
		/// The radius range your hand move inside a circle for [timeRequired] seconds would be regarded as a pause-clicking
		public float pauseThresold = PAUSE_THRESOLD;
		/// Decide if the user need to do clicks or only move the cursor
		public bool doClick = DO_CLICK;
		/// Use Grip gesture to click or not
		public bool useGripGesture = USE_GRIP_GESTURE;
		/// Value 0 - 0.95f, the larger it is, the smoother the cursor would move
		public float cursorSmoothing = CURSOR_SMOOTHING;
		public bool experimental = EXPERIMENTAL;

		// Default values
		public const float MOUSE_SENSITIVITY = 2.5f;
		public const float TIME_REQUIRED = 2f;
		public const float PAUSE_THRESOLD = 60f;
		public const bool DO_CLICK = true;
		public const bool USE_GRIP_GESTURE = true;
		public const float CURSOR_SMOOTHING = 0.54f;
		public const bool EXPERIMENTAL = false;

		bool debug = false;
		string _pointer = "MOUSE";

		/// Determine if we have tracked the hand and used it to move the cursor,
		/// If false, meaning the user may not lift their hands, we don't get the last hand position and some actions like pause-to-click won't be executed.
		bool alreadyTrackedPos = false;

		/// for storing the time passed for pause-to-click
		float timeCount = 0;
		/// For storing last cursor position
		Point lastCurPos = new Point(0, 0);

		/// If true, user did a left hand Grip gesture
		bool wasLeftGrip = false;
		/// If true, user did a right hand Grip gesture
		bool wasRightGrip = false;
		bool wasRightLasso = false;
		bool wasLeftLasso = false;
        bool game = false;
        bool first_skeleton = true;
        int prev_x;
        int prev_y;

		private int activeBodyIndex = -1;

		public Kinect()
		{
			 // get Active Kinect Sensor
			sensor = KinectSensor.GetDefault();
			// open the reader for the body frames
			bodyFrameReader = sensor.BodyFrameSource.OpenReader();
			bodyFrameReader.FrameArrived += bodyFrameReader_FrameArrived;

			_gestureController = new GestureController();
			_gestureController.GestureRecognized += GestureController_GestureRecognized;

			// get screen with and height
			screenWidth = (int)SystemParameters.PrimaryScreenWidth;
			screenHeight = (int)SystemParameters.PrimaryScreenHeight;

			// set up timer, execute every 0.1s
			timer.Interval = new TimeSpan(0, 0, 0, 0, 100);
			timer.Tick += new EventHandler(Timer_Tick);
			timer.Start();

			// open the sensor
			sensor.Open();
		}

		/// Pause to click timer
		void Timer_Tick(object sender, EventArgs e)
		{
			if (!doClick || useGripGesture) return;

			if (!alreadyTrackedPos)
			{
				timeCount = 0;
				return;
			}

			Point curPos = Mouse.GetCursorPosition();

			if ((lastCurPos - curPos).Length < pauseThresold)
			{
				if ((timeCount += 0.1f) > timeRequired)
				{
					Mouse.DoMouseClick();
					timeCount = 0;
				}
			}
			else
			{
				timeCount = 0;
			}

			lastCurPos = curPos;
		}

		void bodyFrameReader_FrameArrived(object sender, BodyFrameArrivedEventArgs e)
		{
			bool dataReceived = false;

			using (BodyFrame bodyFrame = e.FrameReference.AcquireFrame())
			{
				if (bodyFrame != null)
				{
					if (this.bodies == null)
					{
						this.bodies = new Body[bodyFrame.BodyCount];
					}

					// The first time GetAndRefreshBodyData is called, Kinect will allocate each Body in the array.
					// As long as those body objects are not disposed and not set to null in the array,
					// those body objects will be re-used.
					bodyFrame.GetAndRefreshBodyData(this.bodies);
					dataReceived = true;
					if (activeBodyIndex != -1)
					{
						Body body = bodies[activeBodyIndex];
						if (!body.IsTracked)
						{
							activeBodyIndex = -1;
						}
					}
					if (activeBodyIndex == -1)
					{
                        if (first_skeleton)
                        {
                            for (int i = 0; i < bodies.Length; i++)
                            {
                                Body body = bodies[i];
                                if (body.IsTracked)
                                {
                                    activeBodyIndex = i;
                                    Debug.Print("Tracked Body");
                                    // No need to continue loop
                                    break;
                                }
                            }
                        }
                        else
                        {
                            float closest_z = 5;
                            for (int i = 0; i < bodies.Length; i++)
                            {
                                Body body = bodies[i];
                                if (body.IsTracked)
                                {
                                    CameraSpacePoint spineBase = body.Joints[JointType.SpineBase].Position;
                                    float temp = spineBase.Z;
                                    if (temp < closest_z)
                                    {
                                        activeBodyIndex = i;
                                    }
                                }
                            }
                            Debug.Print("Tracked Body");
                        }
					}
					if (activeBodyIndex != -1)
					{
						Body body = bodies[activeBodyIndex];
						// Do stuff with known active body.
						//        }
						//    }
						//}

						if (!dataReceived)
						{
							alreadyTrackedPos = false;
							return;
						}

						//foreach (Body body in this.bodies)
						{

							// get first tracked body only, notice there's a break below.
							if (body.IsTracked)
							{
								if (body.HandRightState == HandState.Open)
								{
									_gestureController.Update(body);
								}
								else
								{
									_gestureController.GestureReset();
								}
								if (body.HandRightState == HandState.Lasso )//&& body.HandRightState == HandState.Lasso)
								{
									if (doClick && useGripGesture)
									{
										if (body.HandLeftState == HandState.Closed)
										{
											if (!wasLeftGrip)
											{
												Mouse.MouseLeftDown();
												wasLeftGrip = true;
											}
										}
										else if (body.HandLeftState == HandState.Open)
										{
											if (wasLeftGrip)
											{
												Mouse.MouseLeftUp();
												wasLeftGrip = false;
											}
										}

										if (body.HandLeftState == HandState.Lasso)
										{
											if (!wasLeftLasso)
											{
												Mouse.DoMouseDoubleClick();
												wasLeftLasso = true;
											}
										}
										else if (body.HandLeftState == HandState.Open)
										{
											if (wasLeftLasso)
											{
												Mouse.MouseLeftUp();
												wasLeftLasso = false;
											}
										}
									}
								//}
                                //if (body.HandLeftState == HandState.Lasso && body.HandRightState != HandState.Lasso)
								//{
									// get various skeletal positions
									CameraSpacePoint handLeft = body.Joints[JointType.HandLeft].Position;
									CameraSpacePoint handRight = body.Joints[JointType.HandRight].Position;
									CameraSpacePoint spineBase = body.Joints[JointType.SpineBase].Position;

									if (handRight.Z - spineBase.Z < -0.15f) // if right hand lift forward
									{
										/* hand x calculated by this. we don't use shoulder right as a reference cause the shoulder right
										 * is usually behind the lift right hand, and the position would be inferred and unstable.
										 * because the spine base is on the left of right hand, we plus 0.05f to make it closer to the right. */
										float x = handRight.X - spineBase.X + 0.05f;
										/* hand y calculated by this. ss spine base is way lower than right hand, we plus 0.51f to make it
										 * higer, the value 0.51f is worked out by testing for a several times, you can set it as another one you like. */
										float y = spineBase.Y - handRight.Y + 0.51f;
										// get current cursor position
										Point curPos = Mouse.GetCursorPosition();
										// smoothing for using should be 0 - 0.95f. The way we smooth the cusor is: oldPos + (newPos - oldPos) * smoothValue
										float smoothing = 1 - cursorSmoothing;
                                        // set cursor position
                                        //Mouse.SetCursorPos((int)(curPos.X + (x * mouseSensitivity * screenWidth - curPos.X) * smoothing), (int)(curPos.Y + ((y + 0.25f) * mouseSensitivity * screenHeight - curPos.Y) * smoothing));
                                        //Mouse.SetCursorPos((int)((x)), (int)(((y))));

                                        if (!game)
                                        {
                                            Mouse.SetCursorPos((int)(curPos.X + (x * mouseSensitivity * screenWidth - curPos.X) * smoothing), (int)(curPos.Y + ((y + 0.25f) * mouseSensitivity * screenHeight - curPos.Y) * smoothing));
                                        }
                                        else
                                        {
                                            Mouse.DoMouseMove((int)(curPos.X + (x * mouseSensitivity - curPos.X) * smoothing), (int)(curPos.Y + ((y + 0.25f) * mouseSensitivity- curPos.Y) * smoothing));
                                        }
                                        alreadyTrackedPos = true;
                                    
										// Grip gesture
										//if (doClick && useGripGesture)
										//{
										//	if (body.HandRightState == HandState.Closed)
										//	{
										//		if (!wasRightGrip)
										//		{
										//			Mouse.MouseLeftDown();
										//			wasRightGrip = true;
										//		}
										//	}
										//	else if (body.HandRightState == HandState.Open)
										//	{
										//		if (wasRightGrip)
										//		{
										//			Mouse.MouseLeftUp();
										//			wasRightGrip = false;
										//		}
										//	}
										//}
									}

									//else if (handLeft.Z - spineBase.Z < -0.15f) // if left hand lift forward
									//{
									//    float x = handLeft.X - spineBase.X + 0.3f;
									//    float y = spineBase.Y - handLeft.Y + 0.51f;
									//    Point curPos = MouseKeyBoardControl.GetCursorPosition();
									//    float smoothing = 1 - cursorSmoothing;
									//    MouseKeyBoardControl.SetCursorPos((int)(curPos.X + (x * mouseSensitivity * screenWidth - curPos.X) * smoothing), (int)(curPos.Y + ((y + 0.25f) * mouseSensitivity * screenHeight - curPos.Y) * smoothing));
									//    alreadyTrackedPos = true;

									//    if (doClick && useGripGesture)
									//    {
									//        if (body.HandLeftState == HandState.Closed)
									//        {
									//            if (!wasLeftGrip)
									//            {
									//                MouseKeyBoardControl.MouseLeftDown();
									//                wasLeftGrip = true;
									//            }
									//        }
									//        else if (body.HandLeftState == HandState.Open)
									//        {
									//            if (wasLeftGrip)
									//            {
									//                MouseKeyBoardControl.MouseLeftUp();
									//                wasLeftGrip = false;
									//            }
									//        }
									//    }
									//}
									//else if (handLeft.X - spineBase.X > 0.3f)
									//{
									//    MouseKeyBoardControl.KeyDown();
									//}

									else
									{
										wasLeftGrip = true;
										wasRightGrip = true;
                                        wasLeftLasso = true;
                                        wasRightLasso = true;
										alreadyTrackedPos = false;
									}

									// get first tracked body only

									//break;
								}
								else
								{
									wasLeftGrip = true;
									wasRightGrip = true;
                                    wasLeftLasso = true;
                                    wasRightLasso = true;
                                    alreadyTrackedPos = false;
								}
							}
						}
					}
				}
                else
                {
                    Debug.Print("No body");
                }
			}
		}

		void GestureController_GestureRecognized(object sender, GestureEventArgs e)
		{
			///tblGestures.Text = e.GestureType.ToString();
			if (e.GestureType == GestureType.SwipeLeft)
			{
				Keyboard.KeyLeft();
				if (debug)
					MessageBox.Show("Left");
			}
		   else if (e.GestureType == GestureType.SwipeRight)
			{
				Keyboard.KeyRight();
				if (debug)
					MessageBox.Show("Right");
			}
			else if (e.GestureType == GestureType.SwipeDown)
			{
				Keyboard.KeyDown();
				if (debug)
					MessageBox.Show("Down");
			}
			else if (e.GestureType == GestureType.SwipeUp)
			{
				Keyboard.KeyUp();
				if (debug)
					MessageBox.Show("Up");
			}
			else if (e.GestureType == GestureType.ZoomIn)
			{
				//Keyboard.KeyUp();
				if (debug)
					MessageBox.Show("ZoomIn");
			}
			else if (e.GestureType == GestureType.ZoomIn)
			{
				//Keyboard.KeyUp();
				if (debug)
					MessageBox.Show("ZoomOut");
			}
			if(experimental)
			{
				if (e.GestureType == GestureType.Menu)
				{
					Keyboard.KeyESC();
				}
				if (e.GestureType == GestureType.JoinedHands)
				{
					if (_pointer == "MOUSE")
					{
						_pointer = "PEN";
					}
					else
					{
						_pointer = "MOUSE";
					}
				}
				if (e.GestureType == GestureType.ZoomIn)
				{
					//((dynamic)Windows.).LaserPointerEnabled = true;
					//ppoint.PpSlideShowPointerType.ppSlideShowPointerPen.Equals(true);
					//SlideShowWindows(1).View.EraseDrawing;

				}
				if (e.GestureType == GestureType.ZoomOut)
				{
					//((dynamic)Windows.).LaserPointerEnabled = true;
					//ppoint.PpSlideShowPointerType.ppSlideShowPointerEraser.Equals(true);
					//SlideShowWindows(1).View.EraseDrawing;
				}
			}
		}

		public void Close()
		{
			if (timer != null)
			{
				timer.Stop();
				timer = null;
			}

			if (this.sensor != null)
			{
				this.sensor.Close();
				this.sensor = null;
			}
		}
	}
}
