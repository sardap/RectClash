using System.Linq;
using RectClash.Misc;
using SFML.Graphics;

namespace RectClash.ECS.Graphics
{
	// Stolen https://www.gamedev.net/articles/programming/math-and-physics/making-a-game-engine-transformations-r3566/
	public class PostionCom : Com
	{
		
		private Transform _trans = Transform.Identity;

		private Transform _localToWorldMatrix = Transform.Identity;

		private Transform _worldToLocalMatrix  = Transform.Identity;

		private float _localRotation = 0.0f;

		private bool _isDirty = true;

		private bool _isInverseDirty  = true;


		public SFML.System.Vector2f _postion = new SFML.System.Vector2f(0f, 0f);

		public SFML.System.Vector2f _localScale = new SFML.System.Vector2f(1f, 1f);

		public SFML.System.Vector2f _size = new SFML.System.Vector2f();

		public Transform LocalToWorldMatrix 
		{
			get
			{
				if(_isDirty)
				
				{
					if(Owner.Parent == null)
					{
						_localToWorldMatrix = CalculateLocalToParentMatrix();
					}
					else
					{
						_localToWorldMatrix = Owner.Parent.PostionCom.LocalToWorldMatrix * CalculateLocalToParentMatrix();
					}

					_isDirty = false;
				}

				return _localToWorldMatrix;
			}
		}

		public SFML.System.Vector2f LocalPostion
		{
			get => _postion;
			set
			{
				_postion = value;
				SetDirty();	
			}
		}

		public SFML.System.Vector2f LocalScale
		{
			get => _localScale;
			set
			{
				_localScale = value;
				SetDirty();
			}
		}

		public SFML.System.Vector2f WorldPostion
		{
			get => TransformPoint(LocalPostion);
		}

		public float LocalX
		{
			get => LocalPostion.X;
			set => LocalPostion = new SFML.System.Vector2f(value, LocalY);
		}

		public float LocalY
		{
			get => LocalPostion.Y;
			set => LocalPostion = new SFML.System.Vector2f(LocalX, value);
		}

		public FloatRect WorldRect
		{
			get
			{
				var worldScale = LocalToWorldMatrix.TransformPoint(LocalScale);
				return new FloatRect(WorldPostion.X, WorldPostion.Y, worldScale.X, worldScale.Y);
			}
		}

		public FloatRect Rect
		{
			get
			{
				return new FloatRect(LocalPostion.X, LocalPostion.Y, LocalScale.X, LocalScale.Y);
			}
		}


		// https://www.gamedev.net/articles/programming/math-and-physics/making-a-game-engine-transformations-r3566/
		private void SetDirty()
		{
			if(!_isDirty)
			{
				_isDirty = true;
				_isInverseDirty = true;

				foreach (var child in Owner.Children)
				{
					child.PostionCom.SetDirty();	
				}
			}
		}

		public Transform CalculateLocalToParentMatrix()
		{
			var translate = Transform.Identity;
			translate.Translate(LocalPostion);
			var rotate = Transform.Identity;
			rotate.Rotate(_localRotation);
			var scale = Transform.Identity;
			scale.Scale(LocalScale);

			return Utility.MultiplyTrans(translate, rotate, scale);
		}

		public Transform GetWorldToLocalMatrix()
		{
			if(_isInverseDirty)
			{
				_worldToLocalMatrix = LocalToWorldMatrix.GetInverse();

				_isInverseDirty = false;
			}

			return _worldToLocalMatrix;
		}

		public SFML.System.Vector2f TransformPoint(SFML.System.Vector2f point)
		{
			return LocalToWorldMatrix.TransformPoint(point);
		}

		public void ParentChanged()
		{
			SetDirty();
		}

		public PostionCom()
		{
		}
	}
}