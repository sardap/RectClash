using RectClash.Misc;
using RectClash.SFMLComs;
using SFML.Graphics;
using SFML.System;

namespace RectClash.ECS
{
	public class Camera
	{
		private View _view;

		private Vector2f _postion { get; set; }
		private Vector2f _zoom { get; set; }

		private Transform _localToWorldTransform = Transform.Identity;
		private Transform _worldToLocalTransform = Transform.Identity;

		public View View
		{
			get => _view;
		}

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
				if(InveserveDirty)
				{
					InveserveDirty = false;

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

		public float ZoomY
		{
			get => Zoom.Y;
			set
			{
				Zoom = new Vector2f(ZoomX, value);
				SetDirty();
			}
		}

		public float ZoomX
		{
			get => Zoom.X;
			set
			{
				Zoom = new Vector2f(value, ZoomY);
				SetDirty();
			}
		}


		public bool Dirty
		{
			get;
			set;
		}

		public bool InveserveDirty
		{
			get;
			set;
		}


		public Camera()
		{
			_view = new View();
			_view.Size = Engine.Instance.Window.Size;
			Zoom = new Vector2f(1, 1);
		}

		private void SetDirty()
		{
			Dirty = true;
			InveserveDirty = true;
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