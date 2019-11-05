using RectClash.Game.Perf;
using RectClash.Misc;
using RectClash.SFMLComs;
using SFML.Graphics;
using SFML.System;

namespace RectClash.ECS
{
	public class Camera
	{
		private Vector2f _postion { get; set; }
		private Vector2f _zoom { get; set; }

		private Transform _localToWorldTransform = Transform.Identity;
		private Transform _worldToLocalTransform = Transform.Identity;

		public readonly Subject<string, PerfEvents> Subject = new Subject<string, PerfEvents>();

		public Transform LocalToWorldTransform
		{
			get 
			{
				if(Dirty)
				{
					Dirty = false;
					
					var translate = Transform.Identity;
					translate.Translate(Postion);
					var scale = Transform.Identity;
					scale.Scale(Zoom);

					_localToWorldTransform = Utility.MultiplyTrans(translate, scale);
				}

				return _localToWorldTransform;
			}
		}

		public Transform WorldToLocalTransform
		{
			get
			{
				if(_inverseDirty)
				{
					_inverseDirty = false;

					_worldToLocalTransform = LocalToWorldTransform.GetInverse();
				}

				return _worldToLocalTransform;
			}
		}

		public Vector2f Postion 
		{ 
			get => _postion;
			set
			{
				_postion = value;
				SetDirty();
			}
		}

		public float X
		{
			get => Postion.X;
			set => Postion = new Vector2f(value, Y);
		}

		public float Y
		{
			get => Postion.Y;
			set => Postion = new Vector2f(X, value);
		}

		public Vector2f Zoom 
		{ 
			get => _zoom;
			set
			{
				_zoom = value;
				SetDirty();
			}
		}

		public bool Dirty
		{
			get;
			private set;
		}

		private bool _inverseDirty
		{
			get;
			set;
		}


		public Camera()
		{
			Postion = new Vector2f();
			Zoom = new Vector2f(1, 1);
		}

		private void SetDirty()
		{
			Dirty = true;
			_inverseDirty = true;
			Subject.Notify("Zoom:" + _zoom.X.ToString("0.##"), PerfEvents.CAMERA_ZOOMED);
			Subject.Notify("CameraPostion(" + _postion.X.ToString("0.##") + "," + _postion.Y.ToString("0.##") + ")", PerfEvents.CAMERA_MOVED);
		}

		public Vector2f TransformLocalToWorldPoint(Transform transform, float x, float y)
		{
			transform.Combine(LocalToWorldTransform.GetInverse());
			return transform.TransformPoint(x, y);
		}

		public Vector2f TransformLocalToWorldPoint(Transform transform, Vector2f postion)
		{
			return TransformLocalToWorldPoint(transform, postion.X, postion.Y);
		}
	}
}