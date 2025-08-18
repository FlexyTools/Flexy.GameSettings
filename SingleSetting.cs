namespace Flexy.GameSettings;

public struct SingleSetting
{
	public SingleSetting (String key, Single defaultValue)
	{
		_key     = key;
		_default = defaultValue;
		_value   = SGS.Serializer.GetFloat(_key, _default);
		Changed  = null;
	}

	private readonly	String _key;
	private				Single _default;
	private				Single _value; 
	
	public event Action<Single> Changed;

	public Boolean HasValue => SGS.Serializer.HasKey( _key );

	public Single Get	( )
	{ 
		return _value;
	}
	public void Set	( Single value )
	{ 
		if(_value == value)
			return;

		_value = value;
		SGS.Serializer.SetFloat(_key, value);
		
		try						{ Changed?.Invoke( value ); }
		catch (Exception ex)	{ Debug.LogException( ex ); }
	}
	public void Clear() => Set( _default );

	public static	implicit operator Single ( SingleSetting @this ) => @this._value;
}