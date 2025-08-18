namespace Flexy.GameSettings;

public struct Int32Setting
{
	public Int32Setting (String key, Int32 defaultValue)
	{
		_key     = "Flexy.GameSettings  Int " + key;
		_default = defaultValue;
		_value   = SGS.Serializer.GetInt(_key, _default);
		Changed  = null;
	}

	private readonly String _key;
	private readonly Int32  _default;
	private          Int32  _value; 
	
	public event Action<Int32> Changed;

	public Boolean HasValue => SGS.Serializer.HasKey( _key );

	public Int32 Get	( )
	{ 
		return _value;
	}
	public void Set	( Int32 value )
	{ 
		if (_value == value )
			return;

		_value = value;
		SGS.Serializer.SetInt(_key, value);

		try						{ Changed?.Invoke( value ); }
		catch (Exception ex)	{ Debug.LogException( ex ); }
	}
	public void Clear() => Set( _default );
	
	public static	implicit operator Int32 ( Int32Setting @this ) => @this._value;
}