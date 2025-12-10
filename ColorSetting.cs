using Unity.Collections.LowLevel.Unsafe;

namespace Flexy.GameSettings;

public struct ColorSetting
{
	public ColorSetting ( String key, Color32 defaultValue, Boolean readLater = false ) 
	{
		_key		= "Flexy.GameSettings  Color " + key;
		_default	= defaultValue;
		_value		= readLater ? _default : ReadValue(_key, defaultValue);
		Changed		= null;
	}

	private readonly	String		_key;
	private readonly	Color32		_default;
	private				Color32		_value; 
	
	public event Action<Color32>?	Changed;

	public	Boolean		HasValue	=> SGS.Store.HasKey( _key );

	public	Color32		Read		( ) => _value = ReadValue(_key, _default);
	public	Color32		Get			( ) => _value;
	public	void		Set			( Color32 value )
	{ 
		if (_value.Equals(value))
			return;

		_value = value;
		SGS.Store.SetInt(_key, UnsafeUtility.As<Color32, Int32>(ref _value));

		try						{ Changed?.Invoke(value); }
		catch (Exception ex)	{ Debug.LogException(ex); }
	}
	public	void		SetDefault	( ) => Set(_default);
	
	public static	implicit operator Color32	( ColorSetting @this ) => @this._value;
	public static	implicit operator Color		( ColorSetting @this ) => @this._value;
	
	private static Color32	ReadValue	( String key, Color32 @default )	
	{
		var raw	 = SGS.Store.GetInt(key, UnsafeUtility.As<Color32, Int32>(ref @default));
		return UnsafeUtility.As<Int32, Color32>(ref raw);
	}
}