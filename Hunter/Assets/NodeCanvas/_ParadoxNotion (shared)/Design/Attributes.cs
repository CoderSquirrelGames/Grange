using System;


namespace ParadoxNotion.Design{

	///To exclude a class from being listed. Note: Abstract classes are not listed anyway.
	[AttributeUsage(AttributeTargets.Class)]
	public class DoNotListAttribute : Attribute{}

	///Use for friendlier names if class name is weird for any reason
	[AttributeUsage(AttributeTargets.Class)]
	public class NameAttribute : Attribute{
		public string name;
		public NameAttribute(string name){
			this.name = name;
		}
	}

	///Use to categorize classes
	[AttributeUsage(AttributeTargets.Class)]
	public class CategoryAttribute : Attribute{
		public string category;
		public CategoryAttribute(string category){
			this.category = category;
		}
	}

	///Use to give a description to a class
	[AttributeUsage(AttributeTargets.Class)]
	public class DescriptionAttribute : Attribute{
		public string description;
		public DescriptionAttribute(string description){
			this.description = description;
		}
	}

	///When a class is associated with an icon
	[AttributeUsage(AttributeTargets.Class)]
	public class IconAttribute : Attribute{
		public string iconName;
		public IconAttribute(string iconName){
			this.iconName = iconName;
		}
	}	

	///Makes the int field show as layerfield
	[AttributeUsage(AttributeTargets.Field)]
	public class LayerFieldAttribute : Attribute{}

	///Makes the string field show as tagfield
	[AttributeUsage(AttributeTargets.Field)]
	public class TagFieldAttribute : Attribute{}

	///Makes the string field show as text field with specified height
	[AttributeUsage(AttributeTargets.Field)]
	public class TextAreaFieldAttribute : Attribute{
		public float height;
		public TextAreaFieldAttribute(float height){
			this.height = height;
		}
	}

    ///Makes the float field show as slider
	[AttributeUsage(AttributeTargets.Field)]
	public class SliderFieldAttribute : Attribute{
		public float left;
		public float right;
		public SliderFieldAttribute(float left, float right){
			this.left = left;
			this.right = right;
		}
	}

	///Helper attribute. Designates that the field is required not to be null or string.empty
	[AttributeUsage(AttributeTargets.Field)]
	public class RequiredFieldAttribute : Attribute{}

}