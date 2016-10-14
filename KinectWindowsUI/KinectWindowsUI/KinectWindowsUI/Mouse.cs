using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Threading;

namespace KinectWindowsUI
{
	class Mouse
	{
		public static void MouseLeftDown()
		{
			mouse_event(MouseEventFlag.LeftDown, 0, 0, 0, UIntPtr.Zero);
		}
		public static void MouseLeftUp()
		{
			mouse_event(MouseEventFlag.LeftUp, 0, 0, 0, UIntPtr.Zero);
		}

		public static void DoMouseClick()
		{
			mouse_event(MouseEventFlag.LeftDown | MouseEventFlag.LeftUp, 0, 0, 0, UIntPtr.Zero);
		}

		public static void DoMouseDoubleClick()
		{
			mouse_event(MouseEventFlag.LeftDown | MouseEventFlag.LeftUp, 0, 0, 0, UIntPtr.Zero);
			mouse_event(MouseEventFlag.LeftDown | MouseEventFlag.LeftUp, 0, 0, 0, UIntPtr.Zero);
		}


        public static void DoMouseMove(int x, int y)
        {
            mouse_event(MouseEventFlag.Move, x, y, 0, UIntPtr.Zero);
        }

        [DllImport("user32.dll")]
		public static extern bool SetCursorPos(int X, int Y);
		[DllImport("user32.dll")]
		static extern void mouse_event(MouseEventFlag flags, int dx, int dy, uint data, UIntPtr extraInfo);

		[Flags]
		enum MouseEventFlag : uint
		{
			Move = 0x0001,
			LeftDown = 0x0002,
			LeftUp = 0x0004,
			RightDown = 0x0008,
			RightUp = 0x0010,
			MiddleDown = 0x0020,
			MiddleUp = 0x0040,
			XDown = 0x0080,
			XUp = 0x0100,
			Wheel = 0x0800,
			VirtualDesk = 0x4000,
			Absolute = 0x8000
		}

		/// <summary>
		/// Struct representing a point.
		/// </summary>
		[StructLayout(LayoutKind.Sequential)]
		public struct POINT
		{
			public int X;
			public int Y;

			public static implicit operator Point(POINT point)
			{
				return new Point(point.X, point.Y);
			}
		}

		/// <summary>
		/// Retrieves the cursor's position, in screen coordinates.
		/// </summary>
		/// <see>See MSDN documentation for further information.</see>
		[DllImport("user32.dll")]
		public static extern bool GetCursorPos(out POINT lpPoint);

		public static Point GetCursorPosition()
		{
			POINT lpPoint;
			GetCursorPos(out lpPoint);

			return lpPoint;
		}

		//struct MouseInput
		//{
		//    public int X; // X coordinate
		//    public int Y; // Y coordinate
		//    public uint MouseData; // mouse data, e.g. for mouse wheel
		//    public uint DwFlags; // further mouse data, e.g. for mouse buttons
		//    public uint Time; // time of the event
		//    public IntPtr DwExtraInfo; // further information
		//}

		///// <summary>
		///// super structure for input data of the function SendInput
		///// </summary>
		//struct Input
		//{
		//    public int Type; // type of the input, 0 for mouse  
		//    public MouseInput Data; // mouse data
		//}

		//private MouseInput CreateMouseInput(int x, int y, uint data, uint time, uint flag)
		//{
		//    // create from the given data an object of the type MouseInput, which then can be send
		//    MouseInput Result = new MouseInput();
		//    Result.X = x;
		//    Result.Y = y;
		//    Result.MouseData = data;
		//    Result.Time = time;
		//    Result.DwFlags = flag;
		//    return Result;
		//}

		//[DllImport("user32.dll")]
		//private static extern uint SendInput(uint nInputs, Input[] pInputs, int cbSize);

		//const uint MOUSEEVENTF_MOVE = 0x0001; // move mouse

		//private void SimulateMouseMove(int x, int y)
		//{

		//Input[] MouseEvent = new Input[1];
		//MouseEvent[0].Type = 0;
		//    // move mouse: Flags ABSOLUTE (whole screen) and MOVE (move)
		//    MouseEvent[0].Data = CreateMouseInput(x, y, 0, 0, MOUSEEVENTF_MOVE);
		//    SendInput((uint)MouseEvent.Length, MouseEvent, Marshal.SizeOf(MouseEvent[0].GetType()));
		//}
	}
}
