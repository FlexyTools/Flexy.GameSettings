using Unity.Collections.LowLevel.Unsafe;

namespace Flexy.GameSettings;

public struct ColorSetting : IClearable
{
	public ColorSetting (String key, Color32 defaultValue) 
	{
		_key     = "Flexy.GameSettings  Color " + key;
		_default = defaultValue;
		var raw	 = SGS.Serializer.GetInt(_key, UnsafeUtility.As<Color32, Int32>(ref _default));
		_value   = UnsafeUtility.As<Int32, Color32>(ref raw);
		Changed  = null;
	}

	private readonly String _key;
	private readonly Color32  _default;
	private          Color32  _value; 
	
	public event Action<Color32> Changed;

	public Boolean	HasValue => SGS.Serializer.HasKey( _key );

	public void		Clear	( ) => Set( _default );
	public Color32	Get		( ) => _value;
	public void		Set		( Color32 value )
	{ 
		if (_value.Equals(value) )
			return;

		_value = value;
		SGS.Serializer.SetInt(_key, UnsafeUtility.As<Color32, Int32>(ref _value) );

		try						{ Changed?.Invoke( value ); }
		catch (Exception ex)	{ Debug.LogException( ex ); }
	}
	
	public static	implicit operator Color32	( ColorSetting @this ) => @this._value;
	public static	implicit operator Color		( ColorSetting @this ) => @this._value;
}